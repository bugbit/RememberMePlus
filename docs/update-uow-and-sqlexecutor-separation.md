# Ajuste de separación entre IUnitOfWork e ISqlExecutor

## Objetivo del cambio
Separar el contrato transaccional (`IUnitOfWork`) del contrato de ejecución SQL (`ISqlExecutor`) y mantener el executor en la implementación concreta `UnitOfWork`.

## Alcance
- `IUnitOfWork` ya no hereda de `ISqlExecutor`.
- `UnitOfWork` implementa ambos contratos.
- Repositorios hacen cast seguro a `ISqlExecutor` para ejecutar SQL.

## Decisiones técnicas
- Se mantiene encapsulación interna de conexión/transacción en `UnitOfWork`.
- Se usa verificación por patrón (`unitOfWork is ISqlExecutor sqlExecutor`) para evitar acoplar el contrato transaccional a ejecución SQL.

## Impacto en arquitectura
- Refuerza segregación de interfaces (ISP) separando responsabilidades por contrato.
- Mantiene repositorios dependientes de abstracciones y sin acceso directo a `DbConnection`/`DbTransaction`.

## Validaciones realizadas
- Revisión estática de compilación lógica en contratos y repositorios.
- Confirmación de que repositorios no acceden a `Connection`/`Transaction`.
