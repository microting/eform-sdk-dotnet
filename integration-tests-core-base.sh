#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.Integration.Base.CoreTests/eFormSDK.Integration.Base.CoreTests.csproj
