## ClassroomDB Setup in Codespaces

This repo uses Entity Framework Core migrations from the MVC app instead of a separate SQL project and dacpac deploy.

### Prerequisites
No separate database deployment tool is required for the normal path. The app uses the connection string in [MyMvcApp/appsettings.json](MyMvcApp/appsettings.json).

If you want to create new migrations later, install the EF CLI once:

```bash
dotnet tool install -g dotnet-ef
```

---

### 1 — Start SQL Server

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

If the container already exists, start it instead:

```bash
docker start classroom-sql
```

### 2 — Run the MVC app

```bash
cd /workspaces/codespace_mvc/MyMvcApp
dotnet run
```

`Program.cs` applies any pending migrations at startup, so the first run creates or updates the database automatically.

If you change the model, create a migration first and then rerun the app.

### 3 — Verify

```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" \
  -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"
```

---

**Notes:**
- EF Core migrations are now the source of truth for schema changes.
- If you change `Student.cs` or `ClassroomDbContext.cs`, add a new migration and rerun the app.
- The old `ClassroomDB` SQL project is no longer part of the normal setup path.