# Implementación de pantalla principal attention-first

## Objetivo del cambio

Implementar en la pantalla principal de RememberMe+ el diseño attention-first para hacer muy visibles las tareas vencidas y las tareas próximas a vencer según `App.relative_offset_minutes`.

## Alcance

- Se reemplaza la lista simple de pendientes por dos secciones visuales: `tareas vencidas` y `a punto de vencer`.
- Se añaden tarjetas grandes con iconografía, tiempo relativo, fecha absoluta y acciones táctiles grandes.
- Se añade actualización automática cada minuto mientras la pantalla está visible.
- Se añade pulso visual suave en la sección de vencidas para captar atención sin parpadeo agresivo.
- Se añade consulta de tareas activas hasta el final de la ventana de próximo vencimiento.
- Se añade servicio de aplicación para preparar el dashboard sin delegar la clasificación a la vista.
- Se añaden recursos de color semánticos para modo claro y oscuro.

## Decisiones técnicas

- `HomeDashboardService` centraliza la lectura de configuración, la consulta de tareas activas y la clasificación entre vencidas y próximas.
- `MainPageViewModel` mantiene solo estado de presentación, formato de textos y comandos de UI.
- `IAppClock` encapsula el reloj local para evitar acoplar los casos de uso directamente a `DateTime.Now`.
- `TaskRepository.GetActiveDueBeforeAsync` consulta SQLite con Dapper hasta `now + relative_offset_minutes`.
- `SnoozeAsync` actualiza `date_due_at`, `date_due_at_last` y `datetime_notify_at` con la nueva fecha local pospuesta.
- `Ignorar` es temporal en sesión de pantalla y oculta la tarjeta sin modificar la tarea en base de datos.
- El pulso visual se limita al contenedor de vencidas y se detiene al salir de la pantalla.

## Impacto en arquitectura

- La UI no decide si una tarea está vencida o próxima; consume grupos ya calculados por aplicación.
- Infraestructura mantiene los detalles SQLite/Dapper en `TaskRepository`.
- Los servicios nuevos (`IHomeDashboardService`, `IAppClock`) se registran por interfaz en DI.
- El dominio sigue desacoplado de MAUI y SQLite; no se introducen dependencias externas ni servicios remotos.

## Validaciones realizadas

- Clean Architecture: separación entre consulta/clasificación de aplicación, repositorio de infraestructura y renderizado MAUI.
- DDD: la urgencia se modela como estado funcional calculado, no como detalle visual aislado.
- SOLID: servicios y repositorios se consumen por abstracción y mantienen responsabilidades diferenciadas.
- Stack permitido: .NET 10, C#, .NET MAUI, Android, MVVM, SQLite y Dapper.
- Accesibilidad: tarjetas y botones grandes, alto contraste, icono más texto y soporte de modo oscuro.

## Corrección de compatibilidad de esquema

La implementación no depende de `is_insistent` porque la tabla `Task` objetivo no expone ese campo. La ordenación actual usa únicamente `date_due_at` y `title`, manteniendo la clasificación funcional por vencidas y próximas.
