param vnetName string
param containerAppsSubnetName string 
param apimSubnetName string
param vmSubnetName string
param location string
param addressPrefix string = '10.1.0.0/16'

param subnetContainerAppsPrefix string = '10.1.10.0/23'
param subnetApimPrefix string = '10.1.5.0/24'
param subnetVmPrefix string = '10.1.6.0/24'


resource vnet 'Microsoft.Network/virtualNetworks@2024-05-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [addressPrefix]
    }
  }
}


resource containerApps 'Microsoft.Network/virtualNetworks/subnets@2024-05-01' = {
  name: containerAppsSubnetName
  parent: vnet
  properties: {
    addressPrefix: subnetContainerAppsPrefix

  }
}


resource apimSubnet 'Microsoft.Network/virtualNetworks/subnets@2024-05-01' = {
  name: apimSubnetName
  parent: vnet
  properties: {
    addressPrefix: subnetApimPrefix
    privateEndpointNetworkPolicies: 'Disabled'
    networkSecurityGroup: {
      id: apimNSG.id
    }
    
  }
}

resource vmSubnet 'Microsoft.Network/virtualNetworks/subnets@2024-05-01' = {
  name: vmSubnetName
  parent: vnet
  properties: {
    addressPrefix: subnetVmPrefix
    privateEndpointNetworkPolicies: 'Disabled'

  }
}




resource apimNSG 'Microsoft.Network/networkSecurityGroups@2022-07-01' = {
  name: 'nsg-apim-dev'
  location: location
  properties: {}
}

output vnetId string = vnet.id
output vnetName string = vnet.name
output apimSubnetId string = apimSubnet.id
