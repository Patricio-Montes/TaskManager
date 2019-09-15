using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using TaskManager.Models;
using WebApiClient;

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Tasks> taskList;

            HttpResponseMessage response = BaseAddress.WebApiClient.GetAsync("Tasks").Result;
            taskList = response.Content.ReadAsAsync<IEnumerable<Tasks>>().Result;
            if (response.IsSuccessStatusCode)
            {
                return View(taskList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = BaseAddress.WebApiClient.DeleteAsync("Tasks/" + id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Se elimino la Tarea " + id.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult PartialAdd(Tasks task)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddTasks", new Tasks());
            }
            else
            {
                return Add(task);
            }
        }

        public ActionResult Add(Tasks task)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            task.State_id = 1;
            response = BaseAddress.WebApiClient.PostAsJsonAsync("Tasks", task).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Se añadio la Tarea ";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult PartialEdit(int id)
        {
            if (id != 0)
            {
                if (Request.IsAjaxRequest())
                {
                    HttpResponseMessage response = BaseAddress.WebApiClient.GetAsync("Tasks/" + id.ToString()).Result;
                    return PartialView("_EditTasks", response.Content.ReadAsAsync<Tasks>().Result);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public ActionResult PartialEdit(Tasks task)
        {
            if (task.Task_id != 0)
            {
                // Provisorio, definir estado edicion y esto realizarlo en el servicio una vez se haya auditado el estado anterior del registro.
                task.State_id = 1;
                HttpResponseMessage response = new HttpResponseMessage();
                response = BaseAddress.WebApiClient.PutAsJsonAsync("Tasks/" + task.Task_id.ToString(), task).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Se edito la Tarea " + task.Task_id.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            return RedirectToAction("Error");
        }

        public JsonResult GetTasksProgress()
        {
            Tasks Tasks = new Tasks();
            List<Tasks_Progress> taskProgress = Tasks.GetTasksProgress(1);
            return Json(taskProgress, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateState(int task_id, int state_id)
        {
            Tasks TaskUpdate = new Tasks();
            if (TaskUpdate.UpdateState(task_id, state_id))
            {
                TempData["SuccessMessage"] = "Se completo la Tarea " + task_id.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}