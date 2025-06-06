param sbNameSpace string 
param location string

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2024-01-01' = {
  name: sbNameSpace
  location: location
  sku: {
    name: 'Premium'
    tier: 'Premium'
    capacity: 1 
  }
  properties: {
    publicNetworkAccess: 'Disabled'
  }
}

output serviceBusNamespaceId string = serviceBusNamespace.id
