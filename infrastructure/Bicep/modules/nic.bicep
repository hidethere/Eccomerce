param vnetName string
param subnetName string
param location string


resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' existing = {
  name: vnetName
}

// Reference existing Subnet
resource subnet 'Microsoft.Network/virtualNetworks/subnets@2021-05-01' existing = {
  name: subnetName
  parent: vnet
}


// NIC for VM
resource nic 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: '${subnetName}-nic'
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          subnet: {
            id: subnet.id
          }
          privateIPAllocationMethod: 'Dynamic'
        }
      }
    ]

  }
}


output nicId string = nic.id
