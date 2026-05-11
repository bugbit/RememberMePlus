# Update architecture documentation structure

## Objetivo del cambio

Actualizar la documentación principal del repositorio para reflejar la estructura actual basada en Clean Architecture, DDD, MVVM y persistencia SQLite/Dapper separada en infraestructura.

## Alcance

- Actualización de `README.md` con la estructura actual de directorios y responsabilidades por capa.
- Actualización de `AGENTS.md` con reglas explícitas sobre `Domain`, `Application`, `Infrastructure`, `Infrastructure/Data/Dapper`, `Presentation` y `MauiProgram`.
- Inclusión de Dapper como tecnología permitida únicamente para acceso local a SQLite en infraestructura.
- Incremento de versión del proyecto.
- Registro del cambio en `CHANGELOG.md`.

## Decisiones técnicas

- La estructura documentada mantiene las capas dentro del proyecto MAUI actual porque la solución aún contiene un único `.csproj`.
- Se explicita que las clases que usan Dapper directamente deben quedar bajo `Infrastructure/Data/Dapper`.
- Se documenta que los modelos de persistencia no reemplazan a entidades ni value objects del dominio.
- Se conserva `MauiProgram` como composition root, sin reglas de negocio.

## Impacto en arquitectura

- Las reglas de contribución quedan alineadas con la estructura real del código.
- La documentación reduce ambigüedad sobre dónde ubicar nuevas clases.
- Se refuerza que `Domain` y `Application` no deben depender de detalles de infraestructura.
- Se evita que Dapper se filtre hacia presentación, aplicación o dominio.

## Validaciones realizadas

- Se revisó la estructura actual de archivos del proyecto.
- Se actualizó `README.md` con el árbol de directorios y responsabilidades por capa.
- Se actualizó `AGENTS.md` con reglas específicas de ubicación de clases.
- Se actualizó la versión del proyecto a `1.0.4` y `ApplicationVersion` a `5`.
- Se registró el cambio en `CHANGELOG.md` siguiendo Keep a Changelog.
