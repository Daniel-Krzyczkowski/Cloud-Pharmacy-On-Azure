param location string = resourceGroup().location
param solutionName string = 'cloud-pharmacy'
param environmentType string = 'dev'
param patientApiContainerImage string = ''
param physicianApiContainerImage string = ''
param pharmacyStoreApiContainerImage string = ''
param verifiableCredentialsApiContainerImage string = ''
param revisionSuffix string
param containerPort int
param containerRegistryName string
param containerRegistryUsername string
@secure()
param containerRegistryPassword string
@secure()
param appInsightsKey string
@secure()
param storageAccountKey string =''
@secure()
param storageAccountConnectionString string = ''
@secure()
param cosmosDbConnectionString string =''
@secure()
param patientApiClientSecret string =''
@secure()
param pharmacyStoreApiClientSecret string =''
@secure()
param verifiableCredentialsApiClientSecret string =''
@secure()
param patientVerifiableCredentialDecentralizedIdentifier string =''


module logAnalyticsWorkspace 'log-analytics-workspace.bicep' = {
    name: 'log-${solutionName}-containers-${environmentType}'
    params: {
      location: location
      environmentType: environmentType
      name: 'log-${solutionName}-containers-${environmentType}'
    }
}

module containerAppEnvironment 'container-app-environment.bicep' = {
  name: 'env-container-app-${solutionName}-${environmentType}'
  params: {
    name: 'env-container-app-${solutionName}-${environmentType}'
    location: location
    environmentType: environmentType
    logAnalyticsWorkspaceClientId:logAnalyticsWorkspace.outputs.clientId
    logAnalyticsWorkspaceClientSecret: logAnalyticsWorkspace.outputs.clientSecret
  }
}

module patientApiContainerApp 'patient-api-container-app.bicep' = {
  name: 'patient-api-${environmentType}'
  params: {
    contianerAppName: 'patient-api-${environmentType}'
    location: location
    environmentType: environmentType
    containerAppEnvironmentId: containerAppEnvironment.outputs.id
    containerImage: patientApiContainerImage
    revisionSuffix: revisionSuffix
    containerPort: containerPort
    appInsightsKey: appInsightsKey
    patientApiClientSecret: patientApiClientSecret
    cosmosDbConnectionString: cosmosDbConnectionString

    envVars: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'ApplicationInsightsConfiguration__InstrumentationKey'
          secretRef: 'app-insights-key'
        }
        {
          name: 'AzureAdB2CConfiguration__ClientSecret'
          secretRef: 'patient-api-client-secret'
        }
        {
          name: 'CosmosDbConfiguration__ConnectionString'
          secretRef: 'cosmos-db-connection-string'
        }
    ]
    useExternalIngress: true
    registry: containerRegistryName
    registryUsername: containerRegistryUsername
    registryPassword: containerRegistryPassword

  }
}

module physicianApiContainerApp 'physician-api-container-app.bicep' = {
  name: 'physician-api-${environmentType}'
  params: {
    contianerAppName: 'physician-api-${environmentType}'
    location: location
    environmentType: environmentType
    containerAppEnvironmentId: containerAppEnvironment.outputs.id
    containerImage: physicianApiContainerImage
    revisionSuffix: revisionSuffix
    containerPort: containerPort
    appInsightsKey: appInsightsKey
    storageAccountKey: storageAccountKey
    storageAccountConnectionString: storageAccountConnectionString
    cosmosDbConnectionString: cosmosDbConnectionString

    envVars: [
        {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
        }
        {
          name: 'ApplicationInsightsConfiguration__InstrumentationKey'
          secretRef: 'app-insights-key'
        }
        {
          name: 'BlobStorageConfiguration__Key'
          secretRef: 'blob-storage-key'
        }
        {
          name: 'BlobStorageConfiguration__ConnectionString'
          secretRef: 'blob-storage-connection-string'
        }
        {
          name: 'CosmosDbConfiguration__ConnectionString'
          secretRef: 'cosmos-db-connection-string'
        }
    ]
    useExternalIngress: true
    registry: containerRegistryName
    registryUsername: containerRegistryUsername
    registryPassword: containerRegistryPassword

  }
}

module pharmacyStoreApiContainerApp 'pharmacy-store-api-container-app.bicep' = {
  name: 'pharmacy-store-api-${environmentType}'
  params: {
    contianerAppName: 'pharmacy-store-api-${environmentType}'
    location: location
    environmentType: environmentType
    containerAppEnvironmentId: containerAppEnvironment.outputs.id
    containerImage: pharmacyStoreApiContainerImage
    revisionSuffix: revisionSuffix
    containerPort: containerPort
    appInsightsKey: appInsightsKey
    pharmacyStoreApiClientSecret: pharmacyStoreApiClientSecret
    cosmosDbConnectionString: cosmosDbConnectionString
    
    envVars: [
        {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
        }
        {
          name: 'ApplicationInsightsConfiguration__InstrumentationKey'
          secretRef: 'app-insights-key'
        }
        {
          name: 'AzureAdB2CConfiguration__ClientSecret'
          secretRef: 'pharmacy-store-api-client-secret'
        }
        {
          name: 'CosmosDbConfiguration__ConnectionString'
          secretRef: 'cosmos-db-connection-string'
        }
    ]
    useExternalIngress: true
    registry: containerRegistryName
    registryUsername: containerRegistryUsername
    registryPassword: containerRegistryPassword

  }
}

module verifiableCredentialsApiContainerApp 'verifiable-credentials-api-container-app.bicep' = {
  name: 'verifiable-credentials-api-${environmentType}'
  params: {
    contianerAppName: 'verifiable-credentials-api-${environmentType}'
    location: location
    environmentType: environmentType
    containerAppEnvironmentId: containerAppEnvironment.outputs.id
    containerImage: verifiableCredentialsApiContainerImage
    revisionSuffix: revisionSuffix
    containerPort: containerPort
    appInsightsKey: appInsightsKey
    verifiableCredentialsApiClientSecret: verifiableCredentialsApiClientSecret
    patientVerifiableCredentialDecentralizedIdentifier: patientVerifiableCredentialDecentralizedIdentifier
    envVars: [
        {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
        }
        {
          name: 'ApplicationInsightsConfiguration__InstrumentationKey'
          secretRef: 'app-insights-key'
        }
        {
          name: 'VerifiableCredentialsConfiguration__ClientSecret'
          secretRef: 'verifiable-credentials-api-client-secret'
        }
        {
          name: 'VerifiableCredentialsConfiguration__IssuerAuthority'
          secretRef: 'patient-verifiable-credential-decentralizer-identifier'
        }
        {
          name: 'VerifiableCredentialsConfiguration__VerifierAuthority'
          secretRef: 'patient-verifiable-credential-decentralizer-identifier'
        }
    ]
    useExternalIngress: true
    registry: containerRegistryName
    registryUsername: containerRegistryUsername
    registryPassword: containerRegistryPassword

  }
}
