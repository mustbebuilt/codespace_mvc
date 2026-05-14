Quick fix when database is not running:

1) Start SQL Server container
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest

2) Install sqlpackage if needed
dotnet tool install -g microsoft.sqlpackage

3) Install mssql-tools18 if sqlcmd is not found
curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list | sudo tee /etc/apt/sources.list.d/mssql-release.list
sudo apt-get update
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev

4) Add sqlcmd to PATH (current session)
export PATH="$PATH:/opt/mssql-tools18/bin"

   To persist across sessions:
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc

5) Deploy database schema from existing dacpac
sqlpackage /Action:Publish /SourceFile:/workspaces/codespace_mvc/ClassroomDB/bin/Debug/ClassroomDB.dacpac /TargetServerName:"localhost,1433" /TargetDatabaseName:ClassroomDB /TargetUser:sa /TargetPassword:"ClassroomPassword123!" /TargetTrustServerCertificate:True /p:AllowIncompatiblePlatform=true

6) Verify DB is reachable and created
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT @@VERSION AS Version, DB_ID('ClassroomDB') AS DbId;"

7) Verify Students table
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT name FROM ClassroomDB.sys.tables;"

8) Seed Students data from script
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -d "ClassroomDB" -i /workspaces/codespace_mvc/ClassroomDB/Scripts/02_data.sql

9) Verify seeded row count
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"