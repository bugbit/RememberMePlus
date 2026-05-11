# Add Infrastructure/Data Dapper separation

## Objetivo del cambio

Separar la persistencia local en una capa `Infrastructure/Data` clara, diferenciando contratos de aplicación, abstracciones de infraestructura, clases específicas de Dapper, modelos de datos y mapeadores.

## Alcance

- Nuevo contrato `IReminderTaskRepository` en `Application/Tasks`.
- Nuevas abstracciones de infraestructura `IDbConnectionFactory` e `IDatabaseInitializer`.
- Nueva configuración `SqliteDataOptions`.
- Nueva implementación SQLite/Dapper en `Infrastructure/Data/Dapper`.
- Nuevo modelo de datos `ReminderTaskDataModel` separado del agregado de dominio.
- Nuevo mapper `ReminderTaskDataMapper` para convertir entre dominio y persistencia.
- Nueva implementación `DapperReminderTaskRepository`.
- Registro de infraestructura desde `MauiProgram` usando la ruta local de datos de MAUI.

## Decisiones técnicas

- Las interfaces de caso de uso se colocan en `Application` para que la aplicación dependa de abstracciones.
- Las interfaces puramente técnicas de datos se colocan en `Infrastructure/Data/Abstractions` porque describen detalles de infraestructura.
- Las clases que usan Dapper quedan aisladas bajo `Infrastructure/Data/Dapper`.
- El dominio no recibe atributos SQLite ni dependencias de Dapper.
- Las fechas se almacenan como texto ISO 8601 UTC para conservar orden lexicográfico y portabilidad en SQLite.
- La inicialización de esquema se encapsula en `IDatabaseInitializer` para evitar mezclar creación de tablas con UI o dominio.

## Impacto en arquitectura

- `Domain` sigue independiente de MAUI, SQLite, Dapper e infraestructura.
- `Application` define el contrato de persistencia sin conocer Dapper ni SQLite.
- `Infrastructure` implementa los contratos internos y contiene los detalles de acceso a datos.
- `Presentation` no accede directamente a Dapper ni a SQL.
- `MauiProgram` actúa como composition root y conecta la implementación concreta con las abstracciones.

## Validaciones realizadas

- Se comprobó que las clases con `using Dapper` estén bajo `Infrastructure/Data/Dapper`.
- Se comprobó que `Domain` no contiene referencias a MAUI, SQLite, Dapper, Android, Infrastructure ni Presentation.
- Se comprobó que `Application` no contiene referencias a Dapper, SQLite, MAUI ni Infrastructure.
- Se actualizó la versión del proyecto a `1.0.3` y `ApplicationVersion` a `4`.
- Se registró el cambio en `CHANGELOG.md` siguiendo Keep a Changelog.
