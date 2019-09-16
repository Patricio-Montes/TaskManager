-- Diccionario de datos. Tablas del sistema. 

CREATE TABLE Taskmanager_Tables
( Table_id int IDENTITY (1, 1) NOT NULL,
  Date_add datetime,
  Table_name varchar(40),
  Table_description_Esp varchar(100),
  Table_documentation_Esp varchar(max),
  Table_description_Eng varchar(100),
  Table_documentation_Eng varchar(max),
  
  CONSTRAINT TaskmanagerTables_pk PRIMARY KEY (Table_id)
);

-- Diccionario de datos. Campos. 

CREATE TABLE Taskmanager_Fields
( Field_id int IDENTITY (1, 1) NOT NULL,
  Table_reference int CONSTRAINT FK_Taskmanager_Tables FOREIGN KEY (Table_reference) REFERENCES Taskmanager_Tables(Table_id),
  Date_add datetime,
  Field_name varchar(40),
  Field_description_Esp varchar(100),
  Field_documentation_Esp varchar(max),
  Field_description_Eng varchar(100),
  Field_documentation_Eng varchar(max),
  Field_pk varchar(40),
  Data_type varchar(20),
  Field_length int,
  Allow_nulls char(1),
  
  CONSTRAINT Taskmanager_Fields_pk PRIMARY KEY (Field_id)
);

-- Tabla de Usuarios. Pensando en que la administración de tareas, tipicamente podria contener asignación de responsables.
-- Ademas, a los usuarios involucrados en esta tabla, se le registrarian se le asociarian las operaciones registradas en la base de auditoria. 

CREATE TABLE Users
( User_id int IDENTITY (1, 1) NOT NULL,
  Date_add datetime,
  Date_last_upd datetime,
  User_add int,
  User_last_upd int,
  User_name varchar(50),
  Usr_password varchar(20),
  Name varchar(50),
  Surname varchar(50),
  Email_adrress varchar(60),
  
  CONSTRAINT Users_pk PRIMARY KEY (User_id)
);


--insert into Users values(getdate(), getdate(), 1, 1, 'admin', 'admin', 'Administrador', 'Administrador', 'admin@gmail.com')


-- Tabla de prioridades. Define la "urgencia" de una tarea. Ejemplo: Alta, Baja, Media, etc 
CREATE TABLE Priorities_Tasks
( Priority_id int IDENTITY (1, 1) NOT NULL,
  Date_add datetime,
  Date_last_upd datetime,
  --User_add int CONSTRAINT FK_UserAddPriority FOREIGN KEY (User_add) REFERENCES Users(User_id),
  --User_last_upd int CONSTRAINT FK_UserLastUpdPriority FOREIGN KEY (User_last_upd) REFERENCES Users(User_id),
  User_add int,
  User_last_upd int,
  Priority_description varchar(60),
  
  CONSTRAINT Priorities_Tasks_pk PRIMARY KEY (Priority_id)
);

insert into Priorities_Tasks values(getdate(), getdate(), 1, 1, 'Alta')
insert into Priorities_Tasks values(getdate(), getdate(), 1, 1, 'Media')
insert into Priorities_Tasks values(getdate(), getdate(), 1, 1, 'Baja')

-- Tabla Estados de tareas. Se define pensando en la escalabilidad del sitio. 
-- Se considera que una tarea puede tener estados tipicos tales como: pendiente de realización ,pendiente de asignacion, asignada/reasignada, completada, rechazada, etc.
CREATE TABLE States_Tasks
( State_id int IDENTITY (1, 1) NOT NULL,
  Date_add datetime,
  Date_last_upd datetime,
  --User_add int CONSTRAINT FK_UserAddStates FOREIGN KEY (User_add) REFERENCES Users(User_id),
  --User_last_upd int CONSTRAINT FK_UserLastUpdStates FOREIGN KEY (User_last_upd) REFERENCES Users(User_id),
  User_add int,
  User_last_upd int,
  State_description varchar(60),
  
  CONSTRAINT States_Tasks_pk PRIMARY KEY (State_id)
);

insert into States_Tasks values(getdate(), getdate(), 1, 1, 'Pendiente')
insert into States_Tasks values(getdate(), getdate(), 1, 1, 'Asignada')
insert into States_Tasks values(getdate(), getdate(), 1, 1, 'Completada')
insert into States_Tasks values(getdate(), getdate(), 1, 1, 'Rechazada')

-- Tabla de tareas. Tabla principal del sitio.  
CREATE TABLE Tasks
( Task_id int IDENTITY (1, 1) NOT NULL,
  Task_Title varchar(100),
  Task_description varchar(250),
  User_respon int CONSTRAINT FK_UserResponTasks FOREIGN KEY (user_respon) REFERENCES Users(user_id),
  Priority_id int CONSTRAINT FK_ProrityTasks FOREIGN KEY (priority_id) REFERENCES Priorities_Tasks(priority_id),
  Expiration_date datetime,
  State_id int, CONSTRAINT FK_StatesTasks FOREIGN KEY (state_id) REFERENCES States_Tasks(state_id),
  
  CONSTRAINT Tasks_pk PRIMARY KEY (task_id)
);


-- Tabla de auditoria. Registro de las operaciones asociada a las tareas

CREATE TABLE Audit_Tasks
(
	Audit_id int IDENTITY(1,1) NOT NULL,
	Nro_movement int NULL,
	Operation_type char(1) NULL,
	Operation_date datetime NULL,
	User_add int NULL,
	Task_id int NULL,
	Task_Title varchar(100) NULL,
	Task_description varchar(250) NULL,
	User_respon int NULL,
	Priority_id int NULL,
	Expiration_date datetime NULL,
	State_id int NULL,

	CONSTRAINT Audit_Tasks_pk PRIMARY KEY (Audit_id)
);


-- Tabla de progreso. Es insertada por el store GetTasksProgressByUser, y se utiliza para obtener desde la API el progreso de tareas por usuario
CREATE TABLE Tasks_Progress
(
	Id int IDENTITY(1,1) NOT NULL,
	State_id int NULL,
	State_description varchar(60) NULL,
	Percentage int null,
	UserID int NULL,

	CONSTRAINT Tasks_Progress_pk PRIMARY KEY (Id)
);








