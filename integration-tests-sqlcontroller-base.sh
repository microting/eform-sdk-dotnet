#!/bin/bash
./armprepareinstall.sh
dotnet test --no-restore -c Release -v n eFormSDK.Integration.Base.SqlControllerTests/eFormSDK.Integration.Base.SqlControllerTests.csproj
