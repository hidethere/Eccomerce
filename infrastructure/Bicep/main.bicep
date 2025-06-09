param env string 
param apimSubnetName string = 'apimSubnet-${env}'
param containerAppsSubnetName string =  'containerAppsSubnet-${env}'
param vmSubnetName string = 'vmSubnet-${env}'
param vnetName string = 'vnet-Eccomerce-${env}'
param location string = resourceGroup().location

param sqlServerName string = 'sqlServer-Eccomerce-${env}-v2'
param cosmosAccountName string = 'cosmosacc-eccomerce-${env}-v2'
param redisName string = 'redisEccomerce${env}v2'
param sbName string = 'sb-Eccomerce-${env}-v2'
param eventHubName string = 'eventHub-Eccommerce-${env}-v2'
param keyVaultName string = 'keyVault-Eccomerce-${env}v3'
param apimName string = 'apim-Eccomerce-${env}v2'
param acrName string // injected from the pipeline
param keyvaultUri string // injected from the pipeline


// Acr
param image string // Injected from the pipeline
param BuildId string // Injected from the pipeline
param revisionSuffix string = 'rev-${BuildId}'

// ACA
param containerAppsEnvName string = 'containerAppsEnv-Eccomerce-${env}'
param containerAppsName string

// Vnet

module vnet 'modules/network.bicep' = {
  params: {
    vnetName: vnetName
    apimSubnetName: apimSubnetName
    containerAppsSubnetName: containerAppsSubnetName
    location: location
    vmSubnetName: vmSubnetName
  }
} 

//Identity
module acaUserIdentity 'modules/uami.bicep' = {
  name: 'uamiDeploy'
  params: {
    name: 'aca-uami-${env}'
    location: location
  }
}


// Container Registry
module containerRegistry 'modules/acr.bicep' = {
  name: 'containerRegistryDeploy-${env}'
  params: {
    acrName: acrName
    location: location
  }
}



// Azure SQL
module azureSql 'modules/azureSql.bicep' = {
  name: 'azureSqlDeploy-${env}'
  params: {
    sqlServerName: sqlServerName
    AdminLogin: 'UserDiogo1'
    adminPassword: 'Diogo1234!'
    location: location
  }
}

// Cosmos DB
module cosmosDb 'modules/cosmosDb.bicep' = {
  name: 'cosmosDbDeploy-${env}'
  params: {
    cosmosAccountName: cosmosAccountName
    location: location
  }
}

// Redis Cache
module redisCache 'modules/redis.bicep' = {
  name: 'redisCacheDeploy-${env}'
  params: {
    redisName:redisName
    location: location
  }
}


// Service Bus
module serviceBus 'modules/serviceBus.bicep' = {
  name: 'serviceBusDeploy-${env}'
  params: {
    sbNameSpace: sbName
    location: location
  }
}

// Event Hub
module eventHub 'modules/eventhub.bicep' = {
  name: 'eventHubDeploy-${env}1'
  params: {
    eventHubNameSpace: eventHubName
    //schemaRegistryName: 'schemaRegistry-Eccommerce-${env}'
    location: location
  }
}

// Key Vault
module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVaultDeploy-${env}02'
  params: {
    keyVaultName: keyVaultName
    location: location
  }
}


// Container Apps Env
module containerAppsEnv 'modules/acaenv.bicep' = {
  name: 'containerAppsEnvDeploy-${env}-1'
  params: {
    vnetId: vnet.outputs.vnetId
    containerAppsEnvName: containerAppsEnvName
    subnetName: containerAppsSubnetName
    location: location
  }
}

// Azure Container Apps
module containerApps 'modules/aca.Bicep' = {
  params: {
    acrLoginServer: containerRegistry.outputs.acrLoginServer
    containerAppName: containerAppsName
    environmentName: containerAppsEnvName
    acrName: containerRegistry.outputs.acrName
    image: image // injected from the pipeline
    location: location
    revisionSuffix: revisionSuffix // inject from the pipeline
    userIdentityId: acaUserIdentity.outputs.uamiId
    userIdentityPrincipalId: acaUserIdentity.outputs.principalId
    keyvaultUri: keyvaultUri
    keyvaultId: keyVault.outputs.keyVaultId
    keyvaultName: keyVault.outputs.keyvaultName
    env:env
    cosmosConnectionString: cosmosDb.outputs.cosmosConnectionString
  }
}

// APIM
/*
module apim 'modules/apim.bicep' = {
  name: 'apimDeploy-${env}'
  params: {
    apimName: apimName
    publisherEmail: 'admin@yourdomain.com'
    publisherName: 'YourCompanyName'
    subnetResourceId: vnet.outputs.apimSubnetId
    location: location
  }
}
*/
