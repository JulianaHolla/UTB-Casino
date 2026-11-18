# UTB Casino: How to Run the Project

ASP.NET Core MVC (.NET 10) app with a MySQL 8 database that runs in Docker.

## 1. Prerequisites

Install these once per computer:

| Tool | Notes |
|---|---|
| [.NET 10 SDK](https://dotnet.microsoft.com/download) | Check with `dotnet --version` |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | Must be **running** (whale icon in the tray) |
| `dotnet-ef` tool | Installed in step 2 below |

Port **3307** on your computer must be free (the database uses it).

## 2. One-time setup

Open a terminal in the folder that contains `docker-compose.yml` (the `UTB_Casino_v4` folder).

**Run each command on its own line and don't add `#` comments after commands. Windows `cmd` doesn't support them.**

**Install the EF Core tool:**

```
dotnet tool install --global dotnet-ef
```

Close the terminal and open a new one afterwards so the tool is found.

**Start the database:**

```
docker compose up -d
```

Wait 20–30 seconds on the first start. Check that it is ready:

```
docker compose logs db
```

You should see `ready for connections` with `port: 3306`. If not, wait a bit and run the command again.

**Create the tables and seed data:**

```
dotnet ef database update -p UTB.Casino.Infrastructure -s UTB.Casino.Web
```

It should end with `Done.` The app never creates or migrates the database itself, so this step is required.

**Trust the HTTPS development certificate (fresh machines only):**

```
dotnet dev-certs https --trust
```

## 3. Run the app

From Visual Studio: set `UTB.Casino.Web` as the startup project and press **F5**.

From the terminal:

```
dotnet run --project UTB.Casino.Web
```

Then open:

- https://localhost:7100
- http://localhost:5100 (redirects to HTTPS)

## 4. Accounts

The migrations seed an `admin` and a `manager` account (`admin@casino.local`, `manager@casino.local`). Only password **hashes** are stored in the code, so ask the project authors for the passwords. Customers register themselves on the Register page (18+ only).

## 5. Run the tests

```
dotnet test
```

## 6. Everyday use

| What | Command |
|---|---|
| Start the database | `docker compose up -d` |
| Stop the database (keeps data) | `docker compose stop` |
| Check the database is up | `docker compose ps` |
| **Reset the database completely** (deletes all data) | `docker compose down -v` then `docker compose up -d` then the `dotnet ef database update` command from step 2 |

## 7. Troubleshooting

| Problem | Cause and fix |
|---|---|
| `Unable to connect to any of the specified MySQL hosts` | The database isn't running. Start Docker Desktop, then run `docker compose up -d`. |
| `Access denied for user 'casino_app'` | The Docker volume was created earlier with different credentials. Reset it: `docker compose down -v`, then `docker compose up -d`, then run the migration again. |
| Access denied for `'casino_app'@'localhost'` | Another MySQL installation may be using port 3307. Find it with `netstat -ano | findstr 3307` and stop it. |
| `Could not execute because the specified command or file was not found` (for `dotnet ef`) | The tool isn't installed or the terminal wasn't restarted. See step 2. |
| `Table '...' doesn't exist` | The migration wasn't applied. Run the `dotnet ef database update` command from step 2. |
| Wrong credentials even after a reset | A stale User Secret overrides `appsettings.json`. Check with `dotnet user-secrets list --project UTB.Casino.Web` and clear it with `dotnet user-secrets clear --project UTB.Casino.Web`. |
| Browser warns about the HTTPS certificate | Run `dotnet dev-certs https --trust` and restart the browser. |

## 8. Configuration reference

- Connection string: `UTB.Casino.Web/appsettings.json`, key `ConnectionStrings:MySQL`. Defaults match `docker-compose.yml` (`localhost`, port `3307`, database `utb_casino`, user `casino_app`).
- To override it without editing files, use User Secrets:
  ```
  dotnet user-secrets set "ConnectionStrings:MySQL" "server=localhost;port=3307;database=utb_casino;user=casino_app;password=YOUR_PASSWORD" --project UTB.Casino.Web
  ```
- Logs are written to `UTB.Casino.Web/logs/`. Check the newest file first when something fails.

## 9. Before sharing or submitting the project

Delete these folders to keep the zip small and avoid leaking machine-specific files: `.vs`, `bin`, `obj`, `UTB.Casino.Web/logs`. The other computer will rebuild what it needs.
