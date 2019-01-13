Migration and seeding
=====================

EF Core & Data Access
---------------------

- Run entity framework migrations - for instance from Visual Studio command line (Nuget package manager):

::
    Add-Migration DbInit -context AdminDbContext -output Data/Migrations
    Update-Database -context AdminDbContext


- Or via ``dotnet CLI``:

::
    dotnet ef migrations add DbInit -c AdminDbContext -o Data/Migrations
    dotnet ef database update -c AdminDbContext


Migrations are not a part of the repository - they are ignored in ``.gitignore``.

We suggest to use seed data:

- In ``Program.cs`` -> ``Main``, uncomment ``DbMigrationHelpers.EnsureSeedData(host)`` or use dotnet CLI ``dotnet run /seed``
- The ``Clients`` and ``Resources`` files in ``Configuration/IdentityServer`` are the initial data, based on a sample from IdentityServer4
- The ``Users`` file in ``Configuration/Identity`` contains the default admin username and password for the first login

