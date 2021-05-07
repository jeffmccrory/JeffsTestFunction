#create a resource group
$resourceGroup = "JeffsTestFunctionResource"
$location ="eastus"
az group create -n $resourceGroup -l $location

# function app name
$functionAppName = "JeffsTestFuncApp"

# to deploy the function app
az group deployment create -g $resourceGroup --template-file azuredeploy.json --parameters appName=$functionAppName

# deploy the projects functions to the cloud app
func azure functionapp publish JeffsTestFuncApp

#see whats in the resource group created
az resource list -g $resourceGroup -o table

# check the app settings were configured correctly
az functionapp config appsettings list -n $functionAppName -g $resourceGroup -o table

# publish and deploy the functionapp
func azure functionapp publish JeffsTestFuncApp

# to clean up when we're done
az group delete -n $resourceGroup --no-wait -y 