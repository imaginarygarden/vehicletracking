# Contributing to Vehicle Tracking

Thank you for considering a contribution. This document explains how to report
problems, propose changes, run the project, and keep additions consistent with
the existing architecture.

The project is still pre-release. Discussion before a large implementation is
especially valuable while contracts and deployment expectations are evolving.

## Ways to contribute

- Report a reproducible bug.
- Suggest or discuss a feature.
- Improve documentation, accessibility, validation, or user experience.
- Add tests for existing behavior.
- Implement an agreed roadmap item from [TODO.md](TODO.md).
- Review pull requests and help reproduce reported issues.

## Before starting

For a small fix, opening a pull request directly is fine. For a new feature,
schema change, authentication change, new dependency, or architectural
refactor, open an issue first and describe:

- the problem being solved;
- the proposed behavior;
- expected UI and data-model changes;
- security/privacy consequences;
- alternatives considered;
- migration or compatibility concerns.

This prevents contributors from spending time on a direction that does not fit
the project.

## Development setup

Requirements:

- .NET 9 SDK
- Docker Engine or Docker Desktop with Docker Compose
- Git

Clone the repository and create a private environment file:

```powershell
Copy-Item .env.template .env
docker compose up -d db
dotnet restore
dotnet build VehicleTracking.sln
dotnet run --project VehicleTracking.Web
```

On Linux or macOS, use `cp .env.template .env` instead of `Copy-Item`.

The database-only Compose service publishes PostgreSQL to the host, so the web
project can run under an IDE debugger. Pending migrations are applied when the
application starts. See [README.md](README.md#local-development) for all startup
options.

Never commit `.env`, passwords, connection strings, database dumps, production
logs, private keys, or real user data.

## Architecture expectations

Keep the existing project responsibilities intact:

- **Domain** contains entities, domain contracts, and enums. It must not depend
  on Web, Persistence, MudBlazor, or EF Core configuration.
- **Application** contains feature contracts, DTOs, repositories/services,
  validation, authentication behavior, and calculations.
- **Persistence** contains EF Core context/configuration, migrations, and data
  access implementations.
- **Web** contains composition/DI, HTTP endpoints, authentication integration,
  Razor components, and MudBlazor UI.

When adding a vehicle-related feature such as maintenance or insurance:

- give it a focused interface/service or repository;
- keep calculations and business rules out of Razor markup;
- use a dedicated dialog or page rather than expanding an unrelated dialog;
- verify resource ownership in Application/data access, not only in UI code;
- never trust an entity ID supplied by a client without checking ownership;
- calculate derived values from source data unless persistence is demonstrably
  necessary;
- follow existing DTO, DI, EF configuration, authorization, snackbar, and dialog
  conventions.

Avoid introducing a framework, mediator, mapping library, or generic abstraction
for a single use case. Prefer the smallest change that leaves the next feature
straightforward.

## Coding style

- Target the framework and nullable-reference settings already configured by the
  project.
- Use asynchronous APIs for I/O and keep method naming consistent with existing
  interfaces.
- Keep nullable annotations accurate and avoid the null-forgiving operator when
  normal control flow can prove a value exists.
- Use `decimal` for monetary quantities and measured fuel values.
- Store timestamps consistently in UTC; convert to local time only for display.
- Avoid broad `catch` blocks and never silently swallow exceptions.
- Use `ILogger` for diagnostic detail and show safe, actionable messages to
  users.
- Remove unused usings and resolve compiler/analyzer warnings introduced by a
  change.
- Match the existing MudBlazor visual language and test responsive layouts.
- Do not reformat or restructure unrelated files in the same pull request.

## Security and privacy

Authentication, authorization, import/export, IP addresses, user agents,
encryption, documents, and backups are security-sensitive areas.

Contributions in these areas should include:

- a brief threat/privacy analysis;
- secure defaults;
- retention and deletion behavior where personal data is involved;
- tests for ownership and failure paths;
- documentation for new environment variables or operator responsibilities;
- no secrets or sensitive values in logs or exceptions.

Do not open a public issue for a vulnerability that could put deployed users at
risk. Use GitHub private vulnerability reporting when enabled, or contact the
maintainer privately.

## Database changes

For a schema change:

1. Update the domain entity and its EF configuration.
2. Create a focused migration; do not edit an already-released migration.
3. Review generated SQL and cascade-delete behavior.
4. Preserve existing data and provide a safe transition for changed types or
   required columns.
5. Update `.env.template`, README, and roadmap when configuration or operational
   behavior changes.
6. Test both a fresh database and an upgrade from the previous schema when
   possible.

Do not use `EnsureCreated` as a replacement for migrations.

## Tests and verification

There is not yet a dedicated automated test solution, so adding one is welcome.
At minimum, every pull request must build cleanly:

```powershell
dotnet build VehicleTracking.sln --no-restore
```

For Docker-related changes, also run:

```powershell
docker compose config --quiet
docker compose build web
```

Relevant test coverage should accompany behavior changes. Prioritize:

- fuel calculation boundary cases;
- ownership isolation between users;
- authentication/session parsing and expiration;
- validation and error paths;
- EF configuration and migrations;
- responsive component behavior for substantial UI work.

Document any verification that could not be run and why.

## Commit and pull-request guidance

- Create a focused branch from the current default branch.
- Keep commits understandable and avoid mixing unrelated cleanup with behavior
  changes.
- Write an imperative, descriptive summary such as `Add CSV fuel export`.
- Explain user-visible behavior, architecture decisions, database changes, and
  deployment impact in the pull request.
- Include screenshots for meaningful UI changes.
- Link the related issue when one exists.
- Call out breaking changes and required `.env` updates prominently.

Pull-request checklist:

- [ ] The change solves a documented problem and stays within scope.
- [ ] Ownership and authorization are enforced server-side.
- [ ] New inputs and failure cases are validated.
- [ ] Business logic is outside Razor markup.
- [ ] Database changes include a reviewed migration.
- [ ] No secrets, personal data, build output, or local settings are committed.
- [ ] Documentation and `.env.template` are updated where necessary.
- [ ] The solution builds without new warnings.
- [ ] Relevant tests were added or the missing coverage is explained.
- [ ] UI changes were checked at desktop and mobile widths.

## Community expectations

Be respectful, constructive, and patient. Critique ideas and code rather than
people. Harassment, discrimination, personal attacks, or disclosure of another
person's private information are not acceptable.

A formal `CODE_OF_CONDUCT.md` is planned before the community grows.
