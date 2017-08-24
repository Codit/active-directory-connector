# Active Directory connector for Logic Apps

![Build Status](https://codit.visualstudio.com/_apis/public/build/definitions/fd3bf22a-f76c-448b-ad13-f5e97dd3a942/294/badge)[![License](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/tomkerkhove/active-directory-connector/blob/master/LICENSE)

API for querying Graph API using Azure Active Directory Application authentication.

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Ftomkerkhove%2Factive-directory-connector%2Ffeature-slingshot-support%2Fazuredeploy.json%3Ftoken%3DAEJPP6tuY87G8Iqf0-ZK6EWQBXohFTz9ks5ZmYlOwA%253D%253D) 

## Features
The API App allows you to query your AD tenant:

- Get a list of all users
- Get a list of all users by company name
- Get a specific user by user principle name ie. `tom.kerkhove@codit.eu`

Missing something? Feel free to create open an [issue](https://github.com/tomkerkhove/active-directory-connector/issues).

## Installation
All you have to do is host this connector as an **Azure API App** that you can use in your Logic App. 
More information can be found [here](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-custom-hosted-api).

Don't want to go through it yourself? Sit back and wait, we're working on it ([#20](https://github.com/tomkerkhove/active-directory-connector/issues/20))!

## Configuration
In order to use this connector a new Azure AD Application needs to be created in the AD tenant that will be queried. Once created, it needs the following permissions:

- **Application Permissions**
	- Windows Azure Active Directory API
		- _Read directory data_
	
Once everything is setup, configuring the connector itself is straight forward :

- `ActiveDirectory.Tenant` with your AD tenant name, ie. `codito.onmicrosoft.com`
- `ActiveDirectory.QueryApplication.ClientId` with the Application Id of your Azure AD Application
- `ActiveDirectory.QueryApplication.AppKey` with a key that is added to your Azure AD Application
- If you want to track exceptions to Azure Application Insights, make sure to configure `Telemetry.ApplicationInsights` with the instrumentation key of your instance

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