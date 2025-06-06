param acrName string
param location string

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  sku: {
    name: 'Premium'
  }
  properties: {
    adminUserEnabled: false
    publicNetworkAccess: 'Disabled'
  }
}



output acrId string = containerRegistry.id
output acrLoginServer string = containerRegistry.properties.loginServer
