param location string = resourceGroup().location
param solutionName string = 'cloud-pharmacy'
param environmentType string = 'dev'


resource applicationInsights 'Microsoft.Insights/components@2015-05-01' = {
  name: 'appi-${solutionName}-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Bluefield'
    Request_Source: 'rest'
    IngestionMode: 'ApplicationInsights'
  }
}

resource functionAppsStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: 'stfuncappcldphrm${environmentType}'
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

resource functionAppsHostingPlan 'Microsoft.Web/serverfarms@2020-10-01' = {
  name: 'plan-func-apps-${solutionName}-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  sku: {
    name: 'Y1' 
    tier: 'Dynamic'
  }
}

resource notificationsfunctionapp 'Microsoft.Web/sites@2020-06-01' = {
  name: 'func-${solutionName}-notifications-${environmentType}'
  location: location
  kind: 'functionapp'
  tags:{
    'environment':environmentType
  }
  properties: {
    httpsOnly: true
    serverFarmId: functionAppsHostingPlan.id
    clientAffinityEnabled: true
    siteConfig: {
      appSettings: [
        {
          'name': 'APPINSIGHTS_INSTRUMENTATIONKEY'
          'value': applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionAppsStorageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(functionAppsStorageAccount.id, functionAppsStorageAccount.apiVersion).keys[0].value}'
        }
        {
          'name': 'FUNCTIONS_EXTENSION_VERSION'
          'value': '~4'
        }
        {
          'name': 'FUNCTIONS_WORKER_RUNTIME'
          'value': 'dotnet'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionAppsStorageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(functionAppsStorageAccount.id, functionAppsStorageAccount.apiVersion).keys[0].value}'
        }
      ]
    }
  }
}

resource patientWebAppAppServicePlan 'Microsoft.Web/serverfarms@2021-01-15' = {
  name: 'plan-${solutionName}-patient-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  sku: {
    name: 'B1'
    capacity: 1
  }
}

resource patientWebApp 'Microsoft.Web/sites@2021-01-15' = {
  name: 'app-${solutionName}-patient-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  properties: {
    serverFarmId: patientWebAppAppServicePlan.id
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource physicianWebAppAppServicePlan 'Microsoft.Web/serverfarms@2021-01-15' = {
  name: 'plan-${solutionName}-physician-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  sku: {
    name: 'B1'
    capacity: 1
  }
}

resource physicianWebApp 'Microsoft.Web/sites@2021-01-15' = {
  name: 'app-${solutionName}-physician-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  properties: {
    serverFarmId: physicianWebAppAppServicePlan.id
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource pharmacyStoreWebAppAppServicePlan 'Microsoft.Web/serverfarms@2021-01-15' = {
  name: 'plan-${solutionName}-store-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  sku: {
    name: 'B1'
    capacity: 1
  }
}

resource pharmacyStoreWebAppApp 'Microsoft.Web/sites@2021-01-15' = {
  name: 'app-${solutionName}-store-${environmentType}'
  location: location
  tags:{
    'environment':environmentType
  }
  properties: {
    serverFarmId: pharmacyStoreWebAppAppServicePlan.id
  }
  identity: {
    type: 'SystemAssigned'
  }
}
