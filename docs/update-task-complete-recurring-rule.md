# update-task-complete-recurring-rule

## Objetivo del cambio
Ajustar la lógica de completado en `TaskRepository.CompleteAsync` para preservar la fecha previa de vencimiento, mover el vencimiento a la fecha local actual y limitar el cambio de `is_active` a tareas con recurrencia.

## Alcance
- Se actualizó el `UPDATE` de `TaskRepository.CompleteAsync`.
- Se actualizó versionado de la app.
- Se registró el cambio en `CHANGELOG.md`.

## Decisiones técnicas
- `date_due_at_last` conserva el valor anterior de `date_due_at`.
- `date_due_at` se establece con `date('now', 'localtime')` para usar la fecha local del dispositivo.
- `is_active` se actualiza de forma condicional con `CASE` cuando existe `id_taskscheduler` o `id_task_event` (criterio de recurrencia).

## Impacto en arquitectura
El cambio queda encapsulado en infraestructura (`TaskRepository`) y no introduce lógica de negocio en UI ni acoplamientos cruzados, manteniendo la separación de capas.

## Validaciones realizadas
- Revisión estática del SQL de actualización en repositorio.
- Verificación de versionado y registro en changelog.
