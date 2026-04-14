# add-sqlite-create-connection

## Objetivo del cambio
Agregar la creación de conexión SQLite en la factoría de conexiones para permitir acceso directo a la base de datos local.

## Alcance
- Se amplió `IDbConnectionFactory` con `CreateConnection()`.
- Se implementó `CreateConnection()` en `DbConnectionFactory`.
- Se agregó el paquete `Microsoft.Data.Sqlite`.

## Decisiones técnicas
- `CreateConnection()` retorna `DbConnection` para desacoplar consumidores del tipo concreto.
- Se usa `SqliteConnectionStringBuilder` para construir de forma segura la cadena de conexión.
- La conexión se crea sin abrir, dejando el control de ciclo de vida al consumidor.

## Impacto en arquitectura
- Cambio encapsulado en infraestructura de datos.
- No se introduce lógica de negocio en UI.
- Se mantiene la abstracción de acceso a infraestructura mediante contrato.

## Validaciones realizadas
- Verificación de errores de compilación en archivos modificados.
