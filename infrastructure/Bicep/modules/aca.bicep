param containerAppsEnvName string
param location string 
param vnetId string
param subnetName string

resource containerAppsEnv 'Microsoft.App/managedEnvironments@2025-01-01' = {
  name: containerAppsEnvName
  location: location
  properties: {
    
    vnetConfiguration: {
      infrastructureSubnetId: '${vnetId}/subnets/${subnetName}'
    }
    /*appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: '<log-analytics-customer-id>'
        sharedKey: '<log-analytics-shared-key>'
      }
    }*/
  }
}

// app logs costumer and shared key should be replaced with actual values

output containerAppsEnvId string = containerAppsEnv.id
