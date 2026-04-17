# add-interfaces-for-database-initializer-and-app-repository

## Objetivo del cambio
Introducir contratos para `DatabaseInitializer` y `AppRepository` para consumirlos por abstracción.

## Alcance
- Se agregó `IAppRepository`.
- Se agregó `IDatabaseInitializer`.
- `AppRepository` implementa `IAppRepository`.
- `DatabaseInitializer` implementa `IDatabaseInitializer` y depende de `IAppRepository`.
- Se actualizó DI para registrar por interfaces.

## Decisiones técnicas
- Se mantuvo la implementación de lectura de `App` en infraestructura con Dapper.
- Se aplicó inversión de dependencias en el flujo de inicialización.

## Impacto en arquitectura
- Mejor desacoplamiento entre componentes de infraestructura.
- Alineación con principios SOLID (DIP) y extensibilidad para pruebas.

## Validaciones realizadas
- Compilación correcta tras cambios en contratos, implementaciones y DI.
