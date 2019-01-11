
## Overview

### Solution structure:

- STS:

  - `Skoruba.IdentityServer4.STS.Identity` - [Quickstart UI for the IdentityServer4 with Asp.Net Core Identity and EF Core storage](https://github.com/IdentityServer/IdentityServer4.Samples/tree/master/Quickstarts/Combined_AspId_and_EFStorage)

- Admin UI:

  - `Skoruba.IdentityServer4.Admin` - ASP.NET Core MVC application that contains Admin UI

  - `Skoruba.IdentityServer4.Admin.BusinessLogic` - project that contains Dtos, Repositories, Services and Mappers for the IdentityServer4

  - `Skoruba.IdentityServer4.Admin.BusinessLogic.Identity` - project that contains Dtos, Repositories, Services and Mappers for the Asp.Net Core Identity

  - `Skoruba.IdentityServer4.Admin.BusinessLogic.Shared` - project that contains shared Dtos and ExceptionHandling for the Business Logic layer of the IdentityServer4 and Asp.Net Core Identity

  - `Skoruba.IdentityServer4.Admin.EntityFramework` - EF Core data layer that contains Entities for the IdentityServer4

  - `Skoruba.IdentityServer4.Admin.EntityFramework.Identity` - EF Core data layer that contains Entities for the Asp.Net Core Identity

  - `Skoruba.IdentityServer4.Admin.EntityFramework.DbContexts` - project that contains AdminDbContext for the administration

- Tests:

  - `Skoruba.IdentityServer4.Admin.IntegrationTests` - xUnit project that contains the integration tests

  - `Skoruba.IdentityServer4.Admin.UnitTests` - xUnit project that contains the unit tests

### The admininistration contains the following sections:

![Skoruba.IdentityServer4.Admin App](docs/Images/Skoruba.IdentityServer4.Admin-Solution.png)

## IdentityServer4

**Clients**

It is possible to define the configuration according the client type - by default the client types are used:

- Empty
- Web Application - Server side - Implicit flow
- Web Application - Server side - Hybrid flow
- Single Page Application - Javascript - Implicit flow
- Native Application - Mobile/Desktop - Hybrid flow
- Machine/Robot - Resource Owner Password and Client Credentials flow
- TV and Limited-Input Device Application - Device flow

- Actions: Add, Update, Clone, Remove
- Entities:
  - Client Cors Origins
  - Client Grant Types
  - Client IdP Restrictions
  - Client Post Logout Redirect Uris
  - Client Properties
  - Client Redirect Uris
  - Client Scopes
  - Client Secrets

**API Resources**

- Actions: Add, Update, Remove
- Entities:
  - Api Claims
  - Api Scopes
  - Api Scope Claims
  - Api Secrets

**Identity Resources**

- Actions: Add, Update, Remove
- Entities:
  - Identity Claims

## Asp.Net Core Identity

**Users**

- Actions: Add, Update, Delete
- Entities:
  - User Roles
  - User Logins
  - User Claims

**Roles**

- Actions: Add, Update, Delete
- Entities:
  - Role Claims

## Application Diagram

![Skoruba.IdentityServer4.Admin Diagram](docs/Images/Skoruba.IdentityServer4.Admin-App-Diagram.png)


## Template uses following list of nuget packages

- [Available nuget packages](https://www.nuget.org/profiles/skoruba)

## Authentication and Authorization

- Change the specific URLs and names for the IdentityServer and Authentication settings in `Constants/AuthenticationConsts` or `appsettings.json`
- `Constants/AuthorizationConsts.cs` contains configuration of constants connected with authorization - definition of the default name of admin policy
- In the controllers is used the policy which name is stored in - `AuthorizationConsts.AdministrationPolicy`. In the policy - `AuthorizationConsts.AdministrationPolicy` is defined required role stored in - `AuthorizationConsts.AdministrationRole`.
- With the default configuration, it is necessary to configure and run instance of IdentityServer4. It is possible to use initial migration for creating the client as it mentioned above

## Localizations - labels, messages

- All labels and messages are stored in the resources `.resx` - locatated in `/Resources`

  - Client label descriptions from - http://docs.identityserver.io/en/release/reference/client.html
  - Api Resource label descriptions from - http://docs.identityserver.io/en/release/reference/api_resource.html
  - Identity Resource label descriptions from - http://docs.identityserver.io/en/release/reference/identity_resource.html

## Tests

- The solution contains unit and integration tests.
- **Stage environment is used for integration tests**:

  - `DbContext` contains setup for InMemory database
  - `Authentication` is setup for `CookieAuthentication` - with fake login url only for testing purpose
  - `AuthenticatedTestRequestMiddleware` - middleware for testing of authentication.

- If you want to use `Stage environment` for deploying - it is necessary to change these settings in `StartupHelpers.cs`.
