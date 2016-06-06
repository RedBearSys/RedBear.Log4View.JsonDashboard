$ScriptPath = Split-Path $MyInvocation.MyCommand.Path
& "$ScriptPath\LocalConfig.ps1"

Set-Location $log4view

echo "Building release..."
& "$vs\devenv.com" "$log4view\AzureServiceBus.sln" /Rebuild "Debug|Any Cpu" | Out-Null

$fileversion = (Get-Command "$log4view\bin\Debug\Log4View.AzureServiceBus.dll").FileVersionInfo.Fileversion

$res = $fileversion -match "(?<msi>[0-9]+\.[0-9]+\.[0-9]+)"
$msiversion = $matches["msi"]
$folder = (Get-ChildItem "C:\Program Files (x86)\Caphyon" | where { $_.psiscontainer } | Select-Object -First 1).Name
$cmd = "C:\Program Files (x86)\Caphyon\" + $folder + "\bin\x86\AdvancedInstaller.com"

echo "Updating the installer..."
&$cmd /edit "$log4view\..\Setup\Plugin.aip" /SetVersion $msiversion
&$cmd /edit "$log4view\..\Setup\Plugin.aip" /SetProperty ARMDisplayName="Azure Service Bus Plugin for Log4View $fileversion"
&$cmd /rebuild "$log4view\..\Setup\Plugin.aip"

Write-Host "Press any key to continue ..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")