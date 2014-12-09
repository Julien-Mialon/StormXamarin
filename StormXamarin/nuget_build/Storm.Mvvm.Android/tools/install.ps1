param($installPath, $toolsPath, $package, $project)

Write-Host "Installing build event for project " + $project.FullName

$index = $toolsPath.LastIndexOf("packages")
$subTools = $toolsPath.Substring($index)

$buildEvent = "`$(SolutionDir)" + $subTools + "\Preprocessor\Android.BindingPreprocessor.exe `$(ProjectDir)ActivityDescription.json"

$currentPostBuildCmd = $project.Properties.Item("PreBuildEvent").Value

# Append our post build command if it's not already there
if (!$currentPostBuildCmd.Contains($buildEvent)) {
    $project.Properties.Item("PreBuildEvent").Value += $buildEvent
}