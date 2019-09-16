insert into Taskmanager_Tables   
 (  
  Date_add,  
  Table_name,  
  Table_description_Esp,  
  Table_documentation_Esp,  
  Table_description_Eng,  
  Table_documentation_Eng  
 )  
 Select getdate(), name, name, null, name, null  
 from   
  sysobjects O  
 where   
  xtype = 'U'  
  and name not in ('sysdiagrams')  
  and not exists   
  (   
   select * from Taskmanager_Tables T   
   where   
   T.Table_name  = O.name  
  )   
  
 insert into Taskmanager_Fields  
 (  
  Table_reference,  
  Date_add,
  Field_name,
  Field_description_Esp,  
  Field_documentation_Esp,  
  Field_description_Eng,  
  Field_documentation_Eng,  
  Data_type,
  Field_length
 )  
 select T.Table_id, getdate(), C.name, C.name, c.name, c.name, c.name, c.type, c.length
 from   
 Taskmanager_Tables T, syscolumns C  
 where   
 C.id = (select O.id from sysobjects O where O.name   
 = convert(varchar,T.Table_name))  
 and not exists   
 (   
  select * from Taskmanager_Fields F   
  where   
  T.Table_id  = F.Table_reference   
  and C.name = F.Field_name  
 )  
 