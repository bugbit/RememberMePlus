# Reorganize domain tasks and Dapper infrastructure

## Objetivo

Reorganizar las clases de tareas y acceso a datos para acercar la estructura del proyecto a Clean Architecture y DDD, separando explícitamente el dominio de recordatorios de los detalles SQLite/Dapper.

## Alcance

- Añadida la carpeta `Domain/Tasks` con la entidad `ReminderTask` y los value objects `ReminderTaskId`, `ReminderTitle` y `PostponeMinutes`.
- Añadido `ReminderPriority` como enum de prioridad del dominio.
- Reubicadas las abstracciones de infraestructura (`IDatabaseInitializer`, `IDbConnectionFactory`) bajo `Infrastructure/Data/Abstractions`.
- Reubicada la infraestructura SQLite/Dapper bajo `Infrastructure/Data/Dapper`, separando modelos, mappers, repositorios y esquema.
- Añadido `SqliteDataOptions` para centralizar el nombre del archivo SQLite.
- Añadido `Infrastructure/DependencyInjection.cs` para concentrar el registro de servicios de infraestructura.

## Decisiones técnicas

- `DapperReminderTaskRepository` mantiene SQL y Dapper dentro de infraestructura, evitando que el dominio conozca SQLite.
- `ReminderTaskDataModel` representa filas leídas desde SQLite y `ReminderTaskDataMapper` traduce ese modelo a `ReminderTask`.
- `SqliteDbConnectionFactory` usa `SqliteDataOptions.DatabaseFileName` para evitar literales duplicados.
- `MauiProgram` delega el registro de infraestructura en `AddInfrastructure()` para reducir acoplamiento de arranque.

## Impacto en arquitectura

- El dominio queda libre de dependencias de MAUI, SQLite y Dapper.
- La infraestructura conserva los detalles de persistencia y mapeo.
- La UI sigue consumiendo abstracciones registradas por DI; no instancia implementaciones concretas.
- La reorganización no cambia el esquema SQLite ni el comportamiento funcional existente.

## Validaciones realizadas

- Revisada la separación de carpetas solicitada para dominio e infraestructura.
- Validado que los constructores primarios asignan dependencias a campos `readonly` antes de usarlas.
- Intentada compilación con `dotnet build`, no ejecutable porque el SDK de .NET no está instalado en el entorno.
