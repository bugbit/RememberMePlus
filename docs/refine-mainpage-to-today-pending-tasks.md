# Ajuste de MainPage: tareas pendientes de hoy

## Objetivo del cambio
Ajustar la pantalla principal para que muestre solo las tareas pendientes correspondientes al día actual.

## Alcance
- Actualizar el texto de la UI para indicar explícitamente "de hoy".
- Cambiar la consulta de infraestructura para filtrar tareas activas por fecha local actual.
- Mantener la acción de completar con check sin cambios funcionales adicionales.
- Actualizar versión y changelog.

## Decisiones técnicas
- Se aplica el filtro en infraestructura (`TaskRepository`) para mantener la UI simple y sin reglas de negocio.
- Se usa `date('now', 'localtime')` en SQLite para alinear la selección con la fecha local del dispositivo.

## Impacto en arquitectura
- **UI**: cambio de texto y consumo de datos filtrados.
- **Aplicación (ViewModel)**: mantiene orquestación, sin lógica de fecha.
- **Infraestructura**: concentra la regla de consulta para “pendientes de hoy”.
- **Dominio**: sin acoplamiento a MAUI o SQLite.

## Validaciones realizadas
- Revisión de contrato `ITaskRepository` y su implementación.
- Revisión de binding y flujo de carga en `MainPageViewModel`.
- Verificación de actualización de versión y `CHANGELOG.md`.
