# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/).

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
