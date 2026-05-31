## Quick Start for Codespaces

This repository already contains the MVC app and SQL project. To get the web app working in Codespaces with a database, do these steps in order.

### 1) Start SQL Server

Run the SQL Server container if it is not already running:

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

If the container already exists, start it instead:

```bash
docker start classroom-sql
```

### 2) Install the database tools

Install `sqlpackage` once per Codespace if it is missing:

```bash
dotnet tool install -g microsoft.sqlpackage
```

Install `sqlcmd` if your terminal says `sqlcmd: command not found`:

```bash
sudo apt-get update
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev
export PATH="$PATH:/opt/mssql-tools18/bin"
```

If you want that PATH change to persist, add this once:

```bash
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc
```

### 3) Build the SQL project

From the repository root, build the dacpac:

```bash
dotnet build /workspaces/codespace_mvc/ClassroomDB/ClassroomDB.sqlproj
```

### 4) Publish the database

Publish the built dacpac to the local SQL Server instance:

```bash
sqlpackage /Action:Publish /SourceFile:/workspaces/codespace_mvc/ClassroomDB/bin/Debug/ClassroomDB.dacpac /TargetServerName:"localhost,1433" /TargetDatabaseName:ClassroomDB /TargetUser:sa /TargetPassword:"ClassroomPassword123!" /TargetTrustServerCertificate:True /p:AllowIncompatiblePlatform=true
```

### 5) Seed sample data

Seed the Students table if you want sample rows in the app:

```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -d "ClassroomDB" -i /workspaces/codespace_mvc/ClassroomDB/Data/Students_Seed.sql
```

### 6) Run the web app

The MVC app already points to the local database in [MyMvcApp/appsettings.json](MyMvcApp/appsettings.json) and wires the connection in [MyMvcApp/Program.cs](MyMvcApp/Program.cs).

Start it from the app folder:

```bash
cd /workspaces/codespace_mvc/MyMvcApp
dotnet run
```

Open the forwarded port that VS Code shows. The app should now be able to read and write `ClassroomDB` through the `DefaultConnection` string.

### 7) Quick checks

Use these commands if you want to confirm the setup:

```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT DB_ID('ClassroomDB') AS DbId;"
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students;"
```

## Notes

- The app uses SQL Server, not EF migrations, to create the schema in this setup.
- If `dotnet run` fails to connect, check that the SQL container is running and that the dacpac publish step completed successfully.
- If you reseed the database, the sample data script may fail on duplicate rows.