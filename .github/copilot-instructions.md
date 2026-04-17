# Copilot Instructions

## General Guidelines
- El repositorio exige documentar cada cambio significativo en /docs, registrarse en CHANGELOG.md siguiendo Keep a Changelog e incrementar la versión del proyecto manteniendo consistencia entre artefactos.
- En este repositorio debo seguir AGENTS.md y .github/copilot-instructions.md: Clean Architecture + DDD + SOLID, stack .NET 10 MAUI/Android/MVVM/SQLite, y considerar `context/database.sql` como estructura de BD.
- La base de datos a usar es SQLite, y se usará Dapper como parte del stack de acceso a datos junto con SQLite. Sin embargo, en `DatabaseInitializer`, no se debe consultar con Dapper directamente; se debe usar una clase Repository para leer el primer registro de la tabla `App`. Además, `DatabaseInitializer` y `AppRepository` deben trabajar mediante interfaces (abstracciones) en lugar de dependencias concretas.
- En este repositorio, toda clase de tipo servicio o repositorio debe tener su interfaz correspondiente y consumirse mediante abstracción.

## Code Style
- Si uso constructores primarios para inyección de dependencias, debo asignar cada dependencia a un campo readonly con prefijo _ y no usar directamente el parámetro del constructor en métodos o propiedades.

## Project-Specific Rules
- Para este repositorio debo respetar Clean Architecture + DDD + SOLID, con separación estricta de capas: el dominio no depende de UI, MAUI, SQLite ni infraestructura, y la UI no contiene lógica de negocio.
- Debo ceñirme al stack permitido: .NET 10, C#, .NET MAUI, Android, MVVM, SQLite, notificaciones locales y localización, evitando propuestas fuera de ese stack salvo uso explícito existente.