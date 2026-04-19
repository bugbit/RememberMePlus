# add-mainpage-app-info

## Objetivo del cambio
Mostrar los datos de la tabla `App` en la pantalla principal de la aplicación.

## Alcance
- `MainPage.xaml`: reemplazado el template por defecto por un `Grid` con los cuatro campos de `AppRecord` (IdApp, Version, RelativeOffsetMinutes, SnoozeMinutes).
- `MainPage.xaml.cs`: inyecta `IUnitOfWorkFactory` y `IAppRepository`; carga y muestra los datos en `OnAppearing`.
- `MauiProgram.cs`: registrado `MainPage` como `Transient` en DI.

## Decisiones técnicas
- Se usa `OnAppearing` para la carga de datos async porque es el punto del ciclo de vida de MAUI que permite `async void` de forma segura y se ejecuta cada vez que la página se muestra.
- `MainPage` se registra como `Transient` para que DI resuelva sus dependencias correctamente.

## Impacto en arquitectura
- La UI consume datos a través de las abstracciones `IUnitOfWorkFactory` e `IAppRepository`, sin dependencia directa en infraestructura concreta.

## Validaciones realizadas
- Compilación correcta.
