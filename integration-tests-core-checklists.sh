#!/bin/bash
./armprepareinstall.sh
dotnet restore
dotnet build --no-restore eFormSDK.sln
dotnet test --no-restore -c Release -v n eFormSDK.Integration.CheckLists.CoreTests/eFormSDK.Integration.CheckLists.CoreTests.csproj
