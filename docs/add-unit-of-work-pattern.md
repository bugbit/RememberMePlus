# add-unit-of-work-pattern

## Objetivo del cambio
Introducir el patrón Unit of Work para permitir que los repositorios compartan una misma conexión y transacción sin gestionarlas internamente.

## Problema previo
Cada repositorio abría su propia conexión y transacción de forma independiente, lo que impedía que varias operaciones participaran en una misma transacción atómica.

## Solución
Implementar `IUnitOfWork` como abstracción que encapsula una `DbConnection` y un `DbTransaction` activos. Los repositorios reciben el `IUnitOfWork` por parámetro en cada método, manteniéndose stateless.

## Alcance
### Nuevos artefactos
| Archivo | Descripción |
|---|---|
| `IUnitOfWork` | Contrato: `Connection`, `Transaction`, `CommitAsync`, `RollbackAsync`, `IAsyncDisposable` |
| `UnitOfWork` | Implementación: rollback automático en `DisposeAsync` si no se llamó a `CommitAsync` |
| `IUnitOfWorkFactory` | Factoría registrada en DI para crear instancias de `IUnitOfWork` |
| `UnitOfWorkFactory` | Implementación de `IUnitOfWorkFactory` usando `IDbConnectionFactory` |

### Cambios en repositorios
- `IAppRepository.GetFirstAsync(IUnitOfWork, CancellationToken)`: usa `uow.Connection` y `uow.Transaction`.
- `IDatabaseSchemaRepository.CreateOrUpdateDatabaseAsync(IUnitOfWork, CancellationToken)`: ídem.
- `AppRepository` eliminó `IDbConnectionFactory` como dependencia directa.

### Cambios en inicialización
- `DatabaseInitializer` crea el `IUnitOfWork` vía `IUnitOfWorkFactory`, lo pasa al repositorio y llama `CommitAsync`.

## Decisiones técnicas
- Se pasa `IUnitOfWork` **por parámetro** en lugar de inyectarlo por DI, porque MAUI no tiene scope de request y un UoW inyectado como Singleton/Transient generaría conexiones inconsistentes.
- `IUnitOfWorkFactory` sí se registra en DI como Singleton, ya que es stateless.
- El rollback automático en `DisposeAsync` garantiza que si se produce una excepción antes del `CommitAsync`, los cambios nunca se persisten.

## Impacto en arquitectura
- Los repositorios son completamente stateless.
- El caller (servicio o inicializador) tiene control explícito sobre el alcance de la transacción.
- Compatible con Clean Architecture y DDD sin introducir dependencias en capas superiores.

## Validaciones realizadas
- Compilación correcta.
