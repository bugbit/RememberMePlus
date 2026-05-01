# Fix compilación TaskRepository por AsList

## Objetivo del cambio
Resolver el error de compilación en `TaskRepository` causado por uso de `AsList()` sin método de extensión disponible en el contexto actual.

## Alcance
- Ajuste puntual en `TaskRepository.GetPendingTodayOrOverdueAsync`.

## Decisiones técnicas
- Se reemplaza `AsList()` por `ToList()` sobre `IEnumerable<TaskItemRecord>` para mantener compatibilidad sin depender de extensiones específicas.

## Impacto en arquitectura
- Sin cambios arquitectónicos: se mantiene Clean Architecture, DDD y separación de responsabilidades.

## Validaciones realizadas
- Revisión estática del método para garantizar retorno `IReadOnlyList<TaskItemRecord>` válido.
- Verificación de eliminación de `AsList()` en repositorio.
