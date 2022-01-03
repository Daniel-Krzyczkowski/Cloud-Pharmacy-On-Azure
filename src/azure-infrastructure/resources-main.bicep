param location string = resourceGroup().location
param solutionName string = 'cloud-pharmacy'
param environmentType string = 'dev'
param tenantId string = ''

resource cosmosDatabase 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: 'cosmos-${solutionName}-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  kind: 'GlobalDocumentDB'
  properties:{
    publicNetworkAccess: 'Enabled'
    enableAutomaticFailover: false
    enableMultipleWriteLocations: false
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
      maxIntervalInSeconds: 5
      maxStalenessPrefix: 100
   }
   locations: [
      {
          locationName: 'North Europe'
          failoverPriority: 0
          isZoneRedundant: false
      }
    ]
  }
}

resource signalrService 'Microsoft.SignalRService/signalR@2021-04-01-preview' ={
  name: 'signalr-${solutionName}-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  sku:{
    name: 'Free_F1'
    tier: 'Free'
    capacity: 1
  }
  kind: 'SignalR'
  properties:{
      features: [
        {
          flag: 'ServiceMode'
          value: 'Serverless'
        }
      ]
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: 'stcloudpharmacy${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  kind: 'StorageV2'
  sku: {
    name: 'Standard_ZRS'
  }
  properties: {
    accessTier: 'Hot'
    supportsHttpsTrafficOnly: true
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: 'kv-${solutionName}-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  properties:{
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: tenantId
    accessPolicies:[]
  }
}
