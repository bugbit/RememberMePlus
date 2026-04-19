# fix-apprecord-sqlite-int64

## Objetivo del cambio
Corregir error de materialización de `AppRecord` por Dapper al leer datos desde SQLite.

## Problema
Dapper lanzaba `InvalidOperationException` indicando que no encontraba un constructor que aceptara `(Int64, Int64, Int64, Int64)` para materializar `AppRecord`, cuyos parámetros eran de tipo `int`.

## Causa
SQLite almacena todos los enteros internamente como `INTEGER` de 64 bits. Al leer con Dapper, los valores se reciben como `System.Int64` (`long`), no como `System.Int32` (`int`). Dapper requiere coincidencia exacta de tipos en el constructor del record.

## Solución
Cambiar los tipos de todos los parámetros de `AppRecord` de `int` a `long`.

## Alcance
- `AppRecord`: `IdApp`, `Version`, `RelativeOffsetMinutes`, `SnoozeMinutes` cambiados a `long`.

## Impacto en arquitectura
- Ninguno estructural. Cambio de tipo primitivo en el modelo de datos de infraestructura.

## Validaciones realizadas
- Compilación correcta.
- Dapper materializa `AppRecord` correctamente desde SQLite.
