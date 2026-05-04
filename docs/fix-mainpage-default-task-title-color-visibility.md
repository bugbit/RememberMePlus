# Ajuste de visibilidad del color por defecto del título en MainPage

## Objetivo del cambio
Corregir la baja visibilidad del color negro fijo en tareas no urgentes.

## Alcance
- Cambiar el color por defecto del título a `AppThemeBinding` para soportar modo claro/oscuro.
- Mantener rojo/naranja para estados urgentes con triggers.

## Decisiones técnicas
- Se elimina `TaskTextColor` para evitar fijar un color por defecto no adaptable al tema.
- `DataTrigger` en XAML aplica:
  - `#B91C1C` cuando `IsOverdue = true`.
  - `#C2410C` cuando `IsNearDue = true`.
- Color base por tema:
  - Light: `#111827`
  - Dark: `#F9FAFB`

## Impacto en arquitectura
- Cambio limitado a capa de presentación (XAML + VM de item).
- Sin impacto en dominio e infraestructura.

## Validaciones realizadas
- Revisión estática de bindings (`IsOverdue`, `IsNearDue`) y triggers en `MainPage.xaml`.
- Build no ejecutable por ausencia de SDK `dotnet` en el entorno.
