## ClassroomDB Setup in Codespaces

### Prerequisites
Install `sqlpackage` (only needed once per Codespace):
```bash
dotnet tool install -g microsoft.sqlpackage
```

---

### 1 — Build the SQL project
```bash
dotnet build /workspaces/codespace_mvc/ClassroomDB/ClassroomDB.sqlproj
```

### 2 — Deploy to the local SQL Server
```bash
sqlpackage \
  /Action:Publish \
  /SourceFile:/workspaces/codespace_mvc/ClassroomDB/bin/Debug/ClassroomDB.dacpac \
  /TargetServerName:"localhost,1433" \
  /TargetDatabaseName:ClassroomDB \
  /TargetUser:sa \
  /TargetPassword:"ClassroomPassword123!" \
  /TargetTrustServerCertificate:True
```

### 3 — (Optional) Seed the Students table
```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" \
  -No -i /workspaces/codespace_mvc/ClassroomDB/Data/Students_Seed.sql
```

---

### Verify
```bash
sqlcmd -S "localhost,1433" -U sa -P "ClassroomPassword123!" \
  -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"
```

---

**Notes:**
- Step 2 is idempotent — re-running it applies any schema changes without dropping data (dacpac diff deploy).
- Step 3 will fail on re-run due to the unique email constraint — only run it against an empty table.
- The connection string in appsettings.json already matches these credentials, so the MVC app will connect automatically after step 2.