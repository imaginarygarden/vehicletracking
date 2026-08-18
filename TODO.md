# Roadmap

This roadmap records known gaps and possible directions. It is not a promise of
delivery dates. Discuss substantial items in an issue before implementation.

## Release blockers

- [x] Select and add an open-source `LICENSE`.
- [ ] Add a formal `CODE_OF_CONDUCT.md` and security-reporting policy.
- [ ] Add automated unit and integration test projects.
- [ ] Test clean install, upgrade, backup, and restore paths.
- [ ] Review authentication/session handling and production defaults.
- [ ] Add rate limiting and brute-force protection to login and registration.
- [ ] Change logout to a CSRF-protected state-changing request.
- [ ] Add database uniqueness constraints and normalization for usernames and
      email addresses.

## Security and privacy

- [ ] Implement deliberate user-agent and IP handling:
  - validate trusted proxy/client-IP behavior;
  - document the lawful purpose and user notice;
  - minimize stored data;
  - define retention and deletion periods;
  - avoid third-party reputation services without an explicit privacy review.
- [ ] Persist ASP.NET Core Data Protection keys outside the web container.
- [ ] Protect persisted Data Protection keys at rest using an appropriate
      platform key manager or certificate.
- [ ] Design server-side encryption for sensitive stored data, including key
      rotation, recovery, and backup implications.
- [ ] Add secrets-manager support for production instead of relying only on a
      plaintext `.env` file.
- [ ] Add security headers and a reviewed Content Security Policy.
- [ ] Add email verification, password reset, and optional multi-factor
      authentication.
- [ ] Add account/session management and remote session revocation.
- [ ] Add privacy controls for account export and permanent account deletion.
- [ ] Add audit events for security-sensitive and destructive actions without
      logging secrets or unnecessary personal data.
- [ ] Perform dependency, container-image, and threat-model reviews before a
      stable public release.

## Data portability

- [ ] Export vehicles, fuel entries, and calculated summaries.
- [ ] Support at least a documented JSON format for lossless backups.
- [ ] Consider CSV export/import for spreadsheet workflows.
- [ ] Validate imports with a preview and actionable row-level errors.
- [ ] Detect duplicates and define merge/replace behavior.
- [ ] Version the interchange format for forward compatibility.
- [ ] Ensure import/export always respects authenticated ownership.

## Vehicle features

- [ ] Maintenance schedules and service history.
- [ ] Repairs and associated costs.
- [ ] Insurance policies, renewal dates, and costs.
- [ ] TÜV/inspection records and reminders.
- [ ] Vehicle documents with secure storage rules.
- [ ] Notes and tags.
- [ ] Additional vehicle details such as make, model, model year, fuel type, and
      VIN where appropriate.
- [ ] Configurable currencies, distance units, and consumption units.
- [ ] Reminders/notifications for upcoming vehicle events.

## Fuel tracking and statistics

- [ ] Expand unit tests for full-tank and partial-fill calculation sequences.
- [ ] Define behavior for corrections, duplicate odometer readings, and entries
      inserted out of chronological order.
- [ ] Add charts for consumption, fuel price, distance, and cost trends.
- [ ] Add date-range filtering and yearly/monthly summaries.
- [ ] Consider per-entry notes, fuel type, station, and location.
- [ ] Consider optional trip or business/private mileage classification.
- [ ] Clearly document calculation assumptions in the UI.

## User experience and accessibility

- [ ] Complete keyboard and screen-reader accessibility review.
- [ ] Add localization and culture-aware date/number/currency formatting.
- [ ] Add user-selectable theme persistence.
- [ ] Improve mobile navigation and test all tables/dialogs at narrow widths.
- [ ] Add confirmation/feedback consistency across all mutations.
- [ ] Add onboarding guidance for first vehicle and first refueling.

## Quality and architecture

- [ ] Add unit tests for calculation and validation services.
- [ ] Add PostgreSQL integration tests for ownership and EF mappings.
- [ ] Add authorization tests proving cross-user isolation.
- [ ] Add end-to-end tests for registration, login, vehicle CRUD, and fuel CRUD.
- [ ] Add CI for restore, build, test, formatting/analyzers, and Docker build.
- [ ] Pin dependency versions consistently and automate update checks.
- [ ] Replace ambiguous boolean mutation results with structured application
      results that distinguish validation, not-found, forbidden, and persistence
      failures.
- [ ] Revisit the generic data-store boundary as feature complexity grows.
- [ ] Move toward feature-oriented folders when maintenance/insurance modules
      make the current shared folders crowded.

## Operations and deployment

- [ ] Add application and database health/readiness endpoints.
- [ ] Add production logging guidance, metrics, and alerting.
- [ ] Persist and protect Data Protection keys in Compose deployments.
- [ ] Provide tested reverse-proxy examples for at least one supported proxy.
- [ ] Automate PostgreSQL backups and verify restore procedures.
- [ ] Document PostgreSQL major-version upgrades.
- [ ] Add graceful migration coordination before supporting multiple web
      replicas.
- [ ] Define container/image tagging and release/versioning strategy.
- [ ] Add a production-oriented Compose example or deployment manifests without
      embedding secrets.
