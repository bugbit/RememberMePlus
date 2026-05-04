# Ajuste de colores actuales para tareas urgentes en MainPage

## Objetivo del cambio
Aplicar los colores actuales de la funcionalidad de urgencia sobre el texto de tareas en la pantalla principal.

## Alcance
- Actualizar la propiedad `TaskTextColor` en `PendingTaskItemViewModel`.

## Decisiones técnicas
- Vencidas: `#B91C1C` (rojo).
- Casi vencidas: `#C2410C` (naranja).
- Resto: `Colors.Black`.

## Impacto en arquitectura
- Cambio limitado a presentación (ViewModel de item).
- Sin impacto en dominio e infraestructura.

## Validaciones realizadas
- Revisión estática de color resultante en `TaskTextColor` y binding en `MainPage.xaml`.
- Build no ejecutable por falta de `dotnet` en el entorno.
