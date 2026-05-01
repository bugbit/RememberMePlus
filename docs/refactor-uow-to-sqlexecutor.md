# Refactor UoW a ISqlExecutor

## Objetivo del cambio
Eliminar el acceso directo a `DbConnection` y `DbTransaction` desde repositorios, encapsulando la ejecución SQL en `IUnitOfWork` mediante `ISqlExecutor`.

## Alcance
- Nuevo contrato `ISqlExecutor`.
- `IUnitOfWork` ahora hereda de `ISqlExecutor` y elimina exposición de conexión/transacción.
- `UnitOfWork` implementa `QueryAsync<T>`, `QueryFirstOrDefaultAsync<T>` y `ExecuteAsync`.
- Repositorios de infraestructura migrados para usar solo métodos del ejecutor SQL.

## Decisiones técnicas
- Se mantiene el patrón Unit of Work para commit/rollback explícito.
- Se reutiliza `CommandDefinition` de Dapper dentro de `UnitOfWork` para propagar cancelación y transacción activa.
- No se cambia la firma pública de repositorios respecto al parámetro `IUnitOfWork`, minimizando impacto.

## Impacto en arquitectura
- Mejora encapsulación en capa de infraestructura.
- Refuerza separación de responsabilidades: repositorios expresan intención SQL sin depender de detalles de conexión/transacción.
- Mantiene coherencia con Clean Architecture y SOLID (inversión de dependencias y SRP).

## Validaciones realizadas
- Compilación de la solución MAUI para validar contratos y uso en repositorios.
- Revisión de que ningún repositorio use `Connection`/`Transaction` directamente.
