<#
.SYNOPSIS
    Deploys application to Azure App Service
.NOTES
    
#>


param (
    [string]$RG = "appsvcdemo-" + [System.Guid]::NewGuid().ToString(), 
    [switch]$CreateRGIfNotExists,
    [string]$DeploymentName = "deployment-" + [System.Guid]::NewGuid().ToString(),    
    [string]$PlanName,
    [string]$WebAppName,
    [string]$ConnString = "Data Source=" + [System.Environment]::MachineName + ";Initial Catalog=WideWorldImporters;Persist Security Info=True;User ID=sa;Password=P@55w0rd2018",
    [string]$DisplayTitleForSite = "Demo Site",
    [string]$StorageAccountName = "netcoreminiappstorage",
    [switch]$DeployDatabase,
    [string]$SqlServerName,
    [string]$SqlDatabaseName,
    [string]$SqlUserName = "SqlAdmin",
    [string]$SqlPassword = ""
)

$Location = "westeurope"
$StorageContainerNameForSqlImport = "sqldbimport"                # when changed, change also ARM template
$StorageBlobNameForSqlImport = "wideworldimportersdb.bacpac"     # when changed, change also ARM template

# create RG if not exists
if($CreateRGIfNotExists){
    $RGExistsAlready = az group exists --name $RG
    Write-Output "Resource Group exists? $RGExistsAlready"
    if($RGExistsAlready -Eq $False){
        Write-Output "Creating Resource Group: $RG"
        az group create -l $Location --name $RG
        Write-Output "Resource Group Created: $RG"
    }
}

$DeployDbArmParam = "false"

# when SQL is to be deployed
if($DeployDatabase){
    $DeployDbArmParam = "true"

    # prepare password if not provided
    if([String]::IsNullOrWhiteSpace($SqlPassword)){
        Write-Output "Generating password for Azure SQL DB"
        Add-Type -AssemblyName System.Web
        $SqlPassword = [System.Web.Security.Membership]::GeneratePassword(15,0)
        Write-Output "Password Generated: $SqlPassword"
    }

    # create storage account
    # original location: https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/wide-world-importers
    Write-Output "Creating Storage Account"
    az storage account create -g $RG -n $StorageAccountName -l $Location --sku Standard_LRS
    Write-Output "Storage Account created"

    Write-Output "Creating container: 'sqldbimport'"
    az storage container create -n $StorageContainerNameForSqlImport --account-name $StorageAccountName
    Write-Output "Container created"

    Write-Output "Uploading SQL BacPac file to storage (also see: https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/wide-world-importers)"
    # Use below line for import towards Azure SQL DB Standard:
      az storage blob upload --account-name $StorageAccountName -f ./deploy/WideWorldImporters-Standard.bacpac -c $StorageContainerNameForSqlImport -n $StorageBlobNameForSqlImport
    # Use below line for import towards Azure SQL DB Premium:
    # az storage blob upload --account-name $StorageAccountName -f ./deploy/WideWorldImporters-Full.bacpac -c $StorageContainerNameForSqlImport -n $StorageBlobNameForSqlImport
    Write-Output "Done uploading SQL BacPac"

    $SasTokenForSqlImport = az storage blob generate-sas --account-name $StorageAccountName --container-name $StorageContainerNameForSqlImport  -n $StorageBlobNameForSqlImport --permissions r --expiry (Get-Date).AddMinutes(20).ToString("yyyy-MM-dTH:mZ")
    Write-Output "Sas-token for reading bacpac during import: $SasTokenForSqlImport"
}

# actual deployment command
Write-Output "About to deploy template"
az group deployment create --resource-group $RG -n $DeploymentName --parameters appServicePlanName=$PlanName webAppName=$WebAppName  sqlConnectionString=$ConnString siteDisplayTitle=$DisplayTitleForSite deployAzureSqlDb=$DeployDbArmParam sqlServerName=$SqlServerName sqlServerUserName=$SqlUserName sqlServerDatabaseName=$SqlDatabaseName sqlServerPassword=`"$SqlPassword`" storageAccountName=$StorageAccountName sqlServerSasTokenForImport=$SasTokenForSqlImport --template-file .\deploy\template.json --verbose
Write-Output "Template deployed"