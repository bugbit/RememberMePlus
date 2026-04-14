# handle-getfirstasync-exception-in-databaseinitializer

## Objetivo del cambio
Controlar fallos en la lectura del primer registro de `App` durante la inicialización para que el flujo continúe con valor nulo.

## Alcance
- Se actualizó `DatabaseInitializer.InitializeAsync`.
- Si `GetFirstAsync` lanza excepción, se asigna `app = null`.
- El método mantiene retorno `int?` con `app?.Version`.

## Decisiones técnicas
- Se encapsuló la llamada al repositorio en bloque `try/catch`.
- Ante excepción, se degrada a valor nulo en lugar de propagar la excepción.

## Impacto en arquitectura
- Se mantiene consumo por abstracción mediante `IAppRepository`.
- No se modifica la responsabilidad del repositorio ni de la UI.

## Validaciones realizadas
- Compilación correcta tras el ajuste.
