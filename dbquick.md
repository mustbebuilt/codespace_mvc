Quick fix when database is not running:

1) Start SQL Server container
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest

2) Install sqlpackage if needed
dotnet tool install -g microsoft.sqlpackage

3) Use sqlcmd from installed location (or add to PATH)
export PATH="$PATH:/opt/mssql-tools18/bin"

4) Deploy database schema from existing dacpac
sqlpackage /Action:Publish /SourceFile:/workspaces/codespace_mvc/ClassroomDB/bin/Debug/ClassroomDB.dacpac /TargetServerName:"localhost,1433" /TargetDatabaseName:ClassroomDB /TargetUser:sa /TargetPassword:"ClassroomPassword123!" /TargetTrustServerCertificate:True /p:AllowIncompatiblePlatform=true

5) Verify DB is reachable and created
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT @@VERSION AS Version, DB_ID('ClassroomDB') AS DbId;"

6) Verify Students table
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT name FROM ClassroomDB.sys.tables;"

7) Seed Students data from script
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -d "ClassroomDB" -i /workspaces/codespace_mvc/ClassroomDB/Scripts/02_data.sql

8) Verify seeded row count
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"