# add-tabs-home-and-add-task-page

## Objetivo del cambio
Agregar navegación por pestañas para separar la pantalla principal y una nueva pantalla de alta de tareas.

## Alcance
- Se actualiza `AppShell` para usar `TabBar`.
- Se mantiene `Home` como pestaña principal.
- Se agrega la nueva pestaña `Añadir` con su página `AddTaskPage`.

## Decisiones técnicas
- Se usa `Shell` con `TabBar` por simplicidad y alineación con navegación MAUI.
- `AddTaskPage` se implementa inicialmente como pantalla base sin lógica de negocio.

## Impacto en arquitectura
- El cambio está acotado a la capa UI (`Shell` y `ContentPage`).
- No se alteran dominio, aplicación ni infraestructura.
- Se evita introducir lógica de negocio en vistas.

## Validaciones realizadas
- Compilación de solución en modo Debug.
- Revisión de rutas Shell para `MainPage` y `AddTaskPage`.
