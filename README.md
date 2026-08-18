# Vehicle Tracking

Vehicle Tracking is a self-hosted ASP.NET Core Blazor application for keeping
vehicle and refueling records in one place. It provides per-vehicle fuel
history and derives mileage, consumption, and cost statistics from odometer and
fuel data.

The project is under active development. Vehicle and fuel tracking are usable,
but several security, privacy, portability, and operational features are still
planned. Review [Known limitations](#known-limitations) and [TODO.md](TODO.md)
before exposing an instance to the internet or storing sensitive production
data.

## Features

- Account registration, login, logout, and cookie-based sessions.
- Role-aware authorization, including blocked/banned-user handling.
- Owner-scoped access: users can only retrieve or mutate their own vehicles and
  fuel entries.
- Vehicle creation, listing, inline editing, and confirmed deletion.
- Refueling creation, editing, and deletion in focused MudBlazor dialogs.
- Refueling records containing date, odometer, liters, total price, and full
  tank status.
- Per-vehicle statistics:
  - average consumption in L/100 km;
  - total fuel cost and liters;
  - total recorded distance;
  - average price per liter;
  - cost per kilometer.
- Full-tank-aware consumption calculation. Partial fills contribute to totals
  and accumulate toward the next usable full-tank interval.
- Responsive MudBlazor UI with light and dark themes.
- PostgreSQL persistence through EF Core migrations.
- Multi-stage, non-root Docker image and Docker Compose development/deployment
  stack.

Consumption and other derived statistics are calculated from raw entries and
are not persisted. Invalid intervals, missing baselines, and non-increasing
odometer readings do not produce misleading consumption figures.

## Technology

- .NET 9 and ASP.NET Core
- Blazor Interactive Server
- MudBlazor
- Entity Framework Core with Npgsql/PostgreSQL
- Cookie authentication and authorization policies
- BCrypt password hashing and zxcvbn password-strength evaluation
- Docker and Docker Compose

## Architecture

```text
VehicleTracking.Web (composition root, HTTP, Blazor, MudBlazor)
    |-- references VehicleTracking.Application
    |       `-- references VehicleTracking.Domain
    `-- references VehicleTracking.Persistence
            |-- references VehicleTracking.Application
            `-- references VehicleTracking.Domain
```

The feature boundaries are intentionally small:

- `IVehicleRepository` / `VehicleRepository` own vehicle operations.
- `IFuelRepository` / `FuelRepository` own refueling operations.
- `IFuelCalculationService` / `FuelCalculationService` own derived statistics.
- `IAuthService` and `IVerificator` own authentication and verification rules.
- `VehicleTrackingDbContext` and EF configurations own persistence details.
- Razor components coordinate UI state; calculation and ownership logic stays
  outside the markup.

This structure is intended to allow maintenance, repairs, insurance,
inspections, documents, and notes to become separate feature areas instead of
being added to one large vehicle service or dialog.

## Prerequisites

Choose the requirements for the way you want to run the project:

### Local IDE development

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Docker Engine or Docker Desktop with Docker Compose, unless PostgreSQL is
  installed separately
- An IDE such as Rider, Visual Studio, or VS Code

### Docker deployment

- Docker Engine or Docker Desktop
- Docker Compose v2+

## Environment configuration

Copy the committed template before the first start:

```powershell
Copy-Item .env.template .env
```

On Linux or macOS:

```bash
cp .env.template .env
```

The application validates its required variables during startup. Docker
Compose also reads the root `.env` automatically. The real `.env` is ignored by
Git and by the Docker build context; never commit it.

| Variable | Purpose |
| --- | --- |
| `CONNECTION_STRING` | PostgreSQL connection used when running the web project directly. Compose overrides it inside the web container so the database host becomes `db`. |
| `CREDENTIALS_MAX_LENGTH` | Maximum configured length for credential-related database fields. |
| `MISC_MAX_LENGTH` | Maximum configured length for general short text fields such as license plates. |
| `STANDARD_ROLE` | Role assigned to newly registered users. |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core environment: normally `Development` locally and `Production` for deployment. |
| `ASPNETCORE_URLS` | URLs used when the project is run directly. Compose binds the container to port `8080` internally. |
| `ASPNETCORE_FORWARDEDHEADERS_ENABLED` | Set to `true` only when deployed behind a trusted reverse proxy that supplies forwarded headers. |
| `AllowedHosts` | Host names accepted by ASP.NET Core. Use the actual public host in production instead of `*`. |
| `LOGIN_PATH` | Authentication login route. |
| `ACCESS_DENIED_PATH` | Authorization failure route. |
| `NOT_FOUND_PATH` | Not-found route. |
| `BCRYPT_FACTOR` | BCrypt work factor. Higher values increase password-hashing cost. |
| `BCRYPT_ENHANCED` | Enables BCrypt enhanced entropy mode. Do not change this casually after users exist. |
| `AUTH_EXPIRATION_HOURS` | Authentication cookie/session lifetime. |
| `AUTH_REFRESH_MINUTES` | Interval before the application revalidates and refreshes a session. |
| `AUTH_PASSWORD_MIN_STRENGTH` | Minimum zxcvbn-derived password score used in Production. |
| `APP_HOST` | Host address to which Compose publishes the web port. Keep `127.0.0.1` when a reverse proxy runs on the same host. |
| `APP_PORT` | Host port mapped to container port `8080`. |
| `POSTGRES_BIND_HOST` | Host address to which the PostgreSQL port is published. Keep this at `127.0.0.1`; PostgreSQL should not be internet-facing. |
| `POSTGRES_PORT` | Host PostgreSQL port for IDE/local access. Containers always use internal port `5432`. |
| `POSTGRES_VERSION` | PostgreSQL Docker image tag. Do not change major versions without following PostgreSQL upgrade procedures. |
| `POSTGRES_USER` | Database user initialized by the PostgreSQL image. |
| `POSTGRES_PASSWORD` | Database password. Replace the development value before deployment. |
| `POSTGRES_DB` | Database initialized by the PostgreSQL image. |

Changes to PostgreSQL initialization variables only affect creation of a new,
empty database volume. Updating the values while reusing an initialized volume
does not recreate its user or database.

## Local development

### Option A: PostgreSQL in Docker, application in the IDE

This is the recommended development workflow.

1. Create and review `.env`.
2. Ensure `CONNECTION_STRING` points to `127.0.0.1` and the configured
   `POSTGRES_PORT`.
3. Start only PostgreSQL:

   ```powershell
   docker compose up -d db
   ```

4. Restore and build:

   ```powershell
   dotnet restore
   dotnet build VehicleTracking.sln
   ```

5. Run `VehicleTracking.Web` from the IDE, or run:

   ```powershell
   dotnet run --project VehicleTracking.Web
   ```

6. Open one of the URLs configured in `ASPNETCORE_URLS`.

Pending EF Core migrations are applied automatically when the application
starts.

Useful database-only commands:

```powershell
docker compose ps db
docker compose logs -f db
docker compose stop db
```

### Option B: PostgreSQL installed locally

Create the database and user yourself, update `CONNECTION_STRING`, and run the
web project normally. The application will apply pending migrations on startup.
The `POSTGRES_*` Compose variables are irrelevant in this mode.

## Local Docker deployment

Build and start the web application and PostgreSQL together:

```powershell
docker compose up --build -d
```

Open `http://localhost:8080`, or use the values configured by `APP_HOST` and
`APP_PORT`.

Inspect the stack:

```powershell
docker compose ps
docker compose logs -f web
docker compose logs -f db
```

Stop and remove containers while retaining PostgreSQL data:

```powershell
docker compose down
```

Start or rebuild again:

```powershell
docker compose up --build -d
```

Delete containers and all PostgreSQL data for a completely clean reinstall:

```powershell
docker compose down --volumes
docker compose up --build -d
```

The database is stored in the Compose-managed `postgres_data` volume. Never use
`--volumes` unless deleting the database is intentional.

## Internet deployment

> [!WARNING]
> The repository is currently a pre-release project, not a fully hardened
> production service. In particular, protected Data Protection key persistence,
> server-side data encryption, audit logging, rate limiting, and automated
> backup/restore verification remain roadmap items. Do not store sensitive data
> until the risks are acceptable for your environment.

Do not expose the development server or PostgreSQL directly to the internet.
A minimum single-server topology is:

```text
Internet
   |
 HTTPS (443)
   |
Trusted reverse proxy (Caddy, Nginx, Apache, Traefik, or a managed ingress)
   |
http://127.0.0.1:8080
   |
Vehicle Tracking web container ---- private Compose network ---- PostgreSQL
```

For a same-host reverse proxy, configure at least:

```dotenv
ASPNETCORE_ENVIRONMENT="Production"
ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
AllowedHosts="tracking.example.com"

APP_HOST="127.0.0.1"
APP_PORT=8080
POSTGRES_BIND_HOST="127.0.0.1"
POSTGRES_PORT=5432

POSTGRES_USER="vehicletracking"
POSTGRES_PASSWORD="replace-with-a-long-random-secret"
POSTGRES_DB="vehicletracking"
```

Also review the BCrypt and session settings. Keep the web application bound to
loopback when the reverse proxy runs on the same server. Keep PostgreSQL bound
to loopback in every case.

The reverse proxy must:

- terminate TLS with a valid certificate;
- redirect public HTTP traffic to HTTPS;
- preserve the original host;
- send `X-Forwarded-For` and `X-Forwarded-Proto`;
- support WebSocket upgrades and long-lived connections required by Blazor
  Interactive Server;
- apply sensible request-size, timeout, and rate limits.

Only enable forwarded headers when the application is reachable exclusively
through a trusted proxy. ASP.NET Core warns that forwarded headers from
untrusted sources can allow IP or scheme spoofing. See Microsoft's
[proxy and load balancer guidance](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer)
and [Linux/Nginx hosting guidance](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx).

Start the deployment after reviewing `.env`:

```powershell
docker compose up --build -d
```

For updates, take a database backup first, pull the new revision, review schema
and `.env.template` changes, and then rebuild:

```powershell
git pull
docker compose up --build -d
```

EF migrations run automatically at application startup. This is convenient for
a single instance, but production operators should always back up before an
upgrade. Do not run multiple upgrading replicas concurrently until migration
coordination is implemented.

ASP.NET Core Data Protection keys should be persisted outside an ephemeral
container and protected at rest before relying on stable sessions across
container replacement or multiple replicas. See Microsoft's
[Data Protection guidance for containers](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview#persisting-keys-when-hosting-in-a-docker-container).

## Database backup and restore

Backups are not automated yet. At minimum, use PostgreSQL's `pg_dump` and test
restores regularly. A named Docker volume is persistence, not a backup: deleting
the volume, disk failure, or operator error can still destroy the data.

Before upgrading PostgreSQL to another major version, follow the official
PostgreSQL upgrade process. Changing `POSTGRES_VERSION` alone is not a database
upgrade strategy.

## Known limitations

- User-agent and IP verification/logging are placeholders.
- Application-level/server-side encryption of stored user data is not yet
  implemented.
- Docker Data Protection key persistence and key-at-rest protection are not yet
  configured.
- Import/export and user-controlled backups are not implemented.
- Maintenance, repairs, insurance, inspections, documents, and notes are not
  implemented.
- Automated unit, integration, and end-to-end test projects are not yet present.
- Production monitoring, health endpoints, rate limiting, and automated backups
  are not yet included.
- The current Compose topology is intended for a single web instance.

See [TODO.md](TODO.md) for the prioritized roadmap.

## Contributing

Contributions, bug reports, documentation fixes, and design discussions are
welcome. Read [CONTRIBUTING.md](CONTRIBUTING.md) before opening a pull request.

For security vulnerabilities, do not open a public issue containing exploit or
sensitive details. Use GitHub's private vulnerability reporting feature when it
is enabled, or contact the repository maintainer privately.

## License

No open-source license has been selected yet. Until a `LICENSE` file is added,
the source is publicly visible but normal copyright restrictions still apply.
Selecting and adding a license is a release-blocking roadmap item.
