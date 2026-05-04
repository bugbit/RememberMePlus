# Restaurar `TryParseDueAt` en PendingTaskItemViewModel

## Objetivo del cambio
Corregir la regresión introducida al retirar el método de parseo requerido por `IsOverdue` e `IsNearDue`.

## Alcance
- Reincorporar `TryParseDueAt` en `PendingTaskItemViewModel`.
- Mantener eliminado el método sobrante en `MainPageViewModel`.

## Decisiones técnicas
- La evaluación temporal del item se conserva encapsulada en el item view model para mantener cohesión y SRP.

## Impacto en arquitectura
- Sin impacto en dominio o infraestructura.
- Presentación mantiene separación entre orquestación de página y estado visual del item.

## Validaciones realizadas
- Revisión estática de referencias a `TryParseDueAt` y `DueAt` en `MainPageViewModel.cs`.
- Intento de compilación no ejecutable por ausencia de SDK `dotnet` en el entorno.
