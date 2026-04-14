# add-database-connection-factory

## Objetivo del cambio
Crear la base de infraestructura para acceso a base de datos con una factoría de conexión y un inicializador preparado para evolución futura.

## Alcance
- Se agregó el contrato `IDbConnectionFactory`.
- Se agregó la implementación `DbConnectionFactory`.
- Se agregó `DatabaseInitializer` vacío con inyección de `IDbConnectionFactory`.
- Se registraron los servicios en DI desde `MauiProgram`.

## Decisiones técnicas
- La factoría expone `GetDatabasePath()` para centralizar la ubicación del archivo SQLite.
- `DatabaseInitializer` queda intencionalmente mínimo para permitir incorporar inicialización/migraciones luego sin acoplar UI.

## Impacto en arquitectura
- Mantiene separación de responsabilidades en infraestructura de datos.
- No introduce lógica de negocio en UI.
- Se conserva la posibilidad de reemplazar implementación vía contrato.

## Validaciones realizadas
- Compilación del proyecto tras registrar servicios y crear nuevas clases.
