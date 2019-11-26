#!/bin/bash
./armprepareinstall.sh
dotnet restore
dotnet build --no-restore eFormSDK.sln
dotnet test --no-restore -c Release -v n eFormSDK.Integration.Tests/eFormSDK.Integration.Tests.csproj || travis_terminate 1;
