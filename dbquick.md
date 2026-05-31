## Quick Start for Codespaces

This repository uses Entity Framework Core migrations to create and update the database.

### 1) Start SQL Server

Run the SQL Server container if it is not already running:

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

If the container already exists, start it instead:

```bash
docker start classroom-sql
```

### 2) Run the MVC app

From the repository root, start the app:

```bash
cd /workspaces/codespace_mvc/MyMvcApp
dotnet run
```

At startup, the app applies any pending migrations and creates or updates `ClassroomDB`.

### 3) Optional checks

Use these commands if you want to confirm the setup:

```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT DB_ID('ClassroomDB') AS DbId;"
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students;"
```

## Notes

- The app uses EF Core migrations as the source of truth for schema changes.
- If `dotnet run` fails to connect, check that the SQL container is running and that `DefaultConnection` still points to `localhost,1433`.
- If you change the model or `ClassroomDbContext`, add a migration before rerunning the app.

## Glossary

- `docker`: A tool that runs software in isolated containers. In this project, it runs SQL Server in the background.
- `EF Core`: Short for Entity Framework Core, the ORM used by the app to map C# classes to database tables.
- `DbContext`: The EF Core class that represents the database session and exposes entity sets.
- `migration`: A versioned change to the database schema created from the EF model.
- `sqlcmd`: A command-line tool for sending SQL queries to SQL Server.
- `SQL Server`: The database engine that stores and manages the ClassroomDB data.