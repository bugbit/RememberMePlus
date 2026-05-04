# Fix non-recurring task notify date

## Objetivo del cambio
Asegurar que, al crear una tarea no recurrente, el campo `datetime_notify_at` se persista con la misma fecha y hora que `date_due_at`.

## Alcance
- Ajuste del `INSERT` de alta de tareas no recurrentes en `TaskRepository`.
- Sin cambios en dominio, UI ni contratos públicos.

## Decisiones técnicas
- Reutilizar el mismo parámetro `@DateDueAt` para `date_due_at` y `datetime_notify_at`.
- Mantener formato persistido `yyyy-MM-dd HH:mm:ss` ya existente.

## Impacto en arquitectura
- Se mantiene la separación de capas: el cambio queda en infraestructura de datos.
- No se introduce lógica de negocio en UI.
- No se acoplan capas internas a detalles externos.

## Validaciones realizadas
- Revisión del SQL de inserción en `TaskRepository` para confirmar que `datetime_notify_at = @DateDueAt`.
- Verificación de compilación del proyecto MAUI.
