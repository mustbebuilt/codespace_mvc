# ClassroomDB + Docker: How This Repo Works

## Purpose
This document explains the EF Core-based database setup in this repository and how it connects to the SQL Server container in Codespaces.

## Big Picture
There are two layers:

1. Source layer (versioned in Git)
- Location: `MyMvcApp/`
- Contains the EF Core model, `DbContext`, migrations, controllers, and views
- Safe to edit, commit, and review like application code

2. Runtime layer (live database engine)
- Location: Docker container (`classroom-sql`)
- Runs Microsoft SQL Server on port `1433`
- Stores live database data while the container exists

In short:
- `MyMvcApp/` = EF Core source of truth
- Docker SQL container = running database server and actual rows

## What the MVC App Does
The MVC app defines the data model and the database schema.

Key files:
- `MyMvcApp/Models/Student.cs`
  - Entity model with validation attributes.

- `MyMvcApp/Data/ClassroomDbContext.cs`
  - EF Core `DbContext` and table configuration.

- `MyMvcApp/Migrations/`
  - EF Core migrations that create and evolve the database schema.

- `MyMvcApp/Program.cs`
  - Applies any pending migrations automatically when the app starts.

## How It Connects to Docker
Typical workflow:

1. Start SQL Server in Docker
- Starts an isolated SQL Server process with credentials and port mapping.

2. Run the MVC app
- EF Core opens a connection to `localhost,1433` using `DefaultConnection`.
- `Database.Migrate()` applies any missing schema changes.

3. Use the app or SQL tools
- The app can read and write rows immediately.
- `sqlcmd` is optional for manual verification.

## Terms and Definitions

### Docker
A platform for running software in isolated environments called containers.

### Container
A lightweight isolated runtime for a process. Here, the process is SQL Server.

### Image
A packaged template used to create containers. Example: `mcr.microsoft.com/mssql/server:2022-latest`.

### Port Mapping
Connects a port on your machine to a port in the container, for example `1433:1433`.

### Entity Framework Core
An ORM that maps C# classes to database tables and handles database access.

### DbContext
The EF Core session object that manages entities, queries, and changes.

### Migration
A versioned schema change generated from the EF Core model.

### Schema
Database structure: tables, columns, keys, constraints, views, procedures, etc.

## Persistence Note
If the SQL container is started without a Docker volume, data is ephemeral:
- Container removed => data removed
- Repo files remain unchanged

To recover quickly after container recreation:
1. Start container
2. Run the MVC app again
3. EF Core re-applies migrations

## Quick Commands (Reference)

Start SQL Server container:

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

Run the MVC app:

```bash
cd /workspaces/codespace_mvc/MyMvcApp
dotnet run
```

Verify row count:

```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"
```
