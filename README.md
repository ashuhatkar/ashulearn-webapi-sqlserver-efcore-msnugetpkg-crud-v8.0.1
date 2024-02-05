﻿The Entity Framework Core tools help with design-time development tasks. They're primarily used to manage Migrations and to scaffold a DbContext and entity types by reverse engineering the schema of a database.
Microsoft.EntityFrameworkCore.Design is for cross-platform command line tooling.

## Setup development environment

- Clone the repository: https://github.com/ashuhatkar/ashulearn-webapi-sqlserver-efcore-msnugetpkg-crud-v8.0.1
- (Windows only) Install Visual Studio. Visual Studio contains tooling support for .NET Aspire that you will want to have. Visual Studio 2022 version 17.9 Preview.
- During installation, ensure that the following are selected:
- ASP.NET and web development workload
- .NET Aspire SDK component in Individual components.
- Install the latest .NET 8 SDK
- On Mac/Linux (or if not using Visual Studio), install the Aspire workload with the following commands:
- dotnet workload update
- dotnet workload install aspire
- dotnet restore <project>.sln
- Configure Docker desktop is free to use as a part of the Docker Personal subscription for individuals non-commercial open-source projects.
- Run SQL Server 2022 container image with Docker
- Configure local kubernetes
- Cloud infrastructure subscription

## Getting started

`Microsoft.EntityFrameworkCore.Design` contains all the design-time logic for Entity Framework Core. It's the code that all of the various tools (PMC cmdlets like `Add-Migration`, `dotnet ef` & `ef.exe`) call into.

If you don't use Migrations or Reverse Engineering, you don't need it.

And when you do need it, we encourage `PrivateAssets="All" `so it doesn't get published to the server where you almost certainly won't need it.

### Prerequisites

Before using the tools:

- [Understand the difference between target and startup project](https://learn.microsoft.com/en-us/ef/core/cli/powershell#target-and-startup-project).
- [Learn how to use the tools with .NET Standard class libraries](https://learn.microsoft.com/en-us/ef/core/cli/powershell#other-target-frameworks).
- [For ASP.NET Core projects, set the environment](https://learn.microsoft.com/en-us/ef/core/cli/powershell#aspnet-core-environment).

## Usage

PMC Command | Usage
-- | --
Get-Help entityframework |Displays information about entity framework commands.
[Add-Migration](https://learn.microsoft.com/en-us/ef/core/cli/powershell#add-migration)  | Creates a migration by adding a migration snapshot.
[Bundle-Migration](https://learn.microsoft.com/en-us/ef/core/cli/powershell#bundle-migration) | Creates an executable to update the database.
[Get-DbContext](https://learn.microsoft.com/en-us/ef/core/cli/powershell#get-dbcontext) | Gets information about a DbContext type.
[Drop-Database](https://learn.microsoft.com/en-us/ef/core/cli/powershell#drop-database) | Drops the database.
[Get-Migration](https://learn.microsoft.com/en-us/ef/core/cli/powershell#get-migration) | Lists available migrations.
[Optimize-DbContext](https://learn.microsoft.com/en-us/ef/core/cli/powershell#optimize-dbcontext) | Generates a compiled version of the model used by the `DbContext`.
[Remove-Migration](https://learn.microsoft.com/en-us/ef/core/cli/powershell#remove-migration) | Removes the last migration snapshot.
[Scaffold-DbContext](https://learn.microsoft.com/en-us/ef/core/cli/powershell#scaffold-dbcontext) | Generates a DbContext and entity type classes for a specified database. This is called reverse engineering.
[Script-DbContext](https://learn.microsoft.com/en-us/ef/core/cli/powershell#script-dbcontext) | Generates a SQL script from the DbContext. Bypasses any migrations.
[Script-Migration](https://learn.microsoft.com/en-us/ef/core/cli/powershell#script-migration) |  Generates a SQL script using all the migration snapshots.
[Update-Database](https://learn.microsoft.com/en-us/ef/core/cli/powershell#update-database) | Updates the database schema based on the last migration snapshot.

## Additional documentation

- [Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Reverse Engineering](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli)
- [Compiled models](https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-di%2Cwith-constant#compiled-models)

## Feedback

If you encounter a bug or issues with this package,you can [open an Github issue](https://github.com/dotnet/efcore/issues/new/choose). For more details, see [getting support](https://github.com/dotnet/efcore/blob/main/.github/SUPPORT.md).