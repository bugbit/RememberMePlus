# Corrección de error de contexto `DueAt` en MainPageViewModel

## Objetivo del cambio
Resolver el error de compilación: `El nombre 'DueAt' no existe en el contexto actual`.

## Alcance
- Eliminar método `TryParseDueAt` que quedó accidentalmente en `MainPageViewModel`.
- Mantener la lógica de coloreado únicamente dentro de `PendingTaskItemViewModel`.

## Decisiones técnicas
- El método de parseo de vencimiento pertenece al item view model (`PendingTaskItemViewModel`) porque depende del estado del item (`DueAt`).
- `MainPageViewModel` conserva solo responsabilidades de orquestación y carga/completado de tareas.

## Impacto en arquitectura
- **Presentación:** se refuerza separación de responsabilidades entre view model de página y view model de item.
- **Dominio/Infraestructura:** sin impacto.

## Validaciones realizadas
- Revisión estática del archivo para confirmar que no quedan referencias inválidas a `DueAt` fuera del item view model.
- Intento de compilación no ejecutable en entorno por falta de SDK `dotnet`.
