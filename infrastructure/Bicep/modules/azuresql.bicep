param location string 
param sqlServerName string
param sqlDatabases array = [
  'userdb'
  'paymentdb'
]
param AdminLogin string
@secure()
param adminPassword string

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: AdminLogin
    administratorLoginPassword: adminPassword
    version: '12.0'
    publicNetworkAccess: 'Disabled'
  }
}


resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = [for dbName in sqlDatabases: {
  name: dbName
  parent: sqlServer
  location: location
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 2147483648
  }
  sku: {
    name: 'GP_Gen5_2'
    tier: 'GeneralPurpose'
    capacity: 2
  }

}]



output sqlServerId string = sqlServer.id

