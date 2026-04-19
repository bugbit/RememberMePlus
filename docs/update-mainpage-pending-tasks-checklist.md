# Actualización de vista principal para tareas pendientes con check

## Objetivo del cambio
Mostrar en la vista principal las tareas pendientes activas y permitir marcarlas como completadas desde un check.

## Alcance
- Se reemplaza el bloque informativo de `MainPage` por una lista de tareas pendientes.
- Se agrega un `MainPageViewModel` para cargar tareas y completar tareas con comando MVVM.
- Se incorpora repositorio de tareas en infraestructura para lectura y actualización.
- Se actualiza versionado de la app y changelog.

## Decisiones técnicas
- Se aplica MVVM para evitar lógica de negocio en la vista.
- `TaskRepository` trabaja con `IUnitOfWork` para mantener transacciones centralizadas.
- Completar tarea desactiva la tarea (`is_active = 0`) y preserva `date_due_at` en `date_due_at_last`.

## Impacto en arquitectura
- **UI**: solo binding y presentación de datos.
- **Aplicación/UI ViewModel**: orquesta casos de uso de cargar/completar.
- **Infraestructura**: implementa acceso a SQLite con Dapper mediante contratos (`ITaskRepository`).
- **Dominio**: sin dependencias nuevas ni acoplamiento a MAUI.

## Validaciones realizadas
- Compilación del proyecto MAUI para Android.
- Revisión de inyección de dependencias para repositorio y view model.
- Verificación de actualización de versión y `CHANGELOG.md`.
