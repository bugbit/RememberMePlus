# Add initial reminder domain model

## Objetivo del cambio

Crear la primera capa de dominio explícita del proyecto para que la solución no quede limitada a una reorganización de UI y empiece a reflejar las reglas centrales de RememberMe+.

## Alcance

- Nueva carpeta `Domain/Tasks`.
- Nuevo agregado `ReminderTask`.
- Nuevos value objects `ReminderTaskId`, `ReminderTitle` y `PostponeMinutes`.
- Nuevo enum de dominio `ReminderPriority`.
- Reglas básicas de activación, desactivación, reactivación, posposición, completado y vencimiento.

## Decisiones técnicas

- El dominio se mantiene dentro del proyecto MAUI actual como carpeta lógica porque la solución todavía contiene un único `.csproj`.
- Las clases del dominio no dependen de MAUI, SQLite, UI, plataformas móviles ni servicios externos.
- `ReminderTask` concentra invariantes del recordatorio para evitar que la UI implemente reglas de negocio.
- `ReminderTitle` normaliza y valida el título para evitar recordatorios sin nombre o con nombres excesivos.
- `PostponeMinutes` impide valores cero o negativos para posposición.

## Impacto en arquitectura

- Se añade una capa `Domain` independiente de presentación e infraestructura.
- La UI puede evolucionar para consumir casos de uso de aplicación sin conocer detalles de persistencia.
- La infraestructura futura de SQLite deberá mapear estas clases sin introducir atributos o dependencias SQLite en dominio.
- La lógica de negocio queda preparada para moverse o exponerse mediante servicios de aplicación cuando existan casos de uso.

## Validaciones realizadas

- Se comprobó que no existían clases de dominio antes del cambio.
- Se comprobó que `Domain/Tasks` no contiene referencias a `Microsoft.Maui`, `SQLite`, `Android` ni namespaces de presentación.
- Se mantuvo el dominio sin dependencias hacia UI o infraestructura.
- Se actualizó la versión del proyecto a `1.0.2` y `ApplicationVersion` a `3`.
- Se registró el cambio en `CHANGELOG.md` siguiendo Keep a Changelog.
