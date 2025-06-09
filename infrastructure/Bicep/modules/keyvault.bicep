param keyVaultName string
param location string 
param tenantId string = subscription().tenantId

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: tenantId
    publicNetworkAccess: 'Enabled'
    sku: {
      family: 'A'
      name: 'standard' // or 'premium'
    }
    accessPolicies: [] // Empty for now or configure access policies here
    enableRbacAuthorization: true
    enabledForDeployment: true
    enabledForDiskEncryption: true
    enabledForTemplateDeployment: true
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
    }
  }
}



output keyVaultId string = keyVault.id
output keyvaultName string = keyVault.name
