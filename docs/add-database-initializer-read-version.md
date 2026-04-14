# add-database-initializer-read-version

## Objetivo del cambio
Implementar la inicialización mínima de base de datos para conocer la versión actual almacenada en SQLite.

## Alcance
- Se implementó `InitializeAsync` en `DatabaseInitializer`.
- La inicialización consulta `version` desde el primer registro de la tabla `App`.
- Se agregó `Dapper` como dependencia de acceso a datos.

## Decisiones técnicas
- `InitializeAsync` retorna `int?` para representar ausencia de registro inicial en `App`.
- Se usa `CommandDefinition` con `CancellationToken` para propagación de cancelación.
- La consulta usa `ORDER BY id_app LIMIT 1` para leer explícitamente el primer registro.

## Impacto en arquitectura
- El cambio queda encapsulado en infraestructura de datos.
- No se introduce lógica de negocio en UI.
- Se mantiene la abstracción de conexión mediante `IDbConnectionFactory`.

## Validaciones realizadas
- Compilación correcta del proyecto tras incorporar método y dependencia.
