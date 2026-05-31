## ClassroomDB Setup in Codespaces

This repo uses EF Core migrations from the MVC app (no separate SQL project deployment).

## 0) One-time tools setup

### Install EF CLI (only needed if you create/manage migrations manually)

```bash
dotnet tool install -g dotnet-ef
```

### Install sqlcmd (fixes "sqlcmd: command not found")

Codespaces often starts without `sqlcmd` preinstalled, so check it first:

```bash
which sqlcmd
```

If that prints nothing, install it once with:

```bash
curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | sudo gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/24.04/prod noble main" | sudo tee /etc/apt/sources.list.d/microsoft-prod.list >/dev/null
sudo apt-get update
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev
sudo ln -sf /opt/mssql-tools18/bin/sqlcmd /usr/local/bin/sqlcmd
```

Then refresh the shell path in your current terminal:

```bash
source ~/.bashrc
hash -r
```

If you prefer, opening a brand-new terminal also picks up the new path.

Verify:

```bash
sqlcmd -?
```

If the terminal still cannot find `sqlcmd`, the new shell session may not have loaded yet. Open a fresh terminal or run:

```bash
source ~/.bashrc
hash -r
```

---

## 1) Start SQL Server

First run:

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

If container already exists:

```bash
docker start classroom-sql
```

---

## 2) Apply migrations + seed data

From the app folder:

```bash
cd /workspaces/codespace_mvc/MyMvcApp
dotnet run
```

At startup, the app:
- applies pending migrations
- seeds students only when `Students` is empty

Current seed size is 40 students.

---

## 3) Verify student count

Use a single-line command (avoid multiline quote issues):

```bash
sqlcmd -I -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"
```

Expected result after first successful seed: `40`.

---

## 4) Force reseed to 40 (dev reset)

If you already had existing rows (for example 3 older seeded rows), clear the table and run startup again:

```bash
sqlcmd -I -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "DELETE FROM ClassroomDB.dbo.Students"
ASPNETCORE_URLS="http://127.0.0.1:5299" dotnet run --no-launch-profile
```

Then recheck:

```bash
sqlcmd -I -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"
```

---

## Troubleshooting

- `sqlcmd: command not found`
  Install `mssql-tools18` from section "Install sqlcmd".

- `Invalid object name 'ClassroomDB.dbo.Students'`
  Migrations/seed have not run yet. Start the app once from [MyMvcApp](MyMvcApp).

- Prompt shows `>` and command never finishes
  You likely have an unclosed quote. Press `Ctrl+C`, then rerun the command as one full line.

- `Address already in use` when running app
  Another process is already using the default dev port. Use:

  ```bash
  ASPNETCORE_URLS="http://127.0.0.1:5299" dotnet run --no-launch-profile
  ```

- `DELETE failed ... QUOTED_IDENTIFIER`
  Use `sqlcmd -I` (capital i), as shown above.

---

## Notes

- EF Core migrations are the source of truth for schema changes.
- If you change [MyMvcApp/Models/Student.cs](MyMvcApp/Models/Student.cs) or [MyMvcApp/Data/ClassroomDbContext.cs](MyMvcApp/Data/ClassroomDbContext.cs), add a migration and rerun the app.