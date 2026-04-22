"""Automated NuGet dependency updater for eFormCore/Microting.eForm.csproj.

Reads all PackageReference entries from the project, checks each against NuGet
for the latest non-preview release, applies every needed bump as a single
atomic edit to the csproj, then verifies the result with `dotnet restore`.

On success: creates one GitHub issue summarizing all bumps, stages only the
csproj, commits, tags, and pushes.

On restore failure: rolls back the csproj via `git checkout` and exits
non-zero without creating an issue, commit, or tag.
"""

import os
import re
import subprocess
import sys
from xml.etree import ElementTree as ET

import requests

GITHUB_REPO_OWNER = "microting"
GITHUB_REPO_NAME = "eform-sdk-dotnet"
PROJECT_NAME = "eFormCore/Microting.eForm.csproj"

GITHUB_ACCESS_TOKEN = os.getenv("CHANGELOG_GITHUB_TOKEN")


def read_package_references(csproj_path):
    tree = ET.parse(csproj_path)
    refs = []
    for pr in tree.getroot().iter("PackageReference"):
        name = pr.attrib.get("Include")
        version = pr.attrib.get("Version")
        if name and version:
            refs.append((name, version))
    return refs


def get_latest_stable_version(package_name):
    url = f"https://api.nuget.org/v3-flatcontainer/{package_name.lower()}/index.json"
    response = requests.get(url, timeout=30)
    if response.status_code != 200:
        return None
    stable = [v for v in response.json().get("versions", []) if "-" not in v]
    return stable[-1] if stable else None


def update_csproj_versions(csproj_path, bumps):
    with open(csproj_path, "r", encoding="utf-8") as f:
        content = f.read()
    for name, _old, new in bumps:
        pattern = re.compile(
            r'(<PackageReference\s+Include="'
            + re.escape(name)
            + r'"\s+Version=")[^"]+(")'
        )
        content, n = pattern.subn(r"\g<1>" + new + r"\g<2>", content)
        if n == 0:
            raise RuntimeError(
                f"No PackageReference with an inline Version attribute found for {name}"
            )
    with open(csproj_path, "w", encoding="utf-8") as f:
        f.write(content)


def create_github_issue(bumps):
    plural = "s" if len(bumps) != 1 else ""
    title = f"Bump {len(bumps)} NuGet package{plural}"
    body_lines = ["The following packages were updated:", ""]
    for name, old, new in bumps:
        body_lines.append(f"- `{name}`: {old} -> {new}")
    body = "\n".join(body_lines)

    headers = {
        "Authorization": f"Bearer {GITHUB_ACCESS_TOKEN}",
        "Accept": "application/vnd.github.v3+json",
    }
    response = requests.post(
        f"https://api.github.com/repos/{GITHUB_REPO_OWNER}/{GITHUB_REPO_NAME}/issues",
        headers=headers,
        json={"title": title, "body": body},
    )
    if response.status_code != 201:
        raise RuntimeError(f"Failed to create GitHub issue: {response.text}")
    issue_number = response.json()["number"]
    print(f"GitHub issue '{title}' created. Issue Number: {issue_number}")

    for label in (".Net", "backend", "enhancement"):
        label_response = requests.post(
            f"https://api.github.com/repos/{GITHUB_REPO_OWNER}/{GITHUB_REPO_NAME}/issues/{issue_number}/labels",
            headers=headers,
            json={"labels": [label]},
        )
        if label_response.status_code == 200:
            print(f"Label '{label}' added to the issue.")
        else:
            print(f"Failed to add label '{label}' to the issue.")
    return issue_number


def commit_csproj(csproj_path, issue_number):
    subprocess.run(["git", "add", csproj_path], check=True)
    subprocess.run(["git", "commit", "-m", f"closes #{issue_number}"], check=True)


def push_new_version_tag():
    tags_output = (
        subprocess.check_output(["git", "tag", "--sort=-creatordate"])
        .decode("utf-8")
        .strip()
    )
    if not tags_output:
        print("No tags found in the repository.")
        return
    latest = tags_output.splitlines()[0].lstrip("v")
    major, minor, build = map(int, latest.split("."))
    new_tag = f"v{major}.{minor}.{build + 1}"
    print(f"Current Git Version: {latest}. Creating new tag {new_tag}.")
    subprocess.run(["git", "tag", new_tag], check=True)
    subprocess.run(["git", "push", "--tags"], check=True)
    subprocess.run(["git", "push"], check=True)


def main():
    commits_before = len(
        subprocess.check_output(["git", "log", "--oneline"])
        .decode("utf-8")
        .splitlines()
    )
    print("Current number of commits:", commits_before)

    bumps = []
    for name, current in read_package_references(PROJECT_NAME):
        if "-" in current:
            print(f"Skipping {name}: pinned to pre-release ({current}).")
            continue
        print(f"Checking {name}")
        latest = get_latest_stable_version(name)
        if latest is None:
            print(f"Failed to retrieve package information for {name}.")
            continue
        print(f"Current version {name} is: {current} and latest version is: {latest}")
        if latest == current:
            print(f"The installed version of {name} is already up to date.")
            continue
        bumps.append((name, current, latest))
        print(f"Planned bump: {name} {current} -> {latest}")

    if not bumps:
        print("Nothing to do, everything is up to date.")
        return

    update_csproj_versions(PROJECT_NAME, bumps)

    restore = subprocess.run(
        ["dotnet", "restore", PROJECT_NAME],
        capture_output=True,
        text=True,
    )
    if restore.returncode != 0:
        print("dotnet restore failed after applying bumps. Rolling back csproj.")
        print(restore.stdout)
        print(restore.stderr, file=sys.stderr)
        subprocess.run(["git", "checkout", "--", PROJECT_NAME], check=True)
        sys.exit(1)

    issue_number = create_github_issue(bumps)
    commit_csproj(PROJECT_NAME, issue_number)
    push_new_version_tag()


if __name__ == "__main__":
    main()
