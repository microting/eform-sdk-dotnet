#!/bin/bash
./armprepareinstall.sh
dotnet restore
dotnet build --no-restore eFormSDK.sln
export GITVERSION=`git describe --abbrev=0 --tags | cut -d "v" -f 2`
echo $GITVERSION
dotnet pack eFormSDK.sln -c Release -o ./artifacts -p:PackageVersion=$GITVERSION
export FILENAME="/home/travis/build/microting/eform-sdk-dotnet/eFormCore/artifacts/Microting.eForm.${GITVERSION}.nupkg"
echo $FILENAME
dotnet nuget push $FILENAME -k $NUGET_SECRET_KEY -s https://api.nuget.org/v3/index.json || true
