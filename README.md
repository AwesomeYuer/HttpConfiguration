# HttpConfiguration

- Remote Http Configuration `misc.settings.json`

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


# Deploy files to App Service
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

Connect-AzAccount

# Publish-AzWebApp -ResourceGroupName Default-Web-WestUS -Name MyApp -ArchivePath <zip-package-path> 

Publish-AzWebApp -ResourceGroupName ea-misc-001-rg -Name AzAppServiceDeployTest -ArchivePath "D:\MyGitHub\AzAppServiceDeployTest\AzAppServiceDeployTest\obj\Release\net6.0\PubTmp\AzAppServiceDeployTest-20231030021323085.zip"

```

# GetEnvironmentVariablesAsIEnumerable