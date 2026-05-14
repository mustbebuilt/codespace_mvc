# ClassroomDB + Docker: How This Repo Works

## Purpose
This document explains how the database parts of this repository fit together, especially the relationship between the `ClassroomDB` folder and the Docker container where SQL Server runs.

## Big Picture
There are two layers:

1. Source layer (versioned in Git)
- Location: `ClassroomDB/`
- Contains database definitions and scripts
- Safe to edit, commit, and review like application code

2. Runtime layer (live database engine)
- Location: Docker container (`classroom-sql`)
- Runs Microsoft SQL Server on port `1433`
- Stores live database data while the container exists

In short:
- `ClassroomDB/` = blueprint and deployment artifacts
- Docker SQL container = running database server and actual rows

## What the ClassroomDB Folder Does
The `ClassroomDB/` folder defines what the database should look like and how to populate it.

Key files:
- `ClassroomDB/ClassroomDB.sqlproj`
  - SQL project file.
  - Used by `dotnet build` to produce a deployment package (`.dacpac`).

- `ClassroomDB/Tables/Students.sql`
  - Table schema definition (columns, keys, constraints).

- `ClassroomDB/Scripts/02_data.sql`
  - Seed script to insert sample student records.

- `ClassroomDB/bin/Debug/ClassroomDB.dacpac`
  - Build output artifact used by `sqlpackage` to deploy schema changes.

## How It Connects to Docker
Typical workflow:

1. Start SQL Server in Docker
- Starts an isolated SQL Server process with credentials and port mapping.

2. Publish schema from dacpac
- `sqlpackage` compares the dacpac model to the target database and applies differences.
- This creates/updates objects like tables.

3. Seed data (optional)
- Run `02_data.sql` to insert example rows.

4. App connects to SQL Server
- `MyMvcApp/appsettings.json` points to `localhost,1433` and `ClassroomDB`.

## Terms and Definitions

### Docker
A platform for running software in isolated environments called containers.

### Container
A lightweight isolated runtime for a process. Here, the process is SQL Server.

### Image
A packaged template used to create containers. Example: `mcr.microsoft.com/mssql/server:2022-latest`.

### Port Mapping
Connects a port on your machine to a port in the container (for example `1433:1433`). This lets local tools and the MVC app connect to SQL Server.

### SQL Project (`.sqlproj`)
A project format for database schema source code. Building it produces a deployable artifact (dacpac).

### DACPAC (`.dacpac`)
Data-tier application package. A compiled representation of database schema used for deployment and drift-safe updates.

### sqlpackage
CLI tool that publishes a dacpac to a SQL Server target. It performs model-based schema deployment.

### Schema
Database structure: tables, columns, keys, constraints, views, procedures, etc.

### Seed Data
Initial/sample rows inserted after schema creation, usually for testing, demos, or development.

## Persistence Note
If the SQL container is started without a Docker volume, data is ephemeral:
- Container removed => data removed
- Repo files remain unchanged

To recover quickly after container recreation:
1. Start container
2. Publish dacpac
3. Re-run seed script

## Quick Commands (Reference)

Start SQL Server container:

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

Publish schema:

```bash
sqlpackage /Action:Publish /SourceFile:/workspaces/codespace_mvc/ClassroomDB/bin/Debug/ClassroomDB.dacpac /TargetServerName:"localhost,1433" /TargetDatabaseName:ClassroomDB /TargetUser:sa /TargetPassword:"ClassroomPassword123!" /TargetTrustServerCertificate:True /p:AllowIncompatiblePlatform=true
```

Seed data:

```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -d "ClassroomDB" -i /workspaces/codespace_mvc/ClassroomDB/Scripts/02_data.sql
```

Verify row count:

```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"
```
