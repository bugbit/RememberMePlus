# Coloreado condicional en MainPage para tareas vencidas o casi vencidas

## Objetivo del cambio
Aplicar resaltado visual en la lista principal únicamente a tareas vencidas o próximas a vencer para mejorar el foco del usuario.

## Alcance
- Añadir cálculo en el ViewModel de item para detectar estado vencida/casi vencida.
- Aplicar color de fondo condicional por tarea en `MainPage`.
- No alterar reglas de consulta, ordenamiento ni completado de tareas.

## Decisiones técnicas
- Se parsea `DueAt` usando formato persistido `yyyy-MM-dd HH:mm:ss`.
- Se define umbral local de “casi vencida” en 2 horas (`NearDueThreshold`).
- Colores usados:
  - Vencida: `#FEE2E2`
  - Casi vencida: `#FEF3C7`
  - Resto: transparente

## Impacto en arquitectura
- **UI (MainPage.xaml):** solo consume `HighlightColor` vía binding, sin lógica de negocio en la vista.
- **Presentación (MainPageViewModel):** encapsula la lógica de estado visual del item de tarea.
- **Dominio/Infraestructura:** sin cambios ni nuevas dependencias.

## Validaciones realizadas
- Compilación de solución para validar XAML y C# (`dotnet build`).
- Revisión de reglas de capas: no se añadieron dependencias cruzadas fuera de presentación/UI.
