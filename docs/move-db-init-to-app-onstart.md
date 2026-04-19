# move-db-init-to-app-onstart

## Objetivo del cambio
Eliminar el bloqueo del hilo principal durante la inicialización de la base de datos en el arranque de la app.

## Problema
`MauiProgram` llamaba a `databaseInitializer.InitializeAsync().GetAwaiter().GetResult()`, bloqueando el hilo principal durante la inicialización de la BD, lo que podía causar:
- Congelación de la UI durante el arranque.
- Deadlocks potenciales si el código async interno intentaba volver al hilo principal.

## Solución
Mover la inicialización al método `App.OnStart()`, que forma parte del ciclo de vida de MAUI y soporta `async void` de forma segura sin bloquear el hilo principal.

## Alcance
- `App.xaml.cs`: inyecta `IDatabaseInitializer` y llama `await InitializeAsync()` en `OnStart()`.
- `MauiProgram.cs`: eliminado el bloque `.GetAwaiter().GetResult()` y registrado `App` como singleton en DI.

## Decisiones técnicas
- `OnStart()` es el punto correcto del ciclo de vida MAUI para inicialización async post-construcción.
- `App` se registra como singleton para que DI pueda inyectarle dependencias.

## Impacto en arquitectura
- La UI responde inmediatamente al arranque.
- La inicialización de la BD es no bloqueante y segura frente a deadlocks.

## Validaciones realizadas
- Compilación correcta.
