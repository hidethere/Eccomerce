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
param keyvaultId string
param keyVaultName string

resource acaEnv 'Microsoft.App/managedEnvironments@2023-05-01' existing = {
  name: environmentName
}

resource acr 'Microsoft.ContainerRegistry/registries@2023-01-01' existing = {
  name: acrName
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
        targetPort: 80
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
          env: [
            {
              name: 'KEYVAULT_URI'
              value: keyvaultUri
            }
          ]
        }
      ]
      revisionSuffix: revisionSuffix
    }
  }
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

// Key Vault Role Assignment
resource kvSecretsRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyvaultId, 'KeyVaultSecretsUser', userIdentityId)
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6') // Key Vault Secrets User
    principalId: userIdentityPrincipalId
    principalType: 'ServicePrincipal'
  }
}


// Policies


resource kvAccessPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2023-02-01' = {
  name: '${keyVaultName}/add'
  properties: {
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: userIdentityPrincipalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
  }
}
