# Active Directory connector for Logic Apps
API for querying Graph API using Azure Active Directory Application authentication.

![Build Status](https://codit.visualstudio.com/_apis/public/build/definitions/fd3bf22a-f76c-448b-ad13-f5e97dd3a942/294/badge)

## Features
The API App allows you to query your AD tenant:

- Get a list of all users
- Get a specific user by user principle name ie. `tom.kerkhove@codit.eu`

Missing something? Feel free to create open an [issue](https://github.com/tomkerkhove/active-directory-connector/issues).

## Configuration

TBW

```
<appSettings>
	<add key="Telemetry.ApplicationInsights" value="#{Telemetry.ApplicationInsights}#" />
	<add key="ActiveDirectory.Tenant" value="#{ActiveDirectory.Tenant}#" />
	<add key="ActiveDirectory.QueryApplication.ClientId" value="#{ActiveDirectory.QueryApplication.ClientId}#" />
	<add key="ActiveDirectory.QueryApplication.AppKey" value="#{ActiveDirectory.QueryApplication.AppKey}#" />
</appSettings>
```