# Copilot Instructions

## Directrices del proyecto
- El repositorio exige documentar cada cambio significativo en /docs, registrarse en CHANGELOG.md siguiendo Keep a Changelog e incrementar la versión del proyecto manteniendo consistencia entre artefactos.
- En este repositorio debo respetar Clean Architecture + DDD + SOLID, con separación estricta de capas: el dominio no depende de UI, MAUI, SQLite ni infraestructura, y la UI no contiene lógica de negocio.
- Para este repositorio debo ceñirme al stack permitido: .NET 10, C#, .NET MAUI, Android, MVVM, SQLite, notificaciones locales y localización, evitando propuestas fuera de ese stack salvo uso explícito existente.
- Si uso constructores primarios para inyección de dependencias, debo asignar cada dependencia a un campo readonly con prefijo _ y no usar directamente el parámetro del constructor en métodos o propiedades.