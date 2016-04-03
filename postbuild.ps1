param($projectName, $buildConfiguration, $buildStagingDirectory)
 
$VerbosePreference = "continue"
$ErrorActionPreference = "Stop"
     
&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}
$globalJson = Get-Content -Path $PSScriptRoot\global.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore
 
if($globalJson)
{
    $dnxVersion = $globalJson.sdk.version
}
else
{
    Write-Warning "Unable to locate global.json to determine using 'latest'"
    $dnxVersion = "latest"
}
 
& $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -Persistent
 
$dnxRuntimePath = "$($env:USERPROFILE)\.dnx\runtimes\dnx-clr-win-x86.$dnxVersion"
     
#& "dnu" "build" "$PSScriptRoot\src\$projectName" "--configuration" "$buildConfiguration"
Write-Warning "Path is $PSScriptRoot\src\$projectName"
 
& "dnu" "publish" "$PSScriptRoot\src\$projectName" "--configuration" "$buildConfiguration" "--out" "$buildStagingDirectory" "--runtime" "$dnxRuntimePath"