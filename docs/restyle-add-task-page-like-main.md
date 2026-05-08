# Restyle Add Task Page Like Main

## Objetivo

Alinear la pantalla `Añadir tarea` con el estilo visual de la pantalla principal de RememberMe+.

## Alcance

- Actualización visual de `AddTaskPage.xaml`.
- Conservación del flujo existente de alta de tareas no recurrentes.
- Actualización de versión del proyecto a `1.4.1` con `ApplicationVersion` `24`.
- Registro del cambio en `CHANGELOG.md`.

## Decisiones técnicas

- Se reutiliza la misma paleta visual de la pantalla principal: fondo cálido, cabecera granate, tarjetas redondeadas y colores compatibles con tema claro/oscuro.
- La UI mantiene bindings existentes (`Title`, `DueDate`, `DueTime`, `SaveCommand`, `IsSaving`) para no cambiar la lógica de aplicación.
- Los campos se agrupan en una tarjeta de formulario para mejorar jerarquía visual y consistencia con las tarjetas de la pantalla principal.

## Impacto en arquitectura

- El cambio queda limitado a la capa UI de .NET MAUI.
- No se modifica dominio, persistencia, repositorios ni casos de uso.
- La lógica de negocio permanece en el `ViewModel`; la vista solo define presentación y bindings MVVM.

## Validaciones realizadas

- Se comprobó que el cambio mantiene separación de capas y no introduce dependencias nuevas.
- Se comprobó que el cambio respeta MVVM al conservar comandos y propiedades enlazadas.
- Se comprobó que no se añade lógica de negocio en XAML ni code-behind.
