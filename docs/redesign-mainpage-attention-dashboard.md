# Rediseño de pantalla principal orientada a atención

## Objetivo del cambio

Rediseñar la pantalla principal para que las personas despistadas o con problemas de atención identifiquen de forma inmediata:

1. tareas vencidas;
2. tareas dentro de la ventana de próximo vencimiento configurada por `App.relative_offset_minutes`.

## Alcance

- Se separan las tareas en dos secciones visuales:
  - **Tareas vencidas**, con prioridad máxima, tono rojo y tarjetas grandes.
  - **A punto de vencer**, con tono naranja y contador de tiempo restante.
- Se añade resumen superior con contadores de tareas vencidas y próximas.
- Se añaden acciones rápidas por tarjeta:
  - completar;
  - posponer usando `App.snooze_minutes`;
  - ignorar temporalmente en la sesión visual actual.
- Se añade actualización automática cada minuto para mover tareas entre secciones cuando cambie su estado temporal.
- Se añade pulso visual suave para reforzar la atención en elementos críticos.

## Decisiones técnicas

- La consulta de la pantalla principal recibe un límite temporal calculado como `DateTime.Now + relative_offset_minutes`.
- `MainPageViewModel` obtiene la configuración desde `IAppRepository` y orquesta la clasificación de elementos sin introducir lógica de negocio en la vista.
- `TaskRepository` expone `GetHomeAttentionTasksAsync` para recuperar solo tareas activas relevantes para la pantalla principal.
- `TaskRepository.SnoozeAsync` centraliza la actualización de `date_due_at` y `datetime_notify_at` para mantener consistencia de persistencia local.
- La vista MAUI usa `RefreshView`, `ScrollView`, `Border` y `BindableLayout` para un diseño simple, legible y reusable en Android.

## Impacto en arquitectura

- La UI mantiene responsabilidad de presentación y animación visual.
- El ViewModel mantiene la orquestación MVVM: carga, clasificación temporal, comandos y estado observable.
- La infraestructura mantiene el acceso SQLite y las actualizaciones de persistencia mediante `ITaskRepository`.
- El dominio no recibe dependencias de MAUI, SQLite ni detalles de infraestructura.

## Validaciones realizadas

- Clean Architecture: la pantalla consume abstracciones existentes de repositorio y unidad de trabajo.
- DDD: la clasificación vencida/próxima se mantiene alineada con el concepto funcional de vencimiento y ventana de atención.
- SOLID: se añadieron métodos específicos al repositorio para evitar sobrecargar la vista con SQL o detalles de persistencia.
- Stack permitido: cambios limitados a .NET 10, C#, .NET MAUI, MVVM y SQLite/Dapper.
- UX: se priorizan tipografía grande, alto contraste, tarjetas amplias, iconografía de alerta y acciones táctiles grandes.
- Versionado: se incrementó la aplicación a `1.4.0` con `ApplicationVersion` 23.
- CHANGELOG: se documentó el cambio en formato Keep a Changelog.
