# 🎒 Lab Guide: Setting Up Your .NET MVC & SQL Server Development Environment

Welcome to your development environment! This lab uses **GitHub Codespaces**, meaning you do not need to install anything on your local computer. Everything runs directly inside your web browser.

For the current project-specific database runbook (including `sqlcmd` fixes, migration verification, and 40-student seeding), see [dbsetup.md](dbsetup.md).

Follow these step-by-step instructions in the same order used to get this application running in this repository.

---

## 🚀 Step 1: Launch Your GitHub Codespace

1. At the top of this GitHub repository page, click the green **Code** button.
2. Select the **Codespaces** tab.
3. Click **Create codespace on main**.
4. Wait 1–2 minutes for the cloud environment to build. When it finishes, a version of Visual Studio Code will open right in your browser.

---

## 🐳 Step 2: Initialize Your Services

Your workspace runs two environments side-by-side: a **.NET application workspace** and a **Microsoft SQL Server database container**.

1. If prompted in the bottom-right corner to **"Rebuild Container"**, click it. 
2. If you don't see a prompt, open the Command Palette by pressing `Ctrl + Shift + P` (or `Cmd + Shift + P` on Mac).
3. Type `Codespaces: Rebuild Container` and press **Enter**.
4. Once the rebuild completes, open a terminal window (`Ctrl + ` ` ` or via the top menu: **Terminal > New Terminal**).

---

## 🛠️ Step 3: Start SQL Server

To create the Docker container with SQL Server, run:

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='ClassroomPassword123!' -p 1433:1433 --name classroom-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

---

## 🔧 Step 4: Ensure `sqlcmd` Is Available

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

---

## 🏃‍♂️ Step 5: Run the Existing Application

Use the existing app in this repository and run startup (migrations + seed):

```bash
cd /workspaces/codespace_mvc/MyMvcApp
dotnet run
```

Startup applies migrations and seeds students when the table is empty.

To verify seeded row count (expected: **40**):

```bash
sqlcmd -I -S "localhost,1433" -U sa -P "ClassroomPassword123!" -No -Q "SELECT COUNT(*) AS TotalStudents FROM ClassroomDB.dbo.Students"
```

---

## 🧪 Optional: Build a Fresh MVC App From Scratch

Let's generate a fresh .NET MVC application and install the required database packages.

1. In your Codespace terminal, run this command to generate the template:
   ```bash
   dotnet new mvc -n MyMvcApp
   ```
2. Navigate into your new project folder:
   ```bash
   cd MyMvcApp
   ```
3. Install the Entity Framework Core packages needed to communicate with SQL Server:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Design
   ```
4. Open the file browser on the left side of your screen, open `MyMvcApp`, and click on `appsettings.json`. 
5. Replace its contents or add the `ConnectionStrings` section so it links directly to your containerized SQL Server:
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=ClassroomDB;User Id=sa;Password=ClassroomPassword123!;TrustServerCertificate=True;"
     }
   }
   ```

---

## ▶️ Optional: Run Your Fresh App

Let's test the application to ensure the web server boots successfully.

1. Inside your terminal (ensure you are still in the `MyMvcApp` folder), run:
   ```bash
   dotnet run
   ```
2. Look at the bottom-right corner of your browser. A pop-up will appear saying: **"Your application running on port 5001 is available."**
3. Click the **Open in Browser** button on that pop-up.
4. Your live, running .NET MVC website will open in a new browser tab.
5. **To stop the application:** Click inside your terminal window and press `Ctrl + C`.

---

## 🛑 Troubleshooting

* **My port forwarded webpage shows an error:** Ensure you ran `dotnet run` inside the `MyMvcApp` folder. If the terminal is frozen, press `Ctrl + C` to cancel it and try again.
* **The SQL Server connection fails:** Ensure the background containers are fully loaded. You can verify this by checking the **Ports** tab in the bottom panel; port `1433` should have a green checkmark next to it. If it is missing, run a container rebuild as detailed in Step 2.

## ✅ Common Command Issues

* **`Invalid object name 'ClassroomDB.dbo.Students'`**
   Run app startup once so migrations create the table.

* **`DELETE failed ... QUOTED_IDENTIFIER`**
   Use `sqlcmd -I` (capital i) exactly as shown.

* **Terminal shows `>` and hangs**
   There is likely an unclosed quote. Press `Ctrl + C` and rerun command as one line.

* **`Address already in use` on `dotnet run`**
   Use:

   ```bash
   ASPNETCORE_URLS="http://127.0.0.1:5299" dotnet run --no-launch-profile
   ```

---

## 🗄️ Final Optional: SQL Server GUI Guide

Use this only if you want to browse tables and run queries from the VS Code SQL extension UI.

1. On the left sidebar, click the **SQL Server** icon.
2. In the connection pane, click **+ Add Connection**.
3. Enter the following settings:
   * **Server Name:** `localhost`
   * **Database Name:** `ClassroomDB` (or press Enter to browse all)
   * **Authentication Type:** `SQL Login`
   * **User Name:** `sa`
   * **Password:** `ClassroomPassword123!`
   * **Save Password:** `Yes`
   * **Connection Name:** `Classroom Connection`

After connecting, expand **Tables** to inspect data or right-click the connection and choose **New Query**.


