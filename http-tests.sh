#!/bin/bash
cd "$(dirname "$0")"

echo "Running HTTP resilience tests..."
dotnet test eFormSDK.Http.Tests/eFormSDK.Http.Tests.csproj --configuration Release --no-restore --logger:"console;verbosity=normal"
