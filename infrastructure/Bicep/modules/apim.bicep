param apimName string
param location string
param publisherEmail string
param publisherName string
param subnetResourceId string  // Subnet ID for VNET integration

resource apim 'Microsoft.ApiManagement/service@2024-05-01' = {
  name: apimName
  location: location
  sku: {
    name: 'Premium' 
    capacity: 1
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
    
    virtualNetworkConfiguration: {
      subnetResourceId: subnetResourceId
    }
    virtualNetworkType: 'External'
  }
  tags: {}
}

output apimId string = apim.id
