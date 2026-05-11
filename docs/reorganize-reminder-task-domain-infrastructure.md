# Reorganize reminder task domain and infrastructure

## Objetivo del cambio

Organizar las clases base de tareas recordatorio siguiendo la estructura solicitada para separar dominio e infraestructura de persistencia local.

## Alcance

- Se añadió `RememberMePlusApp/Domain/Tasks` para el agregado `ReminderTask`, value objects, prioridad y contrato de repositorio.
- Se añadió `RememberMePlusApp/Infrastructure` para registro de dependencias.
- Se añadió `RememberMePlusApp/Infrastructure/Data` con abstracciones, opciones SQLite y una implementación Dapper para SQLite.
- Se conectó la infraestructura desde `MauiProgram`.
- Se actualizó la versión del proyecto a `1.1.0` / `2`.

## Decisiones técnicas

- El dominio contiene reglas y tipos propios sin depender de MAUI, SQLite ni Dapper.
- La infraestructura implementa el contrato `IReminderTaskRepository` definido en la capa de dominio.
- Las fechas se persisten como texto en formato round-trip (`O`) para conservar zona/desplazamiento y permitir orden lexicográfico por vencimiento.
- `SqliteDataOptions` centraliza la ruta local de la base de datos en el directorio de datos de la aplicación.

## Impacto en arquitectura

- Refuerza Clean Architecture al aislar el modelo de tarea del detalle de persistencia.
- Mantiene DDD con un agregado `ReminderTask` y value objects para identidad, título y minutos de posposición.
- Mantiene SOLID separando conexión, inicialización de esquema, mapeo y repositorio.
- La UI queda sin lógica de negocio y solo registra infraestructura desde el arranque MAUI.

## Validaciones realizadas

- Se comprobó que las clases de dominio no referencian MAUI, SQLite ni Dapper.
- Se comprobó que las clases de infraestructura dependen de abstracciones y del dominio, no de vistas.
- Se comprobó que los constructores primarios asignan dependencias a campos `readonly` con prefijo `_`.
- Se intentó validar con `dotnet --version`, pero el SDK de .NET no está instalado en el entorno.
