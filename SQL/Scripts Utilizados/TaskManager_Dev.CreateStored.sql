      
-- =============================================      
-- Author: Patricio Montes Güemez      
-- Create date: 13/9/2019      
-- Description: Stored Procedure Responsable de insertar en Tasks_Progress el progreso de tareas por usuario      
-- =============================================      
CREATE PROCEDURE [dbo].[GetTasksProgressByUser]       
 @userid int      
AS      
BEGIN      
       
 SET NOCOUNT ON;      
      
 declare @tottasks int
 declare @comporpend int
 declare @eliminadas int
 
 set @comporpend = (select count(*) from Tasks with (nolock) where Tasks.User_respon = @userid)

 set @eliminadas = (select count(*) from Audit_Tasks with (nolock)
					where  Audit_Tasks.User_respon = @userid and Audit_Tasks.Operation_type = 'D')

 -- El total de tareas por usuario de compone de las eliminadas + las completadas y pendientes
 set @tottasks = @eliminadas + @comporpend
 
    
 -- Se establece en 1 si no exiten aun tareas, porque en las proximas querys produciria error matemático al intentar dividir por cero.     
 if @tottasks = 0    
 begin    
 set @tottasks = 1    
 end    
      
 -- Delete de progreso por usuario      
 delete Tasks_Progress where UserID = @userid      
      
 -- Insert del progreso del usuario recibido.       
 insert into Tasks_Progress      
      
 -- % de Tareas pendientes por usuario.       
 select 1 Id_State, isnull(max(States_Tasks.State_description), 'Pendiente') State_description, isnull((count(*) * 100)/@tottasks, 0) Percentage, @userid UserID             
 from Tasks with (nolock) left outer join States_Tasks      
 on Tasks.State_id = States_Tasks.State_id      
 where User_respon = @userid and Tasks.State_id = 1      
      
 union       
      
 --% de Tareas completadas por usuario      
 select 2 Id_State, 'Completada' State_description, isnull((count(*) * 100)/@tottasks, 0) Percentage, @userid UserID      
 from Tasks with (nolock)   
 where Tasks.User_respon = @userid and Tasks.State_id = 2       
      
 union       
      
 --% de Tareas eliminadas por usuario      
 select 999 Id_State, 'Eliminada' State_description, isnull((count(*) * 100)/@tottasks, 0) Percentage, @userid UserID      
 from Audit_Tasks with (nolock)       
 where Audit_Tasks.User_add = @userid and Operation_type = 'D'      
          
 ---- Retorno       
 select * from Tasks_Progress      
END 