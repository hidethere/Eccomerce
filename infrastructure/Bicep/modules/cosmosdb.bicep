param cosmosAccountName string 
param cosmosDatabases array = [
  'orderdb'
  'inventorydb'
  'productdb'
  'categorydb'
]
param location string

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: cosmosAccountName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    publicNetworkAccess: 'Disabled'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
  }
}

resource cosmosSqlDatabases 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = [for dbName in cosmosDatabases: {
  name: dbName
  parent: cosmosAccount
  properties: {
    resource: {
      id: dbName
    }
    options: {}
  }

}]

output cosmosAccountId string = cosmosAccount.id
