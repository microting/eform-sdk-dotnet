#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.CheckLists.Tests/eFormSDK.CheckLists.Tests.csproj
