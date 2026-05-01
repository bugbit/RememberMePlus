# Corrección de argumentos en CreateAsync de IUnitOfWorkFactory

## Objetivo del cambio
Corregir el error de compilación generado tras introducir el parámetro `useTransaction` en `CreateAsync`.

## Alcance
- Ajuste de `DatabaseInitializer` para invocar `CreateAsync` con argumento nombrado `cancellationToken`.

## Decisiones técnicas
- Se mantuvo el orden de parámetros de la firma (`useTransaction`, `cancellationToken`) y se corrigió el punto de uso afectado con argumento nombrado para preservar legibilidad y compatibilidad.

## Impacto en arquitectura
- Sin cambios de arquitectura: se mantiene la orquestación en capa de aplicación/infraestructura mediante abstracciones (`IUnitOfWorkFactory`).

## Validaciones realizadas
- Revisión estática de llamadas a `CreateAsync` para confirmar consistencia de firmas y argumentos.
