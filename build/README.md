# Build Scripts

## LocalConfig.ps1

The software can be built and packaged by running ```BuildClient.ps1``` in PowerShell.

```BuildClient.ps1``` relies on a file called ```LocalConfig.ps1``` in the same folder. This contains details that are specific to a user's own workstation - e.g. the location of Visual Studio or their local Git repository. ```LocalConfig.ps1``` is included in the ```.gitignore``` file, so you won't see it on GitHub.

An example ```LocalConfig.ps1``` is included. Just copy ```LocalConfig-example.ps1``` as ```LocalConfig.ps1``` and customise the values to suit your environment.

## Advanced Installer

The installer is built using [Advanced Installer](http://www.advancedinstaller.com/).