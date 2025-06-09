param containerAppName string
param location string
param environmentName string
param acrLoginServer string
param image string
param revisionSuffix string
param userIdentityId string
param userIdentityPrincipalId string
param acrName string
param keyvaultUri string
param env string
param cosmosConnectionString string
param cosmosAccountName string

resource acaEnv 'Microsoft.App/managedEnvironments@2023-05-01' existing = {
  name: environmentName
}

resource acr 'Microsoft.ContainerRegistry/registries@2023-01-01' existing = {
  name: acrName
}

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' existing = {
  name: cosmosAccountName
}

// Role

// AcrPull Role Assignment 
resource acrPullRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(userIdentityId, 'acrpull')
  scope: acr
  properties: {
    principalId: userIdentityPrincipalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d') // AcrPull
    principalType: 'ServicePrincipal'
  }
}


resource containerApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: containerAppName
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userIdentityId}': {}
    }
  }
  properties: {
    managedEnvironmentId: acaEnv.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
      }
      registries: [
        {
          server: acrLoginServer
          identity: userIdentityId
        }
      ]
    }
    template: {
      containers: [
        {
          name: containerAppName
          image: image
          resources: {
            cpu: '0.5'
            memory: '1.0Gi'
          }
          env: [
            {
              name: 'KEYVAULT_URI'
              value: keyvaultUri
            }
            {
              name: 'ENVIRONMENT_NAME'
              value: env
            }
            {
              name: 'COSMOS_DB_URI' 
              value: cosmosConnectionString
            }
            {
              name: 'COSMOS_PRODUCTDB_NAME' 
              value: 'productdb'
            }
            {
              name: 'COSMOS_CATEGORYDB_NAME'
              value: 'categorydb'
            } 
  
          ]
        }
      ]
      revisionSuffix: revisionSuffix
    }
  }
}


// Cosmos Role Assignment
resource cosmosDataContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(userIdentityPrincipalId, cosmosAccount.id, 'CosmosDataContributor')
  scope: cosmosAccount
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '5bd9cd88-fe45-4216-938b-f97437e15450')
    principalId: userIdentityPrincipalId
    principalType: 'ServicePrincipal'
  }
}


