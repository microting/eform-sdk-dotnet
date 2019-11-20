#!/bin/bash
ARCH=`dpkg --print-architecture`
ARMARCH="arm64"

if [ $ARCH = $ARMARCH ]; then
	echo "WE ARE ON ARM"
	wget https://download.visualstudio.microsoft.com/download/pr/89fb60b1-3359-414e-94cf-359f57f37c7c/256e6dac8f44f9bad01f23f9a27b01ee/dotnet-sdk-3.0.101-linux-arm64.tar.gz
	mkdir -p $HOME/dotnet && tar zxf dotnet-sdk-3.0.101-linux-arm64.tar.gz -C $HOME/dotnet
	export DOTNET_ROOT=$HOME/dotnet
	export PATH=$PATH:$HOME/dotnet
else
	echo "WE ARE NOT ON ARM"
	echo $ARCH
fi
