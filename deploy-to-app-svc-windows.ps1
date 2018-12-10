<#
.SYNOPSIS
    Deploys application to Azure App Service
.NOTES
    
#>

param (
    [string]$RG = "appsvcdemo-" + [System.Guid]::NewGuid().ToString(), 
    [string]$DeploymentName = "deployment-" + [System.Guid]::NewGuid().ToString(),    
    [string]$PlanName,
    [string]$WebAppName,
    [string]$ConnString = "Data Source=" + [System.Environment]::MachineName + ";Initial Catalog=WideWorldImporters;Persist Security Info=True;User ID=sa;Password=P@55w0rd2018"
)

# actual deployment command
az group deployment create --resource-group $RG -n $DeploymentName --parameters AppServicePlanName=$PlanName WebAppName=$WebAppName  SQLConnectionString=$ConnString --template-file .\deploy\template.json