# Add non-recurring task creation

## Objetivo del cambio
Habilitar provisionalmente el alta de tareas no recurrentes desde la pantalla `Añadir`.

## Alcance
- UI de `AddTaskPage` con formulario mínimo (título + fecha + hora).
- Nuevo flujo en `AddTaskPageViewModel` para guardar tareas.
- Nuevo método de repositorio para insertar tareas no recurrentes (`id_taskscheduler` e `id_task_event` en `NULL`).

## Decisiones técnicas
- Se usa MVVM para mantener la lógica de creación fuera de la vista.
- La inserción se implementa en `TaskRepository` mediante `ITaskRepository` para respetar abstracciones.
- Se guarda `date_due_at` con formato `yyyy-MM-dd HH:mm:ss` para alinear con consultas actuales por fecha local.

## Impacto en arquitectura
- UI mantiene responsabilidad de presentación.
- Aplicación/ViewModel orquesta caso de uso de alta.
- Infraestructura encapsula SQL de persistencia.

## Validaciones realizadas
- Compilación de solución con `dotnet build RememberMePlus.slnx`.
