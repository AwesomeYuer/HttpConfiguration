# HttpConfiguration

- Remote Http Configuration `misc.settings.remote.json`

  ```json
  {
	 "misc_a" : "8888",
	 "misc" : {
		"keY111" : "remote",
		"key2" : 9999,
		"key3" : true
     }
  }
  ```

# Visual Studio Publish `Zip` Profile
https://gist.github.com/brianpursley/671b1909e2359d2a0ab1ba004f7e5ea7

```xml
  <Target Name="ZipPublishOutput" AfterTargets="FileSystemPublish">
    <Exec Command="del $(publishUrl)\..\$(MSBuildProjectName).zip" />
    <ZipDirectory SourceDirectory="$(publishUrl)" DestinationFile="$(publishUrl)\..\$(MSBuildProjectName).zip" />
    <RemoveDir Directories="$(publishUrl)" />
  </Target>
```


# Deploy `Zip` file to `App Service` by using `Azure PowerShell`
https://learn.microsoft.com/en-us/azure/app-service/deploy-zip?tabs=powershell

```powershell

# https://www.powershellgallery.com/

$PSVersionTable.PSVersion

# https://github.com/PowerShell/PowerShell/releases/tag/v7.2.16
# https://github.com/PowerShell/PowerShell/releases/download/v7.2.16/PowerShell-7.2.16-win-x64.msi

$PSVersionTable.PSVersion

Get-ExecutionPolicy -List

Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

Get-ExecutionPolicy -List

Install-Module -Name Az -Repository PSGallery -Force

Install-Module -Name Az.Accounts -AllowClobber

Install-Module -Name Az.Websites

# https://learn.microsoft.com/en-us/powershell/module/az.accounts/connect-azaccount?view=azps-10.4.1
Connect-AzAccount

# https://learn.microsoft.com/en-us/powershell/module/az.websites/publish-azwebapp?view=azps-10.4.1
# Publish-AzWebApp -ResourceGroupName Default-Web-WestUS -Name MyApp -ArchivePath <zip-package-path> 
Publish-AzWebApp -ResourceGroupName AzResourceGroupName -Name AzWebAppServiceName -ArchivePath "d:\xxxxxx.zip" -Force

```

# Deploy Azure `WebJobs Zip File` by using `Azure CLI`
  https://mariankostal.com/2020/07/17/deploy-azure-webjob-using-powershell/

  https://azcliprod.blob.core.windows.net/msi/azure-cli-2.53.1-x64.msi

  `deploy.ps1`

```powershell
[string] $directory="."
[string] $csproj="SampleHost"

[string] $buildOutput = "$directory\$csproj\bin\Release\net6.0"
[string] $resourceGroupName = "resourceGroupName"
[string] $webAppName = "AzAppServiceName"
[string] $webJobName = "zzWebJob-001"
[string] $webJobType = "triggered"

$processDir = "$directory\WebJobsPackaging" 
$webJobDir = "$processDir\App_Data\jobs\$webJobType\$webJobName"

Remove-Item $webJobDir -Recurse -Force
New-Item $webJobDir -ItemType Directory

dotnet build $directory\$csproj\$csproj.csproj -c Release
Copy-Item "$buildOutput\*" -Destination $webJobDir -Recurse -Force

Compress-Archive -Path "$processDir\*" -DestinationPath ".\$webJobName.zip" -Force
Remove-Item $processDir -Recurse -Force
 
az webapp deployment source config-zip -g $resourceGroupName -n $webAppName --src "$webJobName.zip"

```

# GetEnvironmentVariablesAsIEnumerable