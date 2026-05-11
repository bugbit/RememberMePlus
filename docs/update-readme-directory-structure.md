# Update README directory structure

## Objetivo del cambio

Actualizar `README.md` para documentar la estructura de carpetas usada por las capas de dominio e infraestructura de tareas recordatorio.

## Alcance

- Se añadió una sección `Estructura de directorios` al README.
- Se documentaron las carpetas `Domain/Tasks` e `Infrastructure/Data` con sus clases principales.
- Se actualizó el índice del README para enlazar la nueva sección.
- Se incrementó la versión del proyecto a `1.1.1` / `3`.
- Se registró el cambio en `CHANGELOG.md`.

## Decisiones técnicas

- La estructura documentada refleja la separación actual entre dominio e infraestructura.
- El README describe el propósito de cada carpeta sin duplicar detalles de implementación.
- Se mantiene la documentación en español para conservar consistencia con el documento principal.

## Impacto en arquitectura

- No se modificó código de ejecución ni reglas de negocio.
- La documentación refuerza la separación Clean Architecture entre dominio, contratos e implementación de persistencia.
- No se añadió dependencia nueva ni lógica en UI.

## Validaciones realizadas

- Se comprobó que la nueva sección del README referencia las carpetas existentes.
- Se comprobó que el cambio queda documentado en `/docs`.
- Se comprobó que la versión y el changelog quedan actualizados.
