# Cambio de resaltado: color de fuente en tareas vencidas/casi vencidas

## Objetivo del cambio
Aplicar el estado visual de urgencia con color de fuente, no con color de fondo, en la lista principal.

## Alcance
- Quitar `BackgroundColor` por item en `MainPage`.
- Aplicar `TextColor` al título de la tarea según estado temporal.

## Decisiones técnicas
- `TaskTextColor` en `PendingTaskItemViewModel` define:
  - `Colors.Red` para tareas vencidas.
  - `Colors.Orange` para tareas casi vencidas (umbral de 2 horas).
  - `Colors.Black` para el resto.
- Se mantiene el cálculo de `IsOverdue` e `IsNearDue` en el item view model para cohesión de presentación.

## Impacto en arquitectura
- **UI:** binding simple de `TextColor` sin lógica de negocio en vista.
- **Presentación:** mantiene responsabilidad del estado visual por item.
- **Dominio/Infraestructura:** sin cambios.

## Validaciones realizadas
- Revisión estática de bindings XAML y propiedades en view model.
- Intento de compilación no ejecutable por ausencia de SDK `dotnet` en el entorno.
