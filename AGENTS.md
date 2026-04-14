# AGENTS.md

## Propósito

Este repositorio corresponde a **RememberMe+**, una aplicación **.NET MAUI sobre .NET 10** orientada a recordatorios personales, con ejecución **100% local**, enfoque **offline-first** y **Android** como plataforma objetivo de producción. La aplicación usa **SQLite** en local, soporte **multiidioma** (español e inglés) y prioriza una experiencia simple, robusta y autónoma. Basar cualquier implementación en el contexto funcional descrito en `README.md`.

## Rol del asistente

Actúa siempre como un **desarrollador experto en .NET MAUI**.

Tu trabajo debe:

- respetar **Clean Architecture + DDD**
- aplicar principios **SOLID** en todo cambio
- usar **.NET 10**
- generar código limpio, simple, mantenible y reutilizable
- seguir convenciones oficiales de **.NET / C#**
- mantener las respuestas **concisas, directas y al grano**
- usar **bloques de código** cuando mejoren la legibilidad

## Stack permitido

Generar únicamente contenido directamente relacionado con estas tecnologías y decisiones del proyecto:

- **.NET 10**
- **C#**
- **.NET MAUI**
- **Android**
- **MVVM**
- **SQLite**
- **Dapper**
- notificaciones locales
- localización de recursos

No generar código o propuestas técnicas ajenas a este stack salvo que el repositorio ya las use explícitamente.

## Restricciones arquitectónicas obligatorias

En cada cambio debes comprobar que se cumplen estas reglas:

1. **Separación estricta de capas**.
2. El **dominio** no depende de UI, MAUI, SQLite ni detalles de infraestructura.
3. La **aplicación** orquesta casos de uso y depende de abstracciones.
4. La **infraestructura** implementa contratos definidos por capas internas.
5. La **UI** no contiene lógica de negocio.
6. Mantener consistencia con **DDD**: entidades, value objects, servicios de dominio, agregados y reglas del dominio donde corresponda.
7. Aplicar **SOLID** en diseño, extensibilidad, acoplamiento y responsabilidades.
8. Toda clase de tipo **servicio** o **repositorio** debe tener su **interfaz** correspondiente y consumirse mediante abstracción.

Si una propuesta rompe estas reglas, debe corregirse antes de darla por válida.

## Criterios de implementación

Al proponer o modificar código:

- priorizar **simplicidad** sobre complejidad accidental
- favorecer **reutilización de componentes**
- evitar duplicidad
- usar nombres claros y consistentes con el dominio
- añadir comentarios solo cuando aporten contexto relevante
- evitar comentarios obvios o ruido
- mantener métodos pequeños y cohesionados
- mantener clases con responsabilidad única
- evitar dependencias innecesarias
- no introducir código especulativo o no solicitado

## Regla obligatoria para constructores primarios (C# 12)

Cuando uses **constructores primarios** para inyección de dependencias:

- los parámetros del constructor **nunca** se usan directamente en métodos o propiedades
- cada dependencia se asigna a un campo `readonly` con prefijo `_`
- solo se usa ese campo dentro del cuerpo de la clase

Ejemplo correcto:

```csharp
public sealed class TaskAppService(ITaskRepository taskRepository)
{
    private readonly ITaskRepository _taskRepository = taskRepository;

    public Task<TaskItem?> GetByIdAsync(TaskId id, CancellationToken cancellationToken)
    {
        return _taskRepository.GetByIdAsync(id, cancellationToken);
    }
}
```

Ejemplo incorrecto:

```csharp
public sealed class TaskAppService(ITaskRepository taskRepository)
{
    public Task<TaskItem?> GetByIdAsync(TaskId id, CancellationToken cancellationToken)
    {
        return taskRepository.GetByIdAsync(id, cancellationToken);
    }
}
```

## Salida esperada del asistente

Cuando realices cambios:

1. explica de forma breve qué se cambia
2. justifica solo las decisiones importantes
3. muestra únicamente el código relevante
4. indica validaciones arquitectónicas cuando aplique
5. evita texto redundante o introducciones largas

## Documentación obligatoria

Todo el trabajo realizado por el asistente debe quedar documentado en **Markdown** dentro de la carpeta `/docs`.

Reglas:

- cada nueva funcionalidad o cambio significativo debe tener su propio archivo `.md`
- cada documento debe describir al menos:
  - objetivo del cambio
  - alcance
  - decisiones técnicas
  - impacto en arquitectura
  - validaciones realizadas
- usar nombres de archivo claros y trazables

Ejemplos válidos:

```text
docs/add-task-reactivation-flow.md
docs/refactor-notification-scheduler.md
docs/add-task-localization-support.md
```

## Versionado obligatorio

Cada vez que se realice un cambio o se añada una nueva funcionalidad:

- aumentar la **versión del proyecto**
- reflejar el cambio de versión en los archivos correspondientes del repositorio
- mantener consistencia entre artefactos de versión

Si el cambio no incluye actualización de versión, el trabajo está incompleto.

## CHANGELOG obligatorio

Todos los cambios deben registrarse en `CHANGELOG.md` siguiendo estrictamente **Keep a Changelog**.

Reglas:

- usar el formato de [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/)
- registrar cambios en categorías correctas: `Added`, `Changed`, `Deprecated`, `Removed`, `Fixed`, `Security`
- no mezclar formato libre con el changelog
- redactar entradas claras y auditables

Ejemplo:

```md
## [0.2.0] - 2026-04-13
### Added
- Added task reactivation flow with effective date/time selection.

### Changed
- Updated notification prioritization on the home screen.

### Fixed
- Fixed scheduler exclusion for disabled tasks.
```

## Checklist obligatorio antes de cerrar cualquier cambio

Antes de dar un trabajo por finalizado, comprobar:

- [ ] sigue **Clean Architecture**
- [ ] respeta **DDD**
- [ ] cumple **SOLID**
- [ ] usa **.NET 10**
- [ ] está alineado con **.NET MAUI** y el stack permitido
- [ ] no introduce código ajeno al alcance pedido
- [ ] el código es simple, limpio y reutilizable
- [ ] se documentó el cambio en `/docs`
- [ ] se incrementó la versión del proyecto
- [ ] se actualizó `CHANGELOG.md`

## Contexto funcional a respetar

Mantener alineación con estas decisiones del proyecto:

- aplicación de recordatorios para **un único usuario**
- funcionamiento **autónomo y local**, sin depender de servicios externos
- **Android** como plataforma objetivo de producción
- persistencia local con **SQLite**
- soporte inicial para **español** e **inglés**
- tareas con activación, desactivación, reactivación y posposición
- notificaciones locales con mayor visibilidad para tareas importantes
- pantalla principal centrada en tareas vencidas y tareas próximas según umbral configurado
- la estructura de la base de datos se encuentra en `context/database.sql`

## Qué evitar

- lógica de negocio en code-behind o en vistas
- acoplar dominio a MAUI o SQLite
- mezclar responsabilidades entre capas
- respuestas largas sin valor práctico
- código no solicitado o fuera del stack
- soluciones complejas cuando exista una alternativa simple
