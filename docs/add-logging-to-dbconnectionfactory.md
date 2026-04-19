# add-logging-to-dbconnectionfactory

## Objetivo del cambio
Añadir trazabilidad del database path al crear una conexión SQLite.

## Alcance
- Inyectado `ILogger<DbConnectionFactory>` en `DbConnectionFactory`.
- Añadido `LogDebug` en `CreateConnection` mostrando el path completo de la base de datos.
- Configurado nivel mínimo de logging a `Debug` en `MauiProgram` en modo DEBUG.

## Decisiones técnicas
- Se usa `LogDebug` porque es información de diagnóstico solo relevante en desarrollo.
- El nivel mínimo por defecto de .NET es `Information`; se establece `Debug` explícitamente para que el log sea visible.

## Impacto en arquitectura
- Ninguno. Cambio aditivo sobre infraestructura existente.

## Validaciones realizadas
- Compilación correcta.
- Log visible en Salida → Depurar de Visual Studio al ejecutar en modo DEBUG.
