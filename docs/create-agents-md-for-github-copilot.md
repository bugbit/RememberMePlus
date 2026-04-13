# Crear AGENTS.md para GitHub Copilot

## Objetivo

Definir una guía operativa para asistentes tipo GitHub Copilot alineada con las reglas del proyecto RememberMe+.

## Alcance

Se ha creado un archivo `AGENTS.md` con instrucciones sobre:

- rol esperado del asistente
- stack permitido
- restricciones de Clean Architecture + DDD
- cumplimiento de SOLID
- reglas de generación de código en .NET 10 / .NET MAUI
- obligación de documentar cambios en `/docs`
- obligación de versionado
- obligación de actualizar `CHANGELOG.md`
- uso correcto de constructores primarios en C# 12

## Decisiones técnicas

- Se ha mantenido el documento en formato Markdown simple para máxima compatibilidad con repositorios GitHub.
- Se ha priorizado redacción breve, normativa y accionable.
- Se ha incorporado una checklist final para facilitar validaciones antes de cerrar cambios.
- Se ha incluido un ejemplo correcto e incorrecto de uso de constructores primarios para evitar ambigüedad.

## Impacto en arquitectura

No introduce cambios funcionales en la aplicación.

Su impacto es de gobernanza técnica:

- refuerza la consistencia arquitectónica
- reduce riesgo de mezclar capas
- establece criterios comunes para futuras contribuciones asistidas

## Validaciones realizadas

- Alineado con el README del proyecto.
- Compatible con el stack declarado: .NET 10, .NET MAUI, Android, SQLite, MVVM y localización.
- Incluye restricciones explícitas para Clean Architecture, DDD y SOLID.
- Incluye la regla solicitada para constructores primarios.

## Observación

Este entregable crea la guía y su documentación asociada. La actualización efectiva de la versión del proyecto y de `CHANGELOG.md` debe aplicarse dentro del repositorio real donde existan esos archivos.
