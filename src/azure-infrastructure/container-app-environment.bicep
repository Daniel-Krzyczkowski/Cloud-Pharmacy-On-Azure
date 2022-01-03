param name string
param location string
param logAnalyticsWorkspaceClientId string
param logAnalyticsWorkspaceClientSecret string
param environmentType string = 'dev'

resource containerAppEnvironment 'Microsoft.Web/kubeEnvironments@2021-02-01' = {
  name: name
  tags:{
    'environment':environmentType
  }
  location: location
  properties: {
    type: 'managed'
    internalLoadBalancerEnabled: false
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspaceClientId
        sharedKey: logAnalyticsWorkspaceClientSecret
      }
    }
  }
}
output id string = containerAppEnvironment.id
