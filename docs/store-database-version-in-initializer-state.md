# store-database-version-in-initializer-state

## Objetivo del cambio
Modificar la inicialización para no devolver la versión de base de datos y almacenarla internamente para tratamiento posterior.

## Alcance
- Se cambió la firma de `IDatabaseInitializer.InitializeAsync` a `Task`.
- `DatabaseInitializer.InitializeAsync` ya no retorna versión.
- La versión leída se guarda en el campo `_databaseVersion`.

## Decisiones técnicas
- Se mantiene la lectura del primer registro de `App` vía `IAppRepository`.
- Se conserva el comportamiento actual de degradación a `app = null` ante excepción.

## Impacto en arquitectura
- Se mantiene el uso de abstracciones (`IDatabaseInitializer`, `IAppRepository`).
- No se añade lógica de negocio en UI.

## Validaciones realizadas
- Compilación correcta tras ajustar contrato e implementación.
