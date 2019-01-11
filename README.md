![Logo](docs/Images/Skoruba.IdentityServer4.Admin-Logo-ReadMe.png)

# Skoruba.IdentityServer4.Admin

> The administration of the IdentityServer4 and Asp.Net Core Identity

For project documentation, please visit [readthedocs](https://skoruba-identityserver4-admin.readthedocs.io).

## Project Status

[![Build status](https://ci.appveyor.com/api/projects/status/5yg59bn70399hn6s/branch/master?svg=true)](https://ci.appveyor.com/project/JanSkoruba/identityserver4-admin/branch/master)
[![Build Status](https://dev.azure.com/skoruba/IdentityServer4.Admin/_apis/build/status/IdentityServer4.Admin-CI?branchName=master)](https://dev.azure.com/skoruba/IdentityServer4.Admin/_build/latest?definitionId=2?branchName=master)
[![Join the chat at https://gitter.im/skoruba/IdentityServer4.Admin](https://badges.gitter.im/skoruba/IdentityServer4.Admin.svg)](https://gitter.im/skoruba/IdentityServer4.Admin?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This is currently in **beta version**

The application is written in the **Asp.Net Core MVC - using .NET Core 2.2**

**NOTE:** Works only with **IdentityServer4 version => 2.3.0** 🚀

## Plan & Vision

### 1.0.0:

- [x] Create the Business Logic & EF layers - available as a nuget package
- [x] Create a project template using dotnet CLI - `dotnet new template`
  - [x] First template: The administration of the IdentityServer4 and Asp.Net Core Identity
- [x] Add logging into
  - [x] Database
  - [x] File
- [x] Add localization for other languages
  - [x] English
  - [x] Chinese
  - [x] Russian

### 1.1.0:

- [ ] Add audit logs to track changes ([#61](https://github.com/skoruba/IdentityServer4.Admin/issues/61))
- [ ] Create a project template using dotnet CLI - `dotnet new template`
  - [ ] Second template: The administration of the IdentityServer4 (without Asp.Net Core Identity) ([#79](https://github.com/skoruba/IdentityServer4.Admin/issues/79))
- [ ] User registration / Password reset
- [ ] Account linking
- [ ] Manage profile

### 2.0.0:

- [ ] Add API:
  - [ ] IdentityServer4
  - [ ] Asp.Net Core Identity
  - [ ] Add swagger support

### Future:

- Add UI tests
- Add more unit and integration tests :blush:
- Extend administration for another protocols
- Create separate UI using `Razor Class Library`

## Licence

This repository is licensed under the terms of the [**MIT license**](LICENSE.md).

**NOTE**: This repository uses the source code from https://github.com/IdentityServer/IdentityServer4.Quickstart.UI which is under the terms of the 
[**Apache License 2.0**](https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/blob/master/LICENSE).

## Acknowledgements

This web application is based on these projects:

- ASP.NET Core
- IdentityServer4.EntityFramework
- ASP.NET Core Identity
- XUnit
- Fluent Assertions
- Bogus
- AutoMapper
- Serilog

Thanks to [Tomáš Hübelbauer](https://github.com/TomasHubelbauer) for the initial code review.

Thanks to [Dominick Baier](https://github.com/leastprivilege) and [Brock Allen](https://github.com/brockallen) - the creators of IdentityServer4.

## Contributors

Thanks goes to these wonderful people ([emoji key](https://github.com/kentcdodds/all-contributors#emoji-key)):

<!-- prettier-ignore-start -->
| [<img src="https://avatars3.githubusercontent.com/u/35664089?s=460&v=3" width="100px;"/><br /><sub> Jan Škoruba</sub>](https://github.com/skoruba) <br /> 💻 💬 📖 💡 🤔 | [<img src="https://avatars0.githubusercontent.com/u/6831144?s=460&v=3" width="100px;"/><br /><sub> Tomáš Hübelbauer</sub>](https://github.com/TomasHubelbauer) <br /> 💻 👀 📖  🤔 | [<img src="https://avatars0.githubusercontent.com/u/1004852?s=460&v=3" width="100px;"/><br /><sub>Michał Drzał </sub>](https://github.com/xmichaelx) <br />💻 👀 📖 💡 🤔 | [<img src="https://avatars0.githubusercontent.com/u/2261603?s=460&v=3" width="100px;"/><br /><sub>cerginio </sub>](https://github.com/cerginio) <br /> 💻 🐛 💡 🤔 | [<img src="https://avatars3.githubusercontent.com/u/13407080?s=460&v=3" width="100px;"/><br /><sub>Sven Dummis </sub>](https://github.com/svendu) <br /> 📖| [<img src="https://avatars1.githubusercontent.com/u/1687087?s=460&v=3" width="100px;"/><br /><sub>Seaear</sub>](https://github.com/Seaear) <br />💻 🌍|
| :---: | :---: | :---: | :---: | :---: | :---: |
|[<img src="https://avatars1.githubusercontent.com/u/1150473?s=460&v=3" width="118px;"/><br /><sub>Rune Antonsen </sub>](https://github.com/ruant) <br />🐛|[<img src="https://avatars1.githubusercontent.com/u/5537607?s=460&v=3" width="118px;"/><br /><sub>Sindre Njøsen </sub>](https://github.com/Sindrenj) <br />💻|[<img src="https://avatars1.githubusercontent.com/u/40323674?s=460&v=3" width="118px;"/><br /><sub>Alevtina Brown </sub>](https://github.com/alev7ina) <br />🌍|
<!-- prettier-ignore-end -->

This project follows the [all-contributors](https://github.com/kentcdodds/all-contributors) specification.
Contributions of any kind are welcome!

## Contact and Suggestion

I am happy to share my attempt of the implementation of the administration for IdentityServer4 and ASP.NET Core Identity.

Any feedback is welcome - feel free to create an issue or send me an email - [jan@skoruba.com](mailto:jan@skoruba.com). Thank you :blush:
