# add-database-schema-repository

## Objetivo del cambio
Separar la responsabilidad de creación y migración del esquema de la base de datos en un repositorio dedicado.

## Alcance
- Añadido `IDatabaseSchemaRepository` con método `CreateOrUpdateDatabaseAsync`.
- Añadido `DatabaseSchemaRepository` que:
  - Consulta si existe registro en la tabla `App`.
  - Si no existe, ejecuta el script `database-v1.sql` embebido.
  - Registra logs de inicio y fin de ejecución del script.
- Añadido script `Resources/Scripts/database-v1.sql` con esquema completo: tablas `App`, `TaskScheduler`, `TaskEvent`, `TaskGroup`, `Task`, índices e inserción inicial.
- `DatabaseInitializer` delega en `IDatabaseSchemaRepository`.

## Decisiones técnicas
- El script SQL se embebe como `EmbeddedResource` (no `MauiAsset`) para poder cargarlo con `Assembly.GetManifestResourceStream`, evitando deadlocks al llamarse desde `MauiProgram` con `.GetAwaiter().GetResult()`.
- Se usan sentencias `CREATE TABLE IF NOT EXISTS` e `CREATE INDEX IF NOT EXISTS` para que el script sea idempotente.

## Impacto en arquitectura
- `DatabaseInitializer` ya no contiene lógica de esquema; delega en `IDatabaseSchemaRepository`.
- Se respeta Clean Architecture: la infraestructura gestiona internamente el esquema sin exponer detalles a capas superiores.

## Validaciones realizadas
- Compilación correcta.
- BD creada correctamente en primera ejecución.
