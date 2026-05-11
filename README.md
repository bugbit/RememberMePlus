# 📌 RememberMe+

## 📑 Índice

* [🧠 Descripción](#-descripción)
* [🚀 Objetivos del proyecto](#-objetivos-del-proyecto)
* [🏗️ Arquitectura](#️-arquitectura)
* [📁 Estructura de directorios](#-estructura-de-directorios)
* [🧩 Tecnologías previstas](#-tecnologías-previstas)
* [🔄 Flujo completo](#-flujo-completo)
* [⏸️ Política de posposición](#️-política-de-posposición)
* [🚨 Estrategias de notificación](#-estrategias-de-notificación)
* [👤 Alcance funcional](#-alcance-funcional)
* [🌍 Multiidioma](#-multiidioma)
* [🚀 Getting Started](#-getting-started)
* [📌 Principios de diseño](#-principios-de-diseño)
* [📈 Futuras mejoras](#-futuras-mejoras)
* [📄 Licencia](#-licencia)
* [📝 Tareas pendientes](#-tareas-pendientes)

## 🧠 Descripción

**RememberMe+** es una aplicación desarrollada en **.NET MAUI sobre .NET 10**, orientada a ayudar al usuario a recordar y gestionar tareas importantes de su día a día, como tomar medicación, hacer llamadas, asistir a citas o completar acciones críticas. La **plataforma objetivo en producción será Android**.

La aplicación combina varios pilares principales:

* una **pantalla principal** que muestra las **tareas vencidas** y las **tareas pendientes del día** próximas al vencimiento. Cuando se notifica una tarea, esta se muestra en primer lugar. Cada tarea puede tener un **checklist** para facilitar su completado
* un conjunto de **páginas de mantenimiento de tareas** para consultar, editar, posponer, activar, desactivar y completar recordatorios
* soporte para **asociar tareas a eventos o rutinas** del día, como desayuno, comida, cena o similar
* configuración de minutos de posposición a nivel **global** o **por tarea**
* soporte **multiidioma**, comenzando por **español** e **inglés**
* funcionamiento **100% local y autónomo**, sin depender de recursos externos

El sistema está pensado inicialmente para un **único usuario**, priorizando simplicidad, control y comportamiento proactivo. Su objetivo no es solo registrar recordatorios, sino también hacer que las tareas importantes resulten claramente visibles para el usuario cuando llegue el momento de atenderlas.

La aplicación debe poder **trabajar de forma autónoma en local**, sin depender de servicios externos, conectividad de red ni cobertura. Esto permite que el sistema esté **siempre disponible** incluso en entornos desconectados o con conectividad inestable.

La aplicación está orientada a una experiencia simple y directa, combinando una pantalla principal con mantenimiento manual de tareas y recordatorios.

---

## 🚀 Objetivos del proyecto

* Recordar tareas importantes a un único usuario
* Permitir interacción mediante la pantalla principal
* Mostrar en la pantalla principal las **tareas pendientes del día** que estén próximas a vencer, junto con las **tareas vencidas**, usando un **umbral horario configurable globalmente**
* Permitir asociar tareas a **eventos funcionales o rutinas** como desayunar, comer o cenar
* Ofrecer páginas de mantenimiento para gestionar tareas manualmente
* Permitir activar o desactivar tareas manualmente en cualquier momento
* Cuando una tarea se reactive, pedir **desde cuándo** debe volver a estar activa
* Hacer todo lo posible para que el usuario perciba una notificación importante mediante recursos locales del dispositivo, como sonido, visibilidad en pantalla de bloqueo y mayor prominencia visual
* Soportar una experiencia **multiidioma** desde el inicio, comenzando por **español** e **inglés**
* Permitir la **posposición configurable** a nivel global o por tarea
* Mantener una experiencia simple, local y orientada a productividad personal
* Orientar la aplicación a **Android como plataforma de producción**
* Garantizar funcionamiento **offline-first**, sin depender de recursos externos
* Usar **SQLite en local** como mecanismo principal de persistencia
* Dejar preparada la evolución futura para **backup y restauración de la base de datos** sin comprometer el funcionamiento local autónomo

---

## 🏗️ Arquitectura

La solución se plantea como una aplicación **.NET MAUI en .NET 10**, organizada en capas lógicas para mantener separación de responsabilidades, con **Android como plataforma objetivo en producción**.

### 1. App MAUI

Aplicación cliente desarrollada con MAUI, con **Android como plataforma objetivo en producción**, responsable de la experiencia de usuario.

**Responsabilidades:**

* interfaz de pantalla principal
* visualización en la pantalla principal de tareas vencidas y tareas pendientes del día próximas a vencer según un **umbral horario configurable globalmente**
* páginas de mantenimiento de tareas
* alta, edición, confirmación, posposición, activación y desactivación de tareas
* solicitud de fecha/hora efectiva al reactivar una tarea
* presentación de alertas y notificaciones locales
* uso de mecanismos de alta visibilidad para tareas importantes, como sonido, prioridad visual elevada y notificación en pantalla de bloqueo, aprovechando las capacidades disponibles en **Android**
* funcionamiento operativo incluso sin conexión o cobertura
* configuración de preferencias del usuario
* selección del idioma de la interfaz y recursos localizados

### 2. Núcleo de aplicación y dominio

Capa central donde reside la lógica funcional del sistema.

**Responsabilidades:**

* modelo de tareas, recordatorios, eventos funcionales asociados, estado de activación y fecha/hora efectiva de reactivación
* clasificación funcional de prioridades
* definición manual de prioridad en la tarea
* reglas de vencimiento y posposición
* exclusión de tareas desactivadas del ciclo de notificación y scheduler
* reactivación de tareas con fecha/hora efectiva indicada por el usuario
* estrategia de notificación reforazada para tareas importantes
* resolución de minutos de posposición a nivel global o por tarea
* modificación de minutos de posposición desde la propia notificación, persistiendo el cambio en la tarea
* asociación de tareas a eventos del día y resolución de recordatorios relativos al evento
* coordinación entre pantalla principal, mantenimiento y scheduler
* garantía de funcionamiento autónomo sin dependencias externas en tiempo de ejecución
* resolución de idioma para interfaz, notificaciones y textos funcionales
* priorización visual de la tarea notificada frente al resto de tareas pendientes visibles del día

### 3. Infraestructura

Capa encargada de la persistencia y de los servicios de plataforma.

**Responsabilidades:**

* almacenamiento local de tareas, configuración e historial mediante **SQLite**
* scheduler local de recordatorios
* integración con notificaciones locales del dispositivo
* uso exclusivo de recursos locales en tiempo de ejecución
* gestión de recursos de localización para interfaz y notificaciones

---

## 🧩 Tecnologías previstas

* **.NET 10**
* **.NET MAUI**
* **Android** como plataforma objetivo de despliegue en producción
* **C#**
* Patrón **MVVM**
* **SQLite** como base de datos local para tareas, eventos asociados, configuración e historial
* Notificaciones locales del dispositivo
* Recursos de localización para **español** e **inglés**
* Arquitectura **offline-first** sin dependencia de servicios externos

---

## 🔄 Flujo completo

1. El usuario crea, consulta o edita una tarea desde la página de mantenimiento
2. El sistema registra la tarea con su fecha, hora o evento asociado, prioridad, configuración de posposición y estado de activación
3. Si una tarea desactivada se vuelve a activar, el sistema solicita **desde cuándo** debe reactivarse y guarda esa fecha/hora efectiva
4. Un scheduler local revisa periódicamente las tareas vencidas o próximas a vencer, sin depender de conectividad externa
5. Cuando corresponde, se lanza la notificación local o el aviso dentro de la app
6. Si la app está abierta en la pantalla principal, primero se muestra la **tarea notificada** y después la **lista de tareas vencidas** y de **tareas pendientes del día** que estén dentro del **umbral horario global configurado**
7. El usuario puede:

   * confirmar la tarea
   * editarla
   * posponerla
   * reprogramarla
   * activarla
   * desactivarla
8. Si la tarea es importante, el sistema utiliza una estrategia de **alta visibilidad** para ayudar a que el usuario perciba la notificación, usando mecanismos locales del dispositivo como sonido, pantalla de bloqueo o mayor prominencia visual, según permita la plataforma
9. El estado de la tarea se actualiza y el ciclo continúa hasta su resolución

---

## ⏸️ Política de posposición

Los **minutos a posponer** se configuran de forma sencilla con dos únicos niveles:

### 1. Configuración global

Define el valor por defecto de minutos de posposición para toda la aplicación.

Ejemplo:

* posponer por defecto: **10 minutos**

### 2. Configuración por tarea

Cada tarea puede sobrescribir el valor global con su propia configuración.

Ejemplo:

* tarea “tomar medicación” → posponer **5 minutos**
* tarea “sacar la basura” → posponer **20 minutos**

### Regla de precedencia

Cuando una tarea tenga configuración propia, se utilizará ese valor. En caso contrario, se aplicará la configuración global.

1. **tarea**
2. **global**

### Cambio de minutos al notificar

Cuando el sistema notifique una tarea, el usuario podrá **posponerla** y también **modificar en ese momento los minutos de posposición**.

Ese cambio no será solo puntual: los nuevos minutos seleccionados quedarán **guardados en la propia tarea**, pasando a ser su configuración de posposición para siguientes ocasiones.

Con este enfoque, la configuración sigue siendo simple, pero permite ajustar cada tarea según la experiencia real de uso.

---

## 🚨 Estrategias de notificación

| Nivel         | Comportamiento                                                             |
| ------------- | -------------------------------------------------------------------------- |
| 🟢 Normal     | Notificación estándar                                                      |
| 🟡 Importante | Notificación reforzada con alta visibilidad                                |
| 🔴 Crítica    | Notificación reforzada con máxima visibilidad disponible en el dispositivo |

---

## 👤 Alcance funcional

Sistema diseñado inicialmente para **un único usuario**, lo que permite:

* simplificar lógica de identidad
* evitar complejidad de multiusuario
* optimizar la experiencia personal de recordatorios
* permitir pausar temporalmente tareas sin necesidad de eliminarlas
* permitir reactivarlas indicando la fecha/hora desde la que vuelven a aplicar
* adaptar reglas de posposición y prioridades a hábitos concretos
* dar mayor visibilidad a las tareas importantes sin depender de servicios externos

---

## 🌍 Multiidioma

La aplicación debe ser **multiidioma** desde su diseño.

Idiomas iniciales previstos:

* **español**
* **inglés**

Esto aplica a:

* interfaz de usuario
* textos funcionales
* notificaciones
* mensajes y textos de la pantalla principal

La estrategia de localización debe evitar textos embebidos en la lógica y centralizar los recursos traducibles.

---

## 🚀 Getting Started

### Requisitos previos

* **.NET 10 SDK**
* workloads de **.NET MAUI** instalados

---

### 1. Clonar el repositorio

```bash
git clone <repo-url>
cd RememberMePlus
```

---

### 2. Restaurar dependencias

```bash
dotnet restore
```

---

### 3. Configurar la aplicación

La aplicación debe permitir configurar, al menos, manteniendo siempre un funcionamiento totalmente local:

* política global de minutos de posposición
* configuración de minutos de posposición por tarea
* configuración de persistencia local SQLite
* umbral horario global para mostrar tareas pendientes próximas en la pantalla principal
* idioma por defecto de la aplicación y recursos localizados
* parámetros necesarios para funcionamiento autónomo sin red

Configuración conceptual mínima:

```json
{
  "Reminders": {
    "DefaultSnoozeMinutes": 10
  }
}
```

---

### 4. Ejecutar la app MAUI

```bash
dotnet build
```

Después, ejecutar la aplicación sobre **Android** según el entorno de desarrollo configurado, ya sea en emulador o dispositivo físico.

---

### 5. Probar los flujos principales

Una vez arrancada la app:

* crear una tarea desde la pantalla principal o desde mantenimiento
* comprobar que la pantalla principal muestra las tareas vencidas y las tareas pendientes del día que estén próximas a vencer según un umbral horario global
* editar una tarea desde mantenimiento
* confirmar una tarea vencida
* posponer una tarea
* comprobar que una tarea importante utiliza mecanismos de notificación reforzada en **Android**, como sonido o visibilidad en pantalla de bloqueo
* validar que la posposición respeta el alcance global o por tarea
* comprobar que al posponer desde una notificación se pueden cambiar los minutos y que el cambio se guarda en la tarea
* comprobar que cuando se notifica una tarea, primero se muestra esa tarea y después la lista de tareas pendientes
* comprobar que la interfaz y las notificaciones funcionan correctamente en español e inglés
* comprobar que una tarea puede quedar asociada a un evento como desayuno, comida o cena
* comprobar que una tarea puede activarse o desactivarse manualmente cuando se quiera
* comprobar que al activar una tarea se solicita desde cuándo debe volver a estar activa

## 📁 Estructura de directorios

```
RememberMePlus/
├── RememberMePlus.slnx                          # Solución
├── README.md
├── LICENSE
├── .gitignore
├── Remembermeplus_v1.db                         # Base de datos local SQLite
├── context/                                     # Contexto de diseño
│   └── database.sql
├── docs/                                        # Documentación de cambios
│   ├── add-domain-and-dapper-infrastructure.md
│   ├── restyle-add-task-page-like-main.md
│   ├── run-database-initializer-at-startup.md
│   ├── store-database-version-in-initializer-state.md
│   ├── update-mainpage-pending-tasks-checklist.md
│   ├── update-mainpage-pending-today-or-overdue.md
│   ├── update-task-complete-recurring-rule.md
│   └── update-uow-and-sqlexecutor-separation.md
└── RememberMePlusApp/                           # Proyecto MAUI principal
    ├── RememberMePlusApp.csproj
    ├── MauiProgram.cs                           # Configuración DI y MAUI builder
    ├── App.xaml / App.xaml.cs                   # Punto de entrada de la app
    ├── AppShell.xaml / AppShell.xaml.cs         # Shell de navegación
    ├── MainPage.xaml / MainPage.xaml.cs         # Página principal (legacy)
    ├── AddTaskPage.xaml / AddTaskPage.xaml.cs   # Página de alta de tarea (legacy)
    ├── AlarmAlertPage.xaml.cs
    ├── AlarmAlertCoordinator.cs
    ├── MauiServiceProvider.cs
    ├── Domain/                                  # Capa de dominio
    │   ├── App/
    │   │   └── AppRecord.cs
    │   └── Tasks/
    │       ├── ReminderTask.cs
    │       ├── ReminderTaskId.cs
    │       ├── ReminderTitle.cs
    │       ├── ReminderPriority.cs
    │       └── PostponeMinutes.cs
    ├── Application/                             # Capa de aplicación
    │   └── Alarms/
    │       ├── AlarmActionService.cs
    │       ├── AlarmStartupService.cs
    │       ├── AlarmTriggerHandler.cs
    │       ├── DateTimeExtensions.cs
    │       ├── IAlarmAlertCoordinator.cs
    │       ├── IAlarmNotificationService.cs
    │       ├── IAlarmScheduler.cs
    │       ├── IAlarmStartupService.cs
    │       ├── IAlarmTriggerHandler.cs
    │       └── ScheduledReminderTask.cs
    ├── Infrastructure/                          # Capa de infraestructura (SQLite + Dapper + Alarmas)
    │   ├── DependencyInjection.cs
    │   ├── Alarms/
    │   │   ├── MauiAlarmNotificationService.cs
    │   │   └── TimerAlarmScheduler.cs
    │   └── Data/
    │       ├── Abstractions/
    │       │   ├── IDatabaseInitializer.cs
    │       │   └── IDbConnectionFactory.cs
    │       ├── Options/
    │       │   └── SqliteDataOptions.cs
    │       ├── Dapper/
    │       │   ├── Mappers/
    │       │   │   └── ReminderTaskDataMapper.cs
    │       │   ├── Models/
    │       │   │   └── ReminderTaskDataModel.cs
    │       │   ├── Repositories/
    │       │   │   ├── DapperDatabaseSchemaRepository.cs
    │       │   │   ├── DapperReminderTaskRepository.cs
    │       │   │   └── DapperRepository.cs
    │       │   ├── Schema/
    │       │   │   └── SqliteDatabaseInitializer.cs
    │       │   ├── SqliteDbConnectionFactory.cs
    │       │   ├── UnitOfWork.cs
    │       │   └── UnitOfWorkFactory.cs
    │       ├── IAppRepository.cs
    │       ├── IDatabaseSchemaRepository.cs
    │       ├── ISqlExecutor.cs
    │       ├── ITaskRepository.cs
    │       ├── IUnitOfWork.cs
    │       └── IUnitOfWorkFactory.cs
    ├── Presentation/                            # Capa de presentación (MVVM)
    │   ├── Pages/
    │   │   └── MainPage.xaml / MainPage.xaml.cs
    │   ├── Shell/
    │   │   └── AppShell.xaml / AppShell.xaml.cs
    │   └── ViewModels/
    │       └── HomeViewModel.cs
    ├── ViewModels/                              # ViewModels adicionales
    │   ├── MainPageViewModel.cs
    │   └── AddTaskPageViewModel.cs
    ├── Resources/
    │   ├── AppIcon/
    │   ├── Fonts/
    │   │   ├── OpenSans-Regular.ttf
    │   │   └── OpenSans-Semibold.ttf
    │   ├── Images/
    │   ├── Raw/
    │   ├── Scripts/
    │   │   └── database-v1.sql                 # Script de inicialización de BD
    │   ├── Splash/
    │   └── Styles/
    │       ├── Colors.xaml
    │       └── Styles.xaml
    ├── Platforms/
    │   ├── Android/
    │   │   └── Alarms/
    │   ├── iOS/
    │   ├── MacCatalyst/
    │   └── Windows/
    └── Properties/
        └── launchSettings.json
```

---

## 📌 Principios de diseño

* separación entre interfaz, dominio e infraestructura
* experiencia unificada entre pantalla principal y mantenimiento manual
* pantalla principal orientada a foco inmediato en la tarea notificada y visibilidad secundaria de tareas vencidas y pendientes próximas del día según configuración global
* configuración simple de posposición, con alcance global o por tarea
* activación y desactivación manual de tareas sin eliminarlas
* reactivación explícita con fecha/hora efectiva definida por el usuario
* soporte para tareas basadas en hora fija o en eventos funcionales
* prioridad a una experiencia simple, robusta y personal
* uso de notificaciones locales con la máxima visibilidad posible según la importancia de la tarea, priorizando las capacidades disponibles en **Android**
* disponibilidad permanente en local, incluso sin cobertura
* arquitectura **offline-first** y sin dependencias externas en tiempo de ejecución
* localización desde el diseño, evitando textos embebidos en la lógica

---

## 📈 Futuras mejoras

* sugerencias automáticas de reprogramación
* catálogo configurable de eventos funcionales del usuario, como desayuno, comida, cena, medicación o descanso
* recordatorios relativos a eventos, por ejemplo “después de desayunar” o “antes de cenar”
* vistas avanzadas de calendario y planificación
* backup y restauración de la base de datos local
* ampliación futura a más idiomas además de español e inglés
* mecanismos de exportación/importación manual para entornos sin conectividad
* soporte futuro a otras plataformas además de Android, siempre que no comprometa el modo local autónomo
* sincronización entre dispositivos, siempre que no comprometa el modo local autónomo
* modo temporal para vacaciones o cambios puntuales de rutina
* analítica de cumplimiento de tareas

---

## 📄 Licencia

MIT
