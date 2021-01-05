#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.Integration.CheckLists.SqlControllerTests/eFormSDK.Integration.CheckLists.SqlControllerTests.csproj
