#!/bin/bash
./armprepareinstall.sh
dotnet restore
dotnet build --no-restore eFormSDK.sln
dotnet test --no-restore -c Release -v n eFormSDK.Tests/eFormSDK.Tests.csproj --logger:Console || travis_terminate 1;
