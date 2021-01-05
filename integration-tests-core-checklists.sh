#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.Integration.CheckLists.CoreTests/eFormSDK.Integration.CheckLists.CoreTests.csproj
