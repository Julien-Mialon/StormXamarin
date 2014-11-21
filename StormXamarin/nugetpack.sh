#!/bin/bash

mkdir nuget_packages

cd Storm.Mvvm
nuget pack Storm.Mvvm.csproj -Prop Configuration=Release -IncludeReferencedProjects
mv *.nupkg ../nuget_packages
cd -

cd Storm.Mvvm.Android
nuget pack Storm.Mvvm.Android.csproj -Prop Configuration=Release -IncludeReferencedProjects
mv *.nupkg ../nuget_packages
cd -