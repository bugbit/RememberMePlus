# Refactor Clean Architecture presentation layer

## Objetivo del cambio

Alinear la estructura inicial del proyecto con Clean Architecture separando explícitamente las clases de presentación de la raíz de la aplicación MAUI y eliminando lógica de demostración generada por plantilla.

## Alcance

- Movimiento de `MainPage` a `Presentation/Pages`.
- Movimiento de `AppShell` a `Presentation/Shell`.
- Creación de `HomeViewModel` en `Presentation/ViewModels`.
- Actualización de `App` y `MauiProgram` para resolver la shell, página y view model mediante inyección de dependencias.
- Sustitución del contador de plantilla por bindings MVVM simples.

## Decisiones técnicas

- La raíz MAUI mantiene `App` y `MauiProgram` como punto de composición y arranque.
- La navegación inicial se configura en `AppShell` usando una instancia de `MainPage` resuelta por DI para evitar constructores sin dependencias y mantener MVVM.
- El code-behind de `MainPage` queda limitado a inicialización de componentes y asignación de `BindingContext`.
- No se crean capas de dominio, aplicación o infraestructura vacías porque el proyecto aún no contiene casos de uso, entidades ni persistencia implementada.

## Impacto en arquitectura

- La UI queda agrupada bajo una capa lógica de presentación.
- No se introduce lógica de negocio en vistas ni code-behind.
- El dominio sigue sin depender de MAUI, SQLite o infraestructura porque todavía no existen clases de dominio implementadas.
- La composición de dependencias queda concentrada en `MauiProgram`, respetando el rol de composition root.

## Validaciones realizadas

- Se verificó la estructura actual del proyecto antes del cambio.
- Se comprobó que las clases MAUI de pantalla y shell estuvieran bajo `Presentation`.
- Se comprobó que el code-behind no contenga reglas de negocio ni lógica de contador de plantilla.
- Se actualizó la versión del proyecto a `1.0.1` y `ApplicationVersion` a `2`.
- Se registró el cambio en `CHANGELOG.md` siguiendo Keep a Changelog.
