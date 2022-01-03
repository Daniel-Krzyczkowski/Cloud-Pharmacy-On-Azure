param location string
param environmentType string = 'dev'
param containerAppEnvironmentId string
param contianerAppName string

// Container Image ref
param containerImage string
param revisionSuffix string

// Networking
param useExternalIngress bool = true
param containerPort int

param registry string
param registryUsername string
@secure()
param registryPassword string
@secure()
param appInsightsKey string
@secure()
param verifiableCredentialsApiClientSecret string
@secure()
param patientVerifiableCredentialDecentralizedIdentifier string

param envVars array = []

resource containerApp 'Microsoft.Web/containerApps@2021-03-01' = {
  name: contianerAppName
  tags:{
    'environment':environmentType
  }
  kind: 'containerapp'
  location: location
  properties: {
    kubeEnvironmentId: containerAppEnvironmentId
    configuration: {
      activeRevisionsMode: 'single'
      secrets: [
        {
          name: 'acr-password'
          value: registryPassword
        }
        {
          name: 'app-insights-key'
          value: appInsightsKey
        }
        {
          name: 'verifiable-credentials-api-client-secret'
          value: verifiableCredentialsApiClientSecret
        }
        {
          name: 'patient-verifiable-credential-decentralizer-identifier'
          value: patientVerifiableCredentialDecentralizedIdentifier
        }
      ]      
      registries: [
        {
          server: registry
          username: registryUsername
          passwordSecretRef: 'acr-password'
        }
      ]
      ingress: {
        external: useExternalIngress
        targetPort: containerPort
        transport: 'auto'
      }
    }
    template: {
      revisionSuffix: revisionSuffix
      containers: [
        {
          image: containerImage
          name: contianerAppName
          env: envVars
        }
      ]
      scale: {
        minReplicas: 1
      }
    }
  }
}
