# Corrección de dependencia de `is_insistent` en pantalla principal

## Objetivo del cambio

Eliminar la dependencia del campo `is_insistent` porque la tabla `Task` objetivo no expone esa columna.

## Alcance

- Se quita `is_insistent` de las consultas SQL de tareas.
- Se quita `is_insistent` de la inserción de tareas no recurrentes.
- Se elimina la propagación de `IsInsistent`/`IsImportant` en records, DTOs y ViewModels.
- Se ajusta la tarjeta visual para no mostrar prioridad derivada de un campo inexistente.
- Se sincroniza la documentación y scripts de esquema locales para no declarar `is_insistent`.

## Decisiones técnicas

- La clasificación del dashboard sigue basada en `date_due_at`, `now` y `App.relative_offset_minutes`.
- La ordenación queda por `date_due_at` y `title`, evitando campos no persistidos.
- La prioridad visual por importancia queda fuera de alcance hasta que exista un campo real y migrado en la tabla `Task`.

## Impacto en arquitectura

- La capa de aplicación conserva la clasificación de vencidas y próximas sin depender de detalles inexistentes de persistencia.
- La infraestructura evita SQL contra columnas no disponibles.
- La UI sigue consumiendo DTOs preparados y no contiene lógica de negocio.

## Validaciones realizadas

- Clean Architecture: la corrección mantiene la clasificación en aplicación y el acceso SQL en infraestructura.
- DDD: no se inventa una regla de importancia sin persistencia real.
- SOLID: se elimina acoplamiento a un campo inexistente y se mantienen contratos simples.
- Stack permitido: cambio limitado a .NET 10, C#, .NET MAUI, SQLite y Dapper.
