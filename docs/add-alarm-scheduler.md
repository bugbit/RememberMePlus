# Add Alarm Scheduler

## Objetivo

Añadir la funcionalidad de programación de alarmas locales para tareas activas de RememberMe+, usando `datetime_notify_at` como fecha-hora de aviso y descartando los segundos para evitar disparos con precisión innecesaria.

## Alcance

- Normalizar en el arranque las tareas activas con `datetime_notify_at` vencido a la fecha-hora local actual.
- Leer la próxima tarea activa a vencer por `datetime_notify_at` y enviarla al Alarm Scheduler.
- Usar `AlarmManager` en Android.
- Usar `Timer` como alternativa para otros entornos.
- Mostrar notificación local sonora y una ventana modal de atención cuando una alarma vence.
- Permitir completar o posponer desde la ventana de alarma.
- Calcular los minutos de posposición desde `Task.snooze_minutes`; si no existe, usar `App.snooze_minutes`.
- Recalcular la próxima alarma tras completar, posponer o crear tareas.

## Decisiones técnicas

- La capa de aplicación contiene los contratos y casos de uso del scheduler (`IAlarmStartupService`, `IAlarmTriggerHandler`, `IAlarmActionService`).
- La infraestructura Dapper expone consultas específicas para normalizar avisos vencidos, obtener la próxima tarea a notificar y recuperar una tarea por alarma.
- Android programa un único `PendingIntent` actualizable para la siguiente tarea, delegando el cálculo del siguiente vencimiento al caso de uso tras cada acción.
- El fallback no Android mantiene un único `Timer` activo para reproducir el mismo flujo funcional sin depender de APIs Android.
- La ventana `AlarmAlertPage` queda en UI y solo delega acciones a `IAlarmActionService`; no contiene lógica de negocio.

## Impacto en arquitectura

- El dominio se mantiene sin dependencias de MAUI, Android, SQLite o Dapper.
- La aplicación orquesta el flujo de arranque, disparo y acciones de alarma mediante interfaces.
- La infraestructura implementa la programación concreta por plataforma y la persistencia SQLite/Dapper.
- La UI muestra la interacción de alarma y delega completar/posponer a la capa de aplicación.

## Validaciones realizadas

- Clean Architecture: las decisiones de programación y acciones están orquestadas fuera de las vistas.
- DDD: la funcionalidad respeta la entidad de tarea existente y utiliza `datetime_notify_at` como dato persistido de planificación.
- SOLID: scheduler, notificación, alerta y acciones están separados por interfaces específicas.
- Stack permitido: .NET 10, C#, .NET MAUI, Android, SQLite, Dapper, notificaciones locales.
- Versionado: actualizado a `1.5.0` con `ApplicationVersion` 26.
- CHANGELOG: registrada la versión `1.5.0` siguiendo Keep a Changelog.
