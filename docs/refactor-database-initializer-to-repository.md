# refactor-database-initializer-to-repository

## Objetivo del cambio
Refactorizar la lectura de versión de base de datos para que `DatabaseInitializer` use un repositorio en lugar de ejecutar SQL directo.

## Alcance
- Se creó `AppRepository` para consultar el primer registro de la tabla `App`.
- Se creó `AppRecord` como DTO inmutable para mapear el contenido de `App`.
- `DatabaseInitializer` ahora delega la lectura en `AppRepository`.
- Se registró `AppRepository` en DI.

## Decisiones técnicas
- Se mantiene Dapper en infraestructura para consultas SQL ligeras.
- `AppRepository.GetFirstAsync` usa `ORDER BY id_app LIMIT 1` para leer explícitamente el primer registro.
- `DatabaseInitializer.InitializeAsync` retorna `app?.Version` para cumplir el alcance actual.

## Impacto en arquitectura
- Se mejora separación de responsabilidades en infraestructura.
- `DatabaseInitializer` deja de tener acceso SQL directo.
- No se introduce lógica de negocio en UI.

## Validaciones realizadas
- Compilación correcta tras refactor y registro DI.
