Installation
============


Requirements
------------

- [Install](https://www.microsoft.com/net/download/windows#/current) the latest .NET Core 2.x SDK (using older versions may lead to 502.5 errors when hosted on IIS or application exiting immediately after starting when self-hosted)

Installation methods
--------------------

**Cloning**

    git clone https://github.com/skoruba/IdentityServer4.Admin

**Installation via dotnet new template**

- Install the dotnet new template:

    dotnet new -i Skoruba.IdentityServer4.Admin.Templates::1.0.0-beta5-update1

- Create new project:

    dotnet new skoruba.is4admin --name MyProject --title MyProject --adminrole MyRole --adminclientid MyClientId


Project template options:

    --name: [string value] for project name
    --title: [string value] for title and footer of the administration in UI
    --adminrole: [string value] for name of admin role, that is used to authorize the administration
    --adminclientid: [string value] for client name, that is used in the IdentityServer4 configuration



Installation of the Client Libraries
------------------------------------

    cd src/Skoruba.IdentityServer4.Admin
    npm install

    cd src/Skoruba.IdentityServer4.STS.Identity
    npm install


Running in Visual Studio
------------------------

- Set Startup projects:
  - Skoruba.IdentityServer4.Admin
  - Skoruba.IdentityServer4.STS.Identity