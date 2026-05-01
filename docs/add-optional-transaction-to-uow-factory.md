# Transacción opcional en la creación de UnitOfWork

## Objetivo del cambio
Permitir crear `IUnitOfWork` sin transacción cuando el caso de uso solo requiere una consulta o comando SQL aislado.

## Alcance
- Actualización del contrato `IUnitOfWorkFactory`.
- Ajustes en `UnitOfWorkFactory` y `UnitOfWork` para soportar transacción opcional.
- Ajuste de `MainPageViewModel.LoadAsync` para lectura sin transacción.

## Decisiones técnicas
- Se añadió `bool useTransaction = true` en `CreateAsync` para mantener compatibilidad y comportamiento por defecto.
- `UnitOfWork` mantiene la conexión abierta siempre y solo inicia transacción cuando `useTransaction` es `true`.
- `CommitAsync` y `RollbackAsync` se comportan como no-op cuando no hay transacción, evitando condicionales en consumidores.

## Impacto en arquitectura
- Se mantiene separación de capas: UI depende de abstracciones (`IUnitOfWorkFactory`), infraestructura implementa detalles de conexión/transacción.
- No se introduce lógica de negocio en la UI ni acoplamiento adicional a SQLite/MAUI en capas internas.

## Validaciones realizadas
- Compilación de la solución para verificar firmas, llamadas y nulabilidad tras el cambio.
