﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    public class TasksController : ApiController
    {
        private readonly TaskManagerApiEntities db = new TaskManagerApiEntities();

        public List<Task> GetTasks()
        {
            // Se realiza un union para recuperar las tareas eliminadas. Se encuentran auditadas en la tabla Audit_Tasks. 
            var taskList = db.Tasks.SqlQuery(@"select Task_id, Task_Title, Task_description, User_respon, Priority_id, Expiration_date, State_id
                                               from Tasks with (nolock)
                                               where User_respon = 1         
                                               union
                                               select Task_id, Task_Title, Task_description, User_respon, Priority_id, Expiration_date, State_id
                                               from Audit_Tasks with (nolock)
                                               where Audit_Tasks.User_respon = 1 and Audit_Tasks.Operation_type = 'D'").ToList();
            return taskList;
        }

        public IQueryable<Task> GetTasks(int userID, int stateID)
        {
            // Tareas por usuario y por estado. Se utiliza para excluir de la lista de tareas pendientes las completadas (Estado 2)
            return db.Tasks.Where(a => a.User_respon == userID && a.State_id != stateID);
        }

        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Task_id)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                TaskAudit(task, "E");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Tasks.Add(task);
            db.SaveChanges();
            TaskAudit(task, "C");

            return CreatedAtRoute("DefaultApi", new { id = task.Task_id }, task);
        }

        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            db.Tasks.Remove(task);
            try
            {
                db.SaveChanges();
                TaskAudit(task, "D");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Task_id == id) > 0;
        }

        // Metodo responsable de preparar e insertar el objeto auditoria.  
        private void TaskAudit(Task task, string operationtype)
        {
            if (task != null && !string.IsNullOrEmpty(operationtype))
            {
                Audit_Tasks oAudit = new Audit_Tasks
                {
                    Operation_type = operationtype,
                    Operation_date = DateTime.Now,
                    // Para el ejemplo se define hardcode al usuario admin. En un caso real deberia tomarse de sesión. 
                    User_add = 1,
                    Task_id = task.Task_id,
                    Task_Title = task.Task_Title,
                    // Se identifica el último nro de movimiento generado para la tarea 
                    Nro_movement = db.Audit_Tasks.Where(a => a.Task_id == task.Task_id).Max(a => a.Nro_movement) ?? 0 + 1,
                    User_respon = task.User_respon,
                    Task_description = task.Task_description,
                    Priority_id = task.Priority_id,
                    State_id = task.State_id,
                    Expiration_date = task.Expiration_date
                };

                db.Audit_Tasks.Add(oAudit);
                db.SaveChanges();
            }
        }
    }
}