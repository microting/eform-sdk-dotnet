import requests
from datetime import datetime, timedelta, timezone
import subprocess
import os
from dateutil import parser

# GitHub repository information
GITHUB_REPO_OWNER = "microting"
GITHUB_REPO_NAME = "eform-sdk-dotnet"
PROJECT_NAME = "eFormCore/Microting.eForm.csproj"
# List of packages to check
PACKAGES = ['AWSSDK.Core', 'AWSSDK.S3', 'AWSSDK.SQS', 'Microsoft.EntityFrameworkCore', 'Microsoft.EntityFrameworkCore.Relational', 'Microsoft.EntityFrameworkCore.Design']

GITHUB_ACCESS_TOKEN = os.getenv("CHANGELOG_GITHUB_TOKEN")


def get_installed_version(package_name):
    # Execute 'dotnet list' command to get the installed packages
    # Run 'dotnet list package' command
    dotnet_list_output = subprocess.check_output(["dotnet", "list", "package"]).decode("utf-8")
    
    # Find lines containing the package name
    package_lines = [line for line in dotnet_list_output.splitlines() if package_name in line]
    
    # Extract the version numbers
    for line in package_lines:
        #print(f"Looking af "+line)
        parts = line.split()
        version_index = parts.index(package_name) + 1
        #print(version_index)
        version = parts[version_index]
        #print(version)
        if "-preview" not in version:
            return version

def create_github_issue(package_name, old_version, new_version):
    # GitHub API URLs for creating an issue and adding labels
    create_issue_url = f"https://api.github.com/repos/{GITHUB_REPO_OWNER}/{GITHUB_REPO_NAME}/issues"
    add_labels_url = f"https://api.github.com/repos/{GITHUB_REPO_OWNER}/{GITHUB_REPO_NAME}/issues/{{issue_number}}/labels"
    
    # Create the issue title and body
    issue_title = f"Bump {package_name} from {old_version} to {new_version}"
    issue_body = f"Please update the package {package_name} from version {old_version} to version {new_version}."
    
    # Create the request headers with the GitHub access token
    headers = {
        "Authorization": f"Bearer {GITHUB_ACCESS_TOKEN}",
        "Accept": "application/vnd.github.v3+json"
    }
    
    # Create the request payload with the issue title and body
    data = {
        "title": issue_title,
        "body": issue_body
    }
    
    # Send the POST request to create the GitHub issue
    response = requests.post(create_issue_url, headers=headers, json=data)
    
    if response.status_code == 201:
        issue_data = response.json()
        issue_number = issue_data["number"]
        print(f"GitHub issue '{issue_title}' created successfully. Issue Number: {issue_number}")
        
        # Add labels to the created issue
        labels = ['.Net', 'backend', 'enhancement']
        add_labels_url = add_labels_url.replace("{issue_number}", str(issue_number))
        
        for label in labels:
            label_data = {"labels": [label]}
            response = requests.post(add_labels_url, headers=headers, json=label_data)
            
            if response.status_code == 200:
                print(f"Label '{label}' added to the issue.")
            else:
                print(f"Failed to add label '{label}' to the issue.")
        
        # Commit the changes with a message referencing the issue number
        commit_message = f"closes #{issue_number}"
        os.system("git add .")
        os.system(f'git commit -a -m "{commit_message}"')
    else:
        print(f"Failed to create GitHub issue. Response: {response.text}")


def check_new_nuget_version(package_name):
    # Construct the NuGet package URL
    package_url = f"https://api.nuget.org/v3-flatcontainer/{package_name.lower()}/index.json"
    #print(package_url)
    
    # Get the package metadata
    response = requests.get(package_url)
    print(f"Checking {package_name}")
    if response.status_code == 200:
        package_data = response.json()
        
        # Get the latest version and publication time
        versions = package_data["versions"]
        #print(f"Versions for {package_name} is {versions}")
        latest_version = versions[-1]
        for line in versions:
            if "-preview" not in line:
                latest_version = line
        
        # Check if the latest version is alread installed
        installed_version = get_installed_version(package_name)
        print(f"Current version {package_name} is: {installed_version} and latest version is: {latest_version}")
        if installed_version:
            if installed_version != latest_version:
                if "-preview" not in latest_version:
                    print(f"The installed version of {package_name} has a change from: "+installed_version+" to: "+latest_version)
                    subprocess.run(['dotnet','add',PROJECT_NAME,'package',package_name])
                    create_github_issue(package_name, installed_version, latest_version)
            else:
                print(f"The installed version of {package_name} is already up to date.")
        else:
            print(f"{package_name} is not installed in the project.")
    else:
        print(f"Failed to retrieve package information for {package_name}.")


# Execute 'git log' command to get the commit log
output = subprocess.check_output(["git", "log", "--oneline"])
output = output.decode("utf-8")

# Count the number of lines in the commit log
current_number_of_commits = len(output.splitlines())

print("Current number of commits:", current_number_of_commits)

# Iterate over the packages
for package in PACKAGES:
    check_new_nuget_version(package)

# Execute 'git log' command to get the commit log
output = subprocess.check_output(["git", "log", "--oneline"])
output = output.decode("utf-8")

# Count the number of lines in the commit log
new_number_of_commits = len(output.splitlines())

print("New number of commits:", new_number_of_commits)

if new_number_of_commits > current_number_of_commits:
# Obtain the current git version
    output = subprocess.check_output(["git", "tag", "--sort=-creatordate"]).strip().decode("utf-8")
    tags = output.split("\n")

    if tags:
        # Extract the first tag
        latest_tag = tags[0]
        
        # Remove the leading 'v' from the tag
        current_git_version = latest_tag.lstrip("v")
        
        print("Current Git Version:", current_git_version)
    else:
        print("No tags found in the repository.")
    
    # Extract major, minor, and build versions from the current git version
    major_version, minor_version, build_version = map(int, current_git_version.split("."))
    build_version += 1
    
    # Create the new git version
    new_git_version = f"v{major_version}.{minor_version}.{build_version}"
    
    # Create the new git tag
    subprocess.run(["git", "tag", new_git_version])
    
    # Push the new git tag and commit
    subprocess.run(["git", "push", "--tags"])
    subprocess.run(["git", "push"])
else:
    print("Nothing to do, everything is up to date.")