# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/).

## [1.0.8] - 2026-04-14
### Changed
- Updated `IDatabaseInitializer.InitializeAsync` to return `Task` instead of `Task<int?>`.
- Updated `DatabaseInitializer` to store database version in an internal field for later processing instead of returning it.

## [1.0.7] - 2026-04-14
### Changed
- Updated `DatabaseInitializer.InitializeAsync` to set `app = null` when `GetFirstAsync` throws an exception.

## [1.0.6] - 2026-04-14
### Changed
- Updated startup flow in `MauiProgram` to execute `IDatabaseInitializer.InitializeAsync()` before returning `MauiApp`.

## [1.0.5] - 2026-04-14
### Added
- Added `IAppRepository` as repository contract for reading the first `App` row.
- Added `IDatabaseInitializer` as database initialization contract.

### Changed
- Updated `DatabaseInitializer` to depend on `IAppRepository` instead of concrete `AppRepository`.
- Registered `IAppRepository` and `IDatabaseInitializer` in DI.

## [1.0.4] - 2026-04-14
### Added
- Added `AppRepository` to read the first row from `App`.
- Added `AppRecord` DTO for mapping the `App` table row.

### Changed
- Refactored `DatabaseInitializer.InitializeAsync` to use `AppRepository` instead of direct SQL query.
- Registered `AppRepository` in the DI container.

## [1.0.3] - 2026-04-14
### Added
- Added `InitializeAsync(CancellationToken)` in `DatabaseInitializer` to read database version from the first row of `App`.
- Added `Dapper` package for lightweight data access in infrastructure.

## [1.0.2] - 2026-04-14
### Added
- Added `CreateConnection()` to `IDbConnectionFactory` and implemented it in `DbConnectionFactory` using SQLite.
- Added package `Microsoft.Data.Sqlite` for SQLite connections.

## [1.0.1] - 2026-04-14
### Added
- Added `IDbConnectionFactory` and `DbConnectionFactory` to centralize SQLite database path resolution.
- Added empty `DatabaseInitializer` with dependency injection of `IDbConnectionFactory`.

### Changed
- Registered database infrastructure services in `MauiProgram` DI container.
