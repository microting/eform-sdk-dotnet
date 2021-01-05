#!/bin/bash
./armprepareinstall.sh
dotnet restore
dotnet build --no-restore eFormSDK.sln
dotnet test --no-restore -c Release -v n eFormSDK.Integration.Base.SqlControllerTests/eFormSDK.Integration.Base.SqlControllerTests.csproj
