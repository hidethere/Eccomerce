param redisName string 
param location string

resource redis 'Microsoft.Cache/Redis@2023-04-01' = {
  name: redisName
  location: location
  properties: {
  sku: {
    name: 'Basic'
    family: 'C'
    capacity: 1
  }
  enableNonSslPort: false
  publicNetworkAccess: 'Disabled' 
  }
}

output redisId string = redis.id
