#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.InSight.Tests/eFormSDK.InSight.Tests.csproj
