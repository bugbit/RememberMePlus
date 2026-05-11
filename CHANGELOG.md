# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/).

## [1.5.0] - 2026-05-11
### Added
- Añadido Alarm Scheduler para programar la próxima tarea activa según `datetime_notify_at` sin considerar segundos.
- Añadido soporte Android con `AlarmManager`, notificación local sonora y ventana de atención con acciones de completar y posponer.
- Añadido fallback con `Timer` para entornos no Android.

### Changed
- El arranque de la aplicación normaliza notificaciones vencidas a la fecha-hora local actual y agenda la siguiente tarea a vencer.
- Las acciones de completar, posponer y crear tarea recalculan la próxima alarma programada.
- Incrementada la versión de la aplicación a `1.5.0` (`ApplicationVersion` 26).

## [1.4.2] - 2026-05-11
### Changed
- Reorganizada la capa de dominio de tareas en `Domain/Tasks` con entidad y value objects de recordatorio.
- Reorganizada la infraestructura SQLite/Dapper en carpetas de abstracciones, opciones, modelos, mappers, repositorios y esquema.
- Centralizado el registro de infraestructura en `Infrastructure/DependencyInjection.cs`.
- Incrementada la versión de la aplicación a `1.4.2` (`ApplicationVersion` 25).

## [1.4.1] - 2026-05-08
### Changed
- La pantalla `Añadir tarea` adopta el estilo visual de la pantalla principal con fondo cálido, cabecera destacada y formulario en tarjeta.
- Incrementada la versión de la aplicación a `1.4.1` (`ApplicationVersion` 24).

## [1.4.0] - 2026-05-08
### Added
- Añadida pantalla principal de atención con secciones separadas para tareas vencidas y tareas a punto de vencer.
- Añadida actualización automática de la pantalla principal y pulso visual para tareas críticas.
- Añadidas acciones rápidas de completar, posponer e ignorar temporalmente desde las tarjetas principales.

### Changed
- La vista principal usa `relative_offset_minutes` de la tabla `App` como ventana de próximo vencimiento.
- Incrementada la versión de la aplicación a `1.4.0` (`ApplicationVersion` 23).

## [1.3.3] - 2026-05-04
### Fixed
- Al crear tareas no recurrentes, `datetime_notify_at` ahora se guarda con el mismo valor de `date_due_at`.

## [1.3.2] - 2026-05-03
### Changed
- La pantalla `Añadir` ahora permite seleccionar también la hora de vencimiento (`DatePicker` + `TimePicker`) para tareas no recurrentes.
- El guardado provisional de tareas no recurrentes persiste `date_due_at` con fecha y hora (`yyyy-MM-dd HH:mm:ss`).

## [1.3.1] - 2026-05-03
### Added
- La pantalla `Añadir` ahora permite crear tareas no recurrentes con título y fecha de vencimiento.

### Changed
- La pantalla `Añadir` muestra explícitamente que el alta es provisionalmente solo para tareas no recurrentes.

## [1.3.0] - 2026-05-02
### Added
- Añadida nueva pestaña `Añadir` en `AppShell` con la página `AddTaskPage`.

### Changed
- `AppShell` ahora usa `TabBar` con dos pestañas: `Home` y `Añadir`.

## [1.2.12] - 2026-05-01
### Fixed
- `TaskRepository.CompleteAsync` ahora cambia `is_active` a `0` solo cuando la tarea **no** es recurrente (`id_taskscheduler IS NULL` e `id_task_event IS NULL`).

## [1.2.11] - 2026-05-01
### Fixed
- Ajustada la detección de tarea recurrente en `TaskRepository.CompleteAsync`: ahora solo cambia `is_active` cuando `id_taskscheduler` o `id_task_event` no es `NULL`.

## [1.2.10] - 2026-05-01
### Changed
- `TaskRepository.CompleteAsync` ahora guarda `date_due_at_last = date_due_at`, actualiza `date_due_at` a la fecha local actual y solo cambia `is_active` cuando la tarea tiene recurrencia (`id_task_event IS NOT NULL`).

## [1.2.9] - 2026-05-01
### Fixed
- Corregida llamada en `DatabaseInitializer` a `IUnitOfWorkFactory.CreateAsync` usando argumento nombrado para `cancellationToken`, evitando el error de compilación por conversión `CancellationToken` -> `bool`.

## [1.2.8] - 2026-05-01
### Changed
- `IUnitOfWorkFactory.CreateAsync` ahora permite indicar `useTransaction` para crear una unidad de trabajo sin transacción cuando no sea necesaria.
- `UnitOfWork` crea transacción de forma opcional y maneja `CommitAsync`/`RollbackAsync` como no-op cuando no existe transacción activa.
- `MainPageViewModel.LoadAsync` crea `IUnitOfWork` sin transacción para lecturas simples de una sola consulta.

## [1.2.7] - 2026-05-01
### Fixed
- Corregido `TaskRepository.GetPendingTodayOrOverdueAsync`: reemplazo de `AsList()` por `ToList()` para evitar dependencia de extensión no resuelta.

## [1.2.6] - 2026-05-01
### Changed
- `IUnitOfWork` deja de heredar de `ISqlExecutor` y vuelve a exponer únicamente control transaccional (`CommitAsync`/`RollbackAsync`).
- `UnitOfWork` implementa explícitamente `ISqlExecutor` además de `IUnitOfWork`.
- Repositorios (`AppRepository`, `TaskRepository`, `DatabaseSchemaRepository`) usan cast seguro `unitOfWork is ISqlExecutor sqlExecutor` antes de ejecutar SQL.

## [1.2.5] - 2026-05-01
### Changed
- `IUnitOfWork` ya no expone `Connection` ni `Transaction`; ahora implementa `ISqlExecutor` para consultas y comandos SQL.
- `UnitOfWork` encapsula internamente la conexión/transacción y centraliza `QueryAsync`, `QueryFirstOrDefaultAsync` y `ExecuteAsync`.
- `AppRepository`, `TaskRepository` y `DatabaseSchemaRepository` dejan de acceder directamente a `DbConnection`/`DbTransaction`.

## [1.2.4] - 2026-04-20
### Changed
- `MainPage` ahora muestra tareas pendientes de hoy o vencidas.
- `TaskRepository` actualiza el filtro de la vista principal a tareas activas con `date_due_at <= fecha local actual`.
- `ITaskRepository` expone `GetPendingTodayOrOverdueAsync` para reflejar la intención del caso de uso.

## [1.2.3] - 2026-04-19
### Changed
- `MainPage` ahora se enfoca en mostrar únicamente las tareas pendientes de hoy.
- `TaskRepository` filtra tareas activas por fecha local actual (`date_due_at = hoy`) para la vista principal.

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
