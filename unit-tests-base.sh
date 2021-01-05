#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.Base.Tests/eFormSDK.Base.Tests.csproj
