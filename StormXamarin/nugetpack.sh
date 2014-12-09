#!/bin/bash

if [ $# -ne 2 ]; then
	echo "Usage : nugetpack.sh [all|Mvvm.Android|Mvvm] [Push|No]"
	exit;
fi

mkdir nuget_packages 2>>/dev/null
cd nuget_build

if [[ $1 == all || $1 == Mvvm ]]; then
	cd Storm.Mvvm
	echo -n "Generating Stom.Mvvm..."
	#nuget pack Storm.Mvvm.csproj -Prop Configuration=Release -Verbosity quiet -NonInteractive -IncludeReferencedProjects
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

if [[ $1 == all || $1 == Mvvm.Android ]]; then
	cd Storm.Mvvm.Android
	echo -n "Generating Storm.Mvvm.Android..."
	#nuget pack Storm.Mvvm.Android.csproj -Prop Configuration=Release -Verbosity quiet -NonInteractive -IncludeReferencedProjects
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

cd ..
echo ""
echo -n "Adding new packages to git tracker..."
git add -f nuget_packages/*.nupkg
echo "		Done !"