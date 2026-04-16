PRAGMA foreign_keys = ON;

-- =========================
-- TABLE: App
-- =========================
CREATE TABLE IF NOT EXISTS App (
    id_app INTEGER PRIMARY KEY,
    version INTEGER NOT NULL,
    relative_offset_minutes INTEGER NOT NULL,
    snooze_minutes INTEGER NOT NULL
);

-- =========================
-- TABLE: TaskScheduler
-- =========================
CREATE TABLE IF NOT EXISTS TaskScheduler (
    id_taskscheduler INTEGER PRIMARY KEY AUTOINCREMENT,
    time_due_at TEXT NOT NULL,
    time_saved_due_at TEXT,
    recurrence_interval INTEGER NOT NULL,
    recurrence_type INTEGER NOT NULL,
    recurrence_sunday INTEGER DEFAULT 0,
    recurrence_monday INTEGER DEFAULT 0,
    recurrence_tuesday INTEGER DEFAULT 0,
    recurrence_wednesday INTEGER DEFAULT 0,
    recurrence_thursday INTEGER DEFAULT 0,
    recurrence_friday INTEGER DEFAULT 0,
    recurrence_saturday INTEGER DEFAULT 0,
    recurrence_interval_month INTEGER
);

-- =========================
-- TABLE: TaskEvent
-- =========================
CREATE TABLE IF NOT EXISTS TaskEvent (
    id_task_event INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    id_taskscheduler INTEGER NOT NULL,
    FOREIGN KEY (id_taskscheduler) REFERENCES TaskScheduler(id_taskscheduler)
        ON DELETE CASCADE
);

-- =========================
-- TABLE: TaskGroup
-- =========================
CREATE TABLE IF NOT EXISTS TaskGroup (
    id_task_group INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL
);

-- =========================
-- TABLE: Task
-- =========================
CREATE TABLE IF NOT EXISTS Task (
    id_task INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    description TEXT,
    id_task_group INTEGER,
    is_insistent INTEGER NOT NULL,
    datetime_create TEXT,
    date_due_at TEXT NOT NULL,
    date_due_at_last TEXT,
    datetime_notify_at TEXT,
    snooze_minutes INTEGER,
    id_taskscheduler INTEGER,
    id_task_event INTEGER,
    is_active INTEGER NOT NULL,
    FOREIGN KEY (id_task_group) REFERENCES TaskGroup(id_task_group),
    FOREIGN KEY (id_taskscheduler) REFERENCES TaskScheduler(id_taskscheduler),
    FOREIGN KEY (id_task_event) REFERENCES TaskEvent(id_task_event)
);

-- =========================
-- INDEXES
-- =========================
CREATE INDEX IF NOT EXISTS idx_task_due_at ON Task(date_due_at);
CREATE INDEX IF NOT EXISTS idx_task_active ON Task(is_active);
CREATE INDEX IF NOT EXISTS idx_task_notify_at ON Task(datetime_notify_at);
CREATE INDEX IF NOT EXISTS idx_scheduler_due_at ON TaskScheduler(time_due_at);

-- Registro inicial de la aplicación
INSERT INTO App (id_app, version, relative_offset_minutes, snooze_minutes)
VALUES (1, 1, 0, 5);