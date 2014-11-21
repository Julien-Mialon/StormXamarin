#!/bin/bash

if [ $# -ne 1 ]; then
	echo "Usage : nugetpack.sh [all|Mvvm.Android|Mvvm]"
	exit;
fi

mkdir nuget_packages 2>>/dev/null

if [[ $1 == all || $1 == Mvvm ]]; then
	cd Storm.Mvvm
	echo -n "Generating Stom.Mvvm..."
	nuget pack Storm.Mvvm.csproj -Prop Configuration=Release -Verbosity quiet -NonInteractive -IncludeReferencedProjects
	mv *.nupkg ../nuget_packages
	cd ..
	echo "				Done !"
fi 

if [[ $1 == all || $1 == Mvvm.Android ]]; then
	cd Storm.Mvvm.Android
	echo -n "Generating Storm.Mvvm.Android..."
	nuget pack Storm.Mvvm.Android.csproj -Prop Configuration=Release -Verbosity quiet -NonInteractive -IncludeReferencedProjects
	mv *.nupkg ../nuget_packages
	cd ..
	echo "		Done !"
fi

echo ""
echo -n "Adding new packages to git tracker..."
git add -f nuget_packages/*.nupkg
echo "		Done !"