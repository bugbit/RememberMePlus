```mermaid
erDiagram
  TaskScheduler {
    identity id_taskscheduler pk
    datetime hour_due_at "* Hora que hay que hacer la tarea"
    datetime hour_save_due_at " Hora guardada para hacer horas temporales"
    int recurrence_interval "*"
    int recurrence_type "* 1=> dia, 2=> semana, 3=> mes, 4=> año"
    bit recurrence_sunday
    bit recurrence_Monday
    bit recurrence_Tuesday
    bit recurrence_Wednesday
    bit recurrence_Thursday
    bit recurrence_Friday
    bit recurrence_Saturday
    int recurrence_interval_month "el primer martes, segundo martes, ... cada n weekday"
  }

  TaskEvent {
    identity id_task_event pk
    string name "*"
    int id_taskscheduler fk
  }

  TaskGroup {
    identity id_task_group pk
    string name "*"
  }

  Task {
    identity id_task pk
    string title "*"
    string description
    int id_task_group fk
    bool is_insistent "*"
    datetime date_create
    datetime date_due_at "* Fecha de vencimientoo"
    datetime date_due_at_last " Ultima Fecha de vencimiento"
    int id_ttaskscheduler fk
    int id_task_event fk
    bit is_repeat "*"
    bit is_active "*"           
  }

  TaskScheduler ||--|{ Task : contains
  TaskEvent ||--|{ Task : contains
  TaskGroup ||--|{ Task : contains
  TaskScheduler ||--|{ TaskEvent : contains
