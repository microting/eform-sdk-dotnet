#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.Integration.Case.CoreTests/eFormSDK.Integration.Case.CoreTests.csproj
