# run-database-initializer-at-startup

## Objetivo del cambio
Ejecutar la inicialización de base de datos antes de iniciar la aplicación MAUI.

## Alcance
- Se actualizó `MauiProgram.CreateMauiApp` para resolver `IDatabaseInitializer` desde DI.
- Se invoca `InitializeAsync()` antes de retornar la instancia de `MauiApp`.

## Decisiones técnicas
- La inicialización se ejecuta en startup para garantizar lectura temprana de versión de base de datos.
- Se mantiene el contrato por abstracción (`IDatabaseInitializer`) sin acoplar arranque a implementación concreta.

## Impacto en arquitectura
- Se respeta separación por capas y consumo mediante interfaces.
- La UI no incorpora lógica de acceso a datos.

## Validaciones realizadas
- Compilación correcta tras el cambio.
