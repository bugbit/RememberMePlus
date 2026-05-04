# Eliminación final de `TryParseDueAt` duplicado en MainPageViewModel

## Objetivo del cambio
Resolver de forma definitiva el error `El nombre 'DueAt' no existe en el contexto actual`.

## Alcance
- Retirar el método duplicado `TryParseDueAt` que seguía presente en `MainPageViewModel`.
- Conservar `TryParseDueAt` exclusivamente en `PendingTaskItemViewModel`.

## Decisiones técnicas
- La propiedad `DueAt` existe solo en el view model de item; por lo tanto, su parseo debe vivir en esa clase para cumplir cohesión y SRP.

## Impacto en arquitectura
- Sin cambios en dominio o infraestructura.
- Se preserva la separación entre orquestación de pantalla y estado visual por item.

## Validaciones realizadas
- Verificación estática: una única definición de `TryParseDueAt` en `MainPageViewModel.cs`, dentro de `PendingTaskItemViewModel`.
- Intento de build no ejecutable en entorno actual por ausencia de SDK `dotnet`.
