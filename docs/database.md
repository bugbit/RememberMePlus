```mermaid
erDiagram
  Task {
    identity id pk
    string title "*"
    string description
    int priority "* 0=> normal, 999=> "
    datetime date_create
    datetime date_due_at "* Fecha de vencimientoo"
    datetime date_due_at_last " Ultima Fecha de vencimiento"
    bit is_repeat "*"
    bit is_active "*"           
  }

  TaskScheduler {
    datetime hour_due_at "* Hora que hay que hacer la tarea"
    datetime hour_save_due_at " Hora guardada para hacer horas temporales"
    int recurrence_interval "*"
    int recurrence_type "* 1=> dia, 2=> semana, 3=> mes, 4=> año"
    bit recurrence_sunday "*"
    bit recurrence_Monday "*"
    bit recurrence_Tuesday "*"
    bit recurrence_Wednesday "*"
    bit recurrence_Thursday "*"
    bit recurrence_Friday "*"
    bit recurrence_Saturday "*"
  }
