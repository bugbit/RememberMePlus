# Corrección de conflicto entre namespace `Application` y tipo MAUI

## Objetivo del cambio

Corregir el error de compilación donde `Application` se resolvía como namespace (`RememberMePlusApp.Application`) en lugar de como el tipo base de MAUI.

## Alcance

- Se actualiza `App.xaml.cs` para heredar explícitamente de `Microsoft.Maui.Controls.Application`.
- Se elimina un `using` no utilizado en `App.xaml.cs`.
- Se incrementa la versión del proyecto.
- Se registra la corrección en `CHANGELOG.md`.

## Decisiones técnicas

Se mantiene el namespace de la capa de aplicación para evitar un refactor innecesario. La corrección más simple y explícita es usar el nombre completamente cualificado del tipo base MAUI en `App`.

## Impacto en arquitectura

- No cambia la separación de capas.
- No introduce dependencias nuevas.
- La UI sigue dependiendo de MAUI y la capa de aplicación mantiene sus servicios separados.

## Validaciones realizadas

- Clean Architecture: sin cambios en dependencias entre capas.
- DDD: sin impacto en reglas de dominio.
- SOLID: corrección mínima y explícita.
- Stack permitido: .NET MAUI y C#.
