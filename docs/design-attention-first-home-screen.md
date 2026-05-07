# Diseño de pantalla principal attention-first

## Objetivo del cambio

Diseñar una pantalla principal para RememberMe+ orientada a personas despistadas o con problemas de atención, priorizando de forma extrema las tareas vencidas y las tareas a punto de vencer dentro de la ventana definida por `App.relative_offset_minutes`.

Con el ejemplo funcional indicado:

- Fecha actual local: `2026-01-04 12:00`.
- `relative_offset_minutes`: `120`.
- Ventana de próximo vencimiento: `2026-01-04 12:00` -> `2026-01-04 14:00`.
- Tareas vencidas: Tarea 1, Tarea 2, Tarea 3, Tarea 4 y Tarea 5.
- Tareas próximas a vencer: Tarea 6.

## Alcance

El diseño cubre:

- Estructura visual completa de la pantalla principal en Android con .NET MAUI.
- Jerarquía visual, colores, componentes reutilizables y estados de alerta.
- Comportamiento de actualización automática cuando cambia el tiempo o una tarea cruza el umbral de vencida.
- Propuesta de ViewModel, contratos y servicios manteniendo Clean Architecture, DDD, MVVM y SOLID.
- Recomendaciones de accesibilidad, modo oscuro y animaciones suaves.

No implementa todavía la pantalla en código productivo; define la propuesta trazable para una implementación posterior.

## Regla funcional de clasificación

La aplicación debe clasificar solo tareas activas:

```text
now = reloj local actual
windowEnd = now + App.relative_offset_minutes

vencida:
  task.date_due_at < now

próxima a vencer:
  task.date_due_at >= now && task.date_due_at <= windowEnd

urgente dentro de próximas:
  task.date_due_at <= now + 15 minutos
```

La comparación debe realizarse en un caso de uso de aplicación, no en la vista. La UI solo consume grupos ya preparados para presentación.

## Estructura visual

La pantalla debe tener una composición vertical simple, con prioridad descendente:

```text
┌──────────────────────────────────────┐
│ RememberMe+                          │
│ Ahora: 04/01/2026 12:00              │
├──────────────────────────────────────┤
│ 🔴 5 TAREAS VENCIDAS                 │
│ Necesitan atención ahora             │
│ ┌──────────────────────────────────┐ │
│ │ ⚠️ Tarea 1                       │ │
│ │ Vencida hace 3 días y 3 horas    │ │
│ │ [Completar] [Posponer] [Ignorar] │ │
│ └──────────────────────────────────┘ │
│ ┌──────────────────────────────────┐ │
│ │ ⚠️ Tarea 2                       │ │
│ │ Vencida hace 3 días y 2 horas    │ │
│ │ [Completar] [Posponer] [Ignorar] │ │
│ └──────────────────────────────────┘ │
├──────────────────────────────────────┤
│ 🟠 1 A PUNTO DE VENCER               │
│ Dentro de los próximos 120 minutos   │
│ ┌──────────────────────────────────┐ │
│ │ ⏰ Tarea 6                       │ │
│ │ Vence en 2 horas                 │ │
│ │ 04/01/2026 14:00                 │ │
│ │ [Completar] [Posponer]           │ │
│ └──────────────────────────────────┘ │
└──────────────────────────────────────┘
```

Si no hay tareas vencidas, la primera sección debe quedar reemplazada por un estado tranquilo de alto contraste:

```text
✅ Sin tareas vencidas
Mantén el foco en lo próximo.
```

## Colores y estados visuales

Usar recursos semánticos en `Resources/Styles/Colors.xaml`, nunca colores literales dispersos en XAML:

| Token | Modo claro | Modo oscuro | Uso |
| --- | --- | --- | --- |
| `OverdueBackground` | `#FDECEC` | `#3A1010` | Fondo de sección vencida |
| `OverdueBorder` | `#C62828` | `#FF6B6B` | Borde de tarjeta vencida |
| `OverdueText` | `#7F0000` | `#FFE5E5` | Texto principal vencido |
| `DueSoonBackground` | `#FFF4D6` | `#332400` | Fondo de sección próxima |
| `DueSoonBorder` | `#F57C00` | `#FFB74D` | Borde de tarjeta próxima |
| `CriticalSoonBorder` | `#D84315` | `#FF8A65` | Próxima en menos de 15 minutos |
| `CardSurface` | `#FFFFFF` | `#1E1E1E` | Superficie de tarjetas |
| `PrimaryAction` | `#1B5E20` | `#81C784` | Completar |
| `SecondaryAction` | `#0D47A1` | `#90CAF9` | Posponer |
| `TemporaryAction` | `#424242` | `#BDBDBD` | Ignorar temporalmente |

El rojo solo debe utilizarse para tareas vencidas. El naranja/amarillo queda reservado para próximas a vencer. Esto reduce ambigüedad cognitiva.

## Componentes reutilizables propuestos

### `HomeAlertSummaryView`

Componente superior compacto con:

- cantidad de vencidas;
- cantidad de próximas;
- hora actual local;
- texto de acción principal: `Revisa lo urgente primero`.

Debe ser puramente visual y recibir datos desde el ViewModel.

### `TaskUrgencySectionView`

Componente de sección con:

- título localizado;
- icono;
- color semántico;
- descripción breve;
- colección de tarjetas.

Debe servir tanto para `Tareas vencidas` como para `A punto de vencer`.

### `UrgentTaskCardView`

Tarjeta grande reutilizable para una tarea:

- icono de estado (`⚠️`, `⏰`, `🔥` para menos de 15 minutos);
- título en tipografía grande;
- vencimiento absoluto;
- texto relativo (`Vencida hace...` o `Vence en...`);
- acciones táctiles grandes.

Tamaño recomendado en Android:

- título: 22-26 sp;
- texto secundario: 16-18 sp;
- botones: altura mínima 52 dp;
- margen entre tarjetas: 12-16 dp;
- radio de tarjeta: 18-24 dp.

## Layout .NET MAUI recomendado

Usar `CollectionView` dentro de un `ScrollView` solo si las listas son pequeñas. Para evitar problemas de virtualización, la opción preferida es una única `CollectionView` con items de presentación heterogéneos: cabecera, sección vencida, tarjetas vencidas, sección próximas y tarjetas próximas.

Para una primera implementación simple y mantenible, se acepta:

```xml
<Grid RowDefinitions="Auto,*" Padding="16">
    <views:HomeAlertSummaryView Grid.Row="0" />

    <ScrollView Grid.Row="1">
        <VerticalStackLayout Spacing="20">
            <views:TaskUrgencySectionView
                Title="{Binding OverdueTitle}"
                Items="{Binding OverdueTasks}"
                AlertLevel="Overdue" />

            <views:TaskUrgencySectionView
                Title="{Binding DueSoonTitle}"
                Items="{Binding DueSoonTasks}"
                AlertLevel="DueSoon" />
        </VerticalStackLayout>
    </ScrollView>
</Grid>
```

Si la cantidad de tareas puede crecer, migrar a `CollectionView` agrupado para mantener rendimiento y accesibilidad.

## Comportamiento UX

### Actualización automática

La pantalla debe refrescarse mientras está visible:

- al aparecer la página;
- cada 60 segundos como máximo;
- al volver la app a primer plano;
- después de completar, posponer o ignorar una tarea.

Cuando una tarea próxima cruza `date_due_at < now`, el caso de uso debe devolverla en el grupo de vencidas en la siguiente actualización. La UI no debe moverla manualmente.

### Acciones principales

Las acciones deben ser visibles y tener orden consistente:

1. `Completar`: acción positiva principal, color verde, botón más prominente.
2. `Posponer`: acción secundaria, permite aplicar minutos globales o de tarea.
3. `Ignorar temporalmente`: solo en vencidas, reduce ruido sin completar la tarea.

Los botones deben incluir texto, no solo iconos, para mejorar comprensión.

### Animaciones y atención

Las animaciones deben ser suaves, breves y respetar accesibilidad:

- tarjetas vencidas críticas: pulso de borde rojo cada 2-3 segundos;
- tareas próximas en menos de 15 minutos: pulso naranja más discreto;
- vibración Android solo para tareas importantes o críticas y con control de preferencia;
- evitar parpadeo rápido para no generar fatiga visual.

La implementación debe comprobar una preferencia local como `attention_animations_enabled` antes de animar. Si no existe todavía, usar el comportamiento visual estático hasta añadir esa configuración.

## Propuesta de ViewModel

El ViewModel debe exponer estado ya preparado para la vista:

```csharp
public sealed partial class HomeViewModel(
    IGetHomeUrgencyDashboardQuery getHomeUrgencyDashboardQuery,
    ICompleteTaskUseCase completeTaskUseCase,
    ISnoozeTaskUseCase snoozeTaskUseCase,
    ITemporarilyIgnoreTaskUseCase temporarilyIgnoreTaskUseCase,
    IAppClock appClock)
{
    private readonly IGetHomeUrgencyDashboardQuery _getHomeUrgencyDashboardQuery = getHomeUrgencyDashboardQuery;
    private readonly ICompleteTaskUseCase _completeTaskUseCase = completeTaskUseCase;
    private readonly ISnoozeTaskUseCase _snoozeTaskUseCase = snoozeTaskUseCase;
    private readonly ITemporarilyIgnoreTaskUseCase _temporarilyIgnoreTaskUseCase = temporarilyIgnoreTaskUseCase;
    private readonly IAppClock _appClock = appClock;
}
```

La regla obligatoria de constructores primarios se mantiene: los parámetros se asignan a campos `readonly` con prefijo `_` y solo se usan esos campos.

## Casos de uso de aplicación propuestos

### `IGetHomeUrgencyDashboardQuery`

Responsabilidad:

- leer `App.relative_offset_minutes`;
- obtener tareas activas relevantes desde el repositorio;
- clasificar vencidas y próximas;
- devolver DTOs de presentación independientes de MAUI.

Contrato sugerido:

```csharp
public interface IGetHomeUrgencyDashboardQuery
{
    Task<HomeUrgencyDashboardDto> ExecuteAsync(CancellationToken cancellationToken);
}
```

### DTOs de aplicación

```csharp
public sealed record HomeUrgencyDashboardDto(
    DateTime Now,
    int RelativeOffsetMinutes,
    IReadOnlyList<HomeTaskUrgencyDto> OverdueTasks,
    IReadOnlyList<HomeTaskUrgencyDto> DueSoonTasks);

public sealed record HomeTaskUrgencyDto(
    long Id,
    string Title,
    DateTime DueAt,
    TimeSpan Delta,
    TaskUrgencyLevel UrgencyLevel);

public enum TaskUrgencyLevel
{
    Overdue,
    DueSoon,
    DueInLessThan15Minutes
}
```

Estos DTOs no deben depender de `Color`, `ImageSource`, `Command`, `View` ni otros tipos MAUI.

## Repositorios y consultas

El repositorio de tareas debe exponer una consulta por intención, no por necesidad de UI:

```csharp
public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItemRecord>> GetActiveDueBeforeAsync(
        DateTime dueBeforeOrEqual,
        CancellationToken cancellationToken);
}
```

La infraestructura implementa la consulta con SQLite y Dapper. El caso de uso decide cómo separar vencidas y próximas usando `now` y `windowEnd`.

## Prioridades visuales

Orden recomendado dentro de cada grupo:

1. mayor retraso para vencidas;
2. menor tiempo restante para próximas;
3. título como desempate estable.

La priorización por importancia no se aplica mientras la tabla `Task` no exponga un campo persistido para esa regla.

La primera tarjeta vencida debe ocupar más altura y mostrar una llamada clara:

```text
⚠️ Haz esto primero
Tarea 1
Vencida hace 3 días y 3 horas
```

Esto ayuda a personas con dificultades de atención a no tener que decidir entre demasiadas opciones simultáneamente.

## Accesibilidad

Requisitos mínimos:

- textos con `SemanticProperties.Description` en secciones y botones;
- alto contraste en modo claro y oscuro;
- botones con altura mínima de 52 dp;
- no depender exclusivamente del color: combinar color, icono y texto;
- soporte de Dynamic Type mediante estilos de texto, evitando tamaños rígidos innecesarios;
- estados vacíos claros y tranquilizadores;
- reducir animaciones si el usuario desactiva animaciones de atención.

## Localización

Agregar claves de recursos para español e inglés:

| Clave | Español | Inglés |
| --- | --- | --- |
| `Home_Overdue_Title` | `Tareas vencidas` | `Overdue tasks` |
| `Home_DueSoon_Title` | `A punto de vencer` | `Due soon` |
| `Home_Overdue_Subtitle` | `Necesitan atención ahora` | `Need attention now` |
| `Home_DueSoon_Subtitle` | `Dentro de los próximos {0} minutos` | `Within the next {0} minutes` |
| `Home_Complete` | `Completar` | `Complete` |
| `Home_Snooze` | `Posponer` | `Snooze` |
| `Home_IgnoreTemporarily` | `Ignorar temporalmente` | `Ignore temporarily` |
| `Home_DueIn` | `Vence en {0}` | `Due in {0}` |
| `Home_OverdueBy` | `Vencida hace {0}` | `Overdue by {0}` |

## Impacto en arquitectura

La propuesta mantiene las capas separadas:

- Dominio: conserva reglas de tarea, vencimiento, prioridad e importancia sin referencias a MAUI o SQLite.
- Aplicación: orquesta la consulta del dashboard, obtiene configuración global y clasifica por ventana temporal.
- Infraestructura: implementa repositorios con SQLite y Dapper.
- UI: renderiza DTOs, ejecuta comandos del ViewModel y no contiene lógica de negocio.

Toda clase de servicio o repositorio propuesta cuenta con interfaz y se consume por abstracción.

## Validaciones realizadas

- Clean Architecture: la clasificación funcional se ubica en aplicación y la vista solo renderiza estado.
- DDD: el vencimiento y la urgencia se tratan como reglas del dominio/aplicación, no como detalles visuales.
- SOLID: los componentes visuales, casos de uso y repositorios tienen responsabilidades separadas.
- Stack permitido: la propuesta usa .NET 10, C#, .NET MAUI, Android, MVVM, SQLite, Dapper, notificaciones locales y localización.
- Offline-first: no se requiere ningún servicio externo.
- Accesibilidad: se priorizan contraste, tipografía grande, botones grandes, texto más icono y animaciones suaves configurables.
