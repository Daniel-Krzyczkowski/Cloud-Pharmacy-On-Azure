param location string
param name string
param environmentType string = 'dev'

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: name
  tags:{
    'environment':environmentType
  }
  location: location
  properties: any({
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}
output clientId string = logAnalyticsWorkspace.properties.customerId
output clientSecret string = logAnalyticsWorkspace.listKeys().primarySharedKey
