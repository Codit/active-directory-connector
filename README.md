# Active Directory connector for Logic Apps

[![Build status](https://ci.appveyor.com/api/projects/status/fmb3y2808ba8hk4p/branch/master?svg=true)](https://ci.appveyor.com/project/tomkerkhove/active-directory-connector-k8ltb/branch/master)
 [![License](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/tomkerkhove/active-directory-connector/blob/master/LICENSE)

API for querying Graph API using Azure Active Directory Application authentication.

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FCoditEU%2Factive-directory-connector%2Fmaster%2Fdeploy%2Fazuredeploy.json%3Ftoken%3DAEJPP6tuY87G8Iqf0-ZK6EWQBXohFTz9ks5ZmYlOwA%253D%253D) 

## Features
The API App allows you to query your AD tenant:

- Get a list of all users
- Get a list of all users by company name
- Get a specific user by user principle name ie. `tom.kerkhove@codit.eu`

Missing something? Feel free to create open an [issue](https://github.com/tomkerkhove/active-directory-connector/issues).

## Installation
All you have to do is host this connector as an **Azure API App** that you can use in your Logic App. 
More information can be found [here](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-custom-hosted-api).

Don't want to go through it yourself? Use the "Deploy To Azure" button!

## Configuration

### Creating an Azure AD Application
In order to use this connector, it is required to use an Azure AD Application that will be used to query Azure AD. Make sure the application is created in the same AD tenant and has the following permissions:

- **Application Permissions**
	- Windows Azure Active Directory API
		- _Read directory data_

### Configuring the API App
The following configuration should be provided in the web.config:

- `ActiveDirectory.Tenant` - Name of your AD tenant ie. `codito.onmicrosoft.com`
- `ActiveDirectory.QueryApplication.ClientId` - Application Id of your Azure AD application
- `ActiveDirectory.QueryApplication.AppKey` - Authentication key for your Azure AD Application
- `Telemetry.ApplicationInsights` - Instrumentation key for Azure Application Inishgts to track exceptions with _(Optional)_

Here is a complete overview of all the settings:

```
<appSettings>
	<!-- Telemetry -->
	<add key="Telemetry.ApplicationInsights" value="#{Telemetry.ApplicationInsights}#" />

	<!-- Authentication -->
	<add key="ActiveDirectory.Tenant" value="#{ActiveDirectory.Tenant}#" />
	<add key="ActiveDirectory.QueryApplication.ClientId" value="#{ActiveDirectory.QueryApplication.ClientId}#" />
	<add key="ActiveDirectory.QueryApplication.AppKey" value="#{ActiveDirectory.QueryApplication.AppKey}#" />
</appSettings>
```

# License Information
This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Codit is the original author of this web application.
