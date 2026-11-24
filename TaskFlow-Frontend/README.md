# TaskFlow Frontend — standalone Blazor WebAssembly (.NET 10)

**Independent source repository** for tier 1. No backend project reference. A copy of the REST API's request/response **JSON models** lives under `DevOpsTasks.Frontend/Models/TaskModels.cs`. Keep the JSON contract in sync when changing either project.

## What it does

Responsive dashboard with task counts, search/filter, create, edit, delete, task priority and status. All data loads over HTTP from the backend. **Frontend has no PostgreSQL access and no database passwords.**

## Files

```text
TaskFlow-Frontend/
  DevOpsTasks.Frontend/
    DevOpsTasks.Frontend.csproj
    Program.cs
    App.razor
    _Imports.razor
    Models/TaskModels.cs
    Pages/Home.razor
    Services/TaskApiClient.cs
    Layout/MainLayout.razor
    wwwroot/{index.html,appsettings.json,css/app.css}
    Properties/launchSettings.json
  deploy/nginx-taskflow.conf
  .github/workflows/build.yml
```

## Run on your developer PC

1. Install **.NET 10 SDK**. Start PostgreSQL and the separate **TaskFlow-Backend** project first (backend at `http://localhost:5200`).
2. Check `DevOpsTasks.Frontend/wwwroot/appsettings.json`:

   ```json
   { "ApiBaseUrl": "http://localhost:5200/" }
   ```

3. From this repository directory:

   ```bash
   dotnet restore DevOpsTasks.Frontend/DevOpsTasks.Frontend.csproj
   dotnet run --project DevOpsTasks.Frontend
   ```

4. Visit **http://localhost:5100**. (The API has the corresponding localhost CORS rule.)

## Publish to the Azure Ubuntu Linux VM (NO Docker)

1. On the build machine:

   ```bash
   dotnet publish DevOpsTasks.Frontend/DevOpsTasks.Frontend.csproj -c Release -o publish/frontend
   ```

2. Copy the **contents** of `publish/frontend/wwwroot/` to `/var/www/taskflow/` on Ubuntu. Example setup on Ubuntu:

   ```bash
   sudo apt-get update
   sudo apt-get install -y nginx
   sudo mkdir -p /var/www/taskflow
   ```

   Transfer the static website files with SCP/SFTP or your CI pipeline, then ensure they are readable by Nginx.

3. **Important:** change the *deployed* `/var/www/taskflow/appsettings.json` to:

   ```json
   { "ApiBaseUrl": "/" }
   ```

   This routes browser API requests to Nginx on the same origin, rather than to `localhost` on each visitor's computer.

4. Copy `deploy/nginx-taskflow.conf` to `/etc/nginx/sites-available/taskflow`. Edit `proxy_pass http://10.0.2.4:8080;` and substitute the actual **private IP** of the Windows VM. Enable it:

   ```bash
   sudo ln -s /etc/nginx/sites-available/taskflow /etc/nginx/sites-enabled/taskflow
   sudo rm -f /etc/nginx/sites-enabled/default
   sudo nginx -t
   sudo systemctl reload nginx
   curl http://localhost/api/health
   ```

5. Open the Linux VM public site. For real internet exposure, configure **HTTPS** with DNS and a TLS certificate, and protect the app with authentication or narrow access to your course lab. The sample's HTTP config is for initial lab testing only; ideally also encrypt traffic between Nginx and IIS.

## Network requirements

- Browser -> Linux VM: ports **80/443**; restrict SSH **22** to administrators or use Bastion.
- Linux VM -> Windows VM: private TCP **8080** (Windows IIS API).
- The frontend does **not** connect to PostgreSQL or use TCP 5432.
- Frontend API endpoint prefix: `/api/`; the backend defines `GET /api/health` and `GET/POST/PUT/DELETE /api/tasks`.

## DevOps exercises

This repository has its own `.github/workflows/build.yml`. Use it to build/publish frontend artifacts, then create a separate release pipeline to copy the published `wwwroot` files to Ubuntu and reload Nginx.
