param env string 
param apimSubnetName string = 'apimSubnet-${env}'
param containerAppsSubnetName string =  'containerAppsSubnet-${env}'
param vmSubnetName string = 'vmSubnet-${env}'
param vnetName string = 'vnet-Eccomerce-${env}'
param location string = resourceGroup().location

param sqlServerName string = 'sqlServer-Eccomerce-${env}-v1'
param cosmosAccountName string = 'cosmosacc-eccomerce-${env}-v1'
param redisName string = 'redisEccomerce${env}v1'
param sbName string = 'sb-Eccomerce-${env}-v1'
param eventHubName string = 'eventHub-Eccommerce-${env}-v1'
param keyVaultName string = 'keyVault-Eccomerce-${env}v1'
param apimName string = 'apim-Eccomerce-${env}v1'
param acrName string // injected from the pipeline


// Acr
param image string // Injected from the pipeline
param BuildId string // Injected from the pipeline
param revisionSuffix string = 'rev-${BuildId}'

// ACA
param containerAppsEnvName string = 'containerAppsEnv-Eccomerce-${env}'
param containerAppsName string = 'containerapps-eccomerce-${env}'

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

module acaUserIdentity 'modules/uami.bicep' = {
  name: 'uamiDeploy'
  params: {
    name: 'aca-uami-${env}'
    location: location
  }
}


module nicVm 'modules/nic.bicep' = {
  name: 'nicVmDeploy-${env}'
  params: {
    vnetName: vnetName
    subnetName: vmSubnetName
    location: location
  }
  dependsOn: [
    vnet
  ]
}


// Linux vm for CICD Env
module vm 'modules/cicdvm.bicep' = {
  name: 'vmLinuxDeploy-${env}'
  params: {
    adminPublicKey: 'ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQDxfi0PUp5iHtRDURTk9IcVGaUEJ/L8QEWSvhet+1Ygvc6+cJmUKnjeViJcJQ+Qyh0mpaGJTM0DmQSpCJi0pmC9VI7NDC051ErsgAq9t0MaWVt0H6yxLwJqzEt38mn/ob2P8YAHWE5cXlDVT1RbZ5fz369db43uZ8a13YG17n3mPGQpuq5I9IcRAu960yxN1U2SCPuJTdJfJlqSfbY0p8+T7r3MNQlNHxCwU/FUgK6VV684w+WZ/oQqfcwm3cQqRdyOTUUbfFW5w/SqKJR2dR9COBIcir+9WdLYMCZwlxy81HSul8E64oDsCoZqbrlXF9AtzTaALV+CVYP/DlzeTM/1YBg1MdcpTKJK9Qvq4vuKohZSNM9dXAG3BsJCvN/diCSb0FTg9OeZ2jjSEdTm29LoKnuDymyJNoTgJ7sU1Ai8zv9GpFXWsk1TP1UPQlAChf0MesqxVDdsXcfjUR6FMgpO6875qIWpyNVD3NNVfmiqStwPufcsg5KI1ZgUIMdMg4GE1nQZpyJhmzvw6zabqYQS56RSr5W+LRYcyFhsyAm1F9BL4dvswzBmVx6ziJnlGy5AZYDovsYxevBibX8//2haDR/5N4U9vpjvxG7Cbd7xsURVx31B0RzCIkh1Kq28K2NqlEUFJCr+PU9uK0Zej0gYuwLO/7VNIeQvG1Gk3FNrPQ== diogo@gmail.com'
    location: location
    nicId: nicVm.outputs.nicId
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
  name: 'keyVaultDeploy-${env}1'
  params: {
    keyVaultName: keyVaultName
    location: location
  }
}


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

