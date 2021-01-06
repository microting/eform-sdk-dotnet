#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.Integration.Case.SqlControllerTests/eFormSDK.Integration.Case.SqlControllerTests.csproj
