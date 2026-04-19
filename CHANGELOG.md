# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/).

## [1.2.2] - 2026-04-19
### Added
- Añadido `ITaskRepository` y `TaskRepository` para listar tareas activas y marcarlas como completadas.
- Añadido `MainPageViewModel` con carga de tareas pendientes y comando para completar mediante check.

### Changed
- `MainPage` ahora muestra la lista de tareas pendientes con `CheckBox` para completarlas.
- Registrados `ITaskRepository` y `MainPageViewModel` en DI.

## [1.2.1] - 2026-04-16
### Added
- `MainPage` muestra los campos de la tabla `App` (IdApp, Version, RelativeOffsetMinutes, SnoozeMinutes).
- `MainPage` inyecta `IUnitOfWorkFactory` y `IAppRepository` para cargar los datos en `OnAppearing`.

## [1.2.0] - 2026-04-16
### Added
- Añadido `IUnitOfWork` con propiedades `Connection`, `Transaction`, `CommitAsync` y `RollbackAsync`.
- Añadido `UnitOfWork` con rollback automático en `DisposeAsync` si no se llamó a `CommitAsync`.
- Añadido `IUnitOfWorkFactory` y `UnitOfWorkFactory` para crear instancias de `IUnitOfWork`.
- Registrado `IUnitOfWorkFactory` como singleton en DI.

### Changed
- `IAppRepository.GetFirstAsync` y `IDatabaseSchemaRepository.CreateOrUpdateDatabaseAsync` aceptan `IUnitOfWork` por parámetro.
- `AppRepository` eliminó `IDbConnectionFactory` como dependencia; usa la conexión y transacción del `IUnitOfWork`.
- `DatabaseInitializer` gestiona el ciclo de vida del `IUnitOfWork`: crea, pasa a repositorio y hace commit.

## [1.1.2] - 2026-04-16
### Changed
- Movida la inicialización de la base de datos de `MauiProgram` a `App.OnStart()` para evitar el bloqueo del hilo principal con `.GetAwaiter().GetResult()`.
- `App` inyecta `IDatabaseInitializer` y lo llama de forma async en `OnStart`.
- Registrado `App` como singleton en DI.

## [1.1.1] - 2026-04-16
### Fixed
- Cambiados los tipos de `AppRecord` de `int` a `long` para compatibilidad con el tipo `INTEGER` (Int64) de SQLite que Dapper no convertía implícitamente.

## [1.1.0] - 2026-04-16
### Added
- Añadido `IDatabaseSchemaRepository` con método `CreateOrUpdateDatabaseAsync`.
- Añadido `DatabaseSchemaRepository` que detecta si la BD existe y ejecuta `database-v1.sql` si no hay registro en `App`.
- Añadido script embebido `Resources/Scripts/database-v1.sql` con el esquema completo (tablas, índices e inserción inicial de `App`).
- El script SQL se carga como `EmbeddedResource` con `Assembly.GetManifestResourceStream` evitando deadlocks por `FileSystem.OpenAppPackageFileAsync`.
- `DatabaseInitializer` delega en `IDatabaseSchemaRepository`.

## [1.0.9] - 2026-04-16
### Added
- Añadido log `LogDebug` en `DbConnectionFactory.CreateConnection` mostrando el database path.
- Inyectado `ILogger<DbConnectionFactory>` en `DbConnectionFactory`.
- Configurado nivel mínimo de logging a `Debug` en modo DEBUG en `MauiProgram`.

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
