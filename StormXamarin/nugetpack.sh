#!/bin/bash

if [ $# -ne 2 ]; then
	echo "Usage : nugetpack.sh [all|Mvvm|Mvvm.Forms|Mvvm.Android|Mvvm.Tablet] [Push|No]"
	exit;
fi

mkdir nuget_packages 2>>/dev/null
cd nuget_build

if [[ $1 == all || $1 == Mvvm ]]; then
	cd Storm.Mvvm
	echo -n "Generating Storm.Mvvm..."
	nuget pack -Verbosity quiet -NonInteractive
	echo "				Done !"
	if [[ $2 == Push ]]; then
		echo -n "Uploading Storm.Mvvm..."
		nuget push *.nupkg
		echo "				Done !"
	fi
	mv *.nupkg ../../nuget_packages
	cd ..
fi 

if [[ $1 == all || $1 == Mvvm.Forms ]]; then
	cd Storm.Mvvm.Forms
	echo -n "Generating Storm.Mvvm.Forms..."
	nuget pack -Verbosity quiet -NonInteractive
	echo "		Done !"
	if [[ $2 == Push ]]; then
		echo -n "Uploading Storm.Mvvm.Forms..."
		nuget push *.nupkg
		echo "				Done !"
	fi
	mv *.nupkg ../../nuget_packages
	cd ..
fi

if [[ $1 == all || $1 == Mvvm.Android ]]; then
	cd Storm.Mvvm.Android
	echo -n "Generating Storm.Mvvm.Android..."
	nuget pack -Verbosity quiet -NonInteractive
	echo "		Done !"
	if [[ $2 == Push ]]; then
		echo -n "Uploading Storm.Mvvm.Android..."
		nuget push *.nupkg
		echo "				Done !"
	fi
	mv *.nupkg ../../nuget_packages
	cd ..
fi

if [[ $1 == all || $1 == Mvvm.Tablet ]]; then
	cd Storm.Mvvm.Tablet
	echo -n "Generating Storm.Mvvm.Tablet..."
	nuget pack -Verbosity quiet -NonInteractive
	echo "				Done !"
	if [[ $2 == Push ]]; then
		echo -n "Uploading Storm.Mvvm.Tablet..."
		nuget push *.nupkg
		echo "				Done !"
	fi
	mv *.nupkg ../../nuget_packages
	cd ..
fi 

cd ..
echo ""
echo -n "Adding new packages to git tracker..."
git add -f nuget_packages/*.nupkg
echo "		Done !"