# Ajuste de MainPage: tareas pendientes de hoy o vencidas

## Objetivo del cambio
Mostrar en la pantalla principal las tareas activas pendientes cuya fecha sea hoy o anterior (vencidas).

## Alcance
- Actualizar el encabezado de `MainPage` para comunicar el nuevo criterio de listado.
- Ajustar el contrato del repositorio para reflejar el caso de uso “hoy o vencidas”.
- Modificar la consulta SQLite en infraestructura para filtrar tareas activas con fecha menor o igual a hoy.
- Actualizar versión y changelog.

## Decisiones técnicas
- La regla de filtrado se mantiene en infraestructura para evitar lógica de negocio en UI.
- Se usa `date('now', 'localtime')` para respetar la fecha local del dispositivo.
- Se conserva la acción de completado con check y el flujo MVVM existente.

## Impacto en arquitectura
- **UI**: solo presentación del nuevo alcance funcional.
- **Aplicación (ViewModel)**: consume el nuevo método del repositorio sin incorporar reglas de fecha.
- **Infraestructura**: concentra la consulta “pendientes de hoy o vencidas”.
- **Dominio**: sin acoplamiento a MAUI o SQLite.

## Validaciones realizadas
- Revisión de consistencia entre interfaz e implementación del repositorio.
- Revisión del binding de `MainPage` y carga en `MainPageViewModel`.
- Verificación de actualización de versión y `CHANGELOG.md`.
