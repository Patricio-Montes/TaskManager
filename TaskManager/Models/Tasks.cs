using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace TaskManager.Models
{
    public partial class Tasks
    {
        public Tasks()
        {
            Priorities = GetPriorities();
            Responsible = GetUsersResponsible();
        }

        public List<Tasks_Progress> GetTasksProgress(int userID)
        {
            Tasks_Progress TaskProgress = new Tasks_Progress();
            return TaskProgress.GetProgress(userID);
        }

        private List<Priorities_Tasks> GetPriorities()
        {
            Priorities_Tasks PrioritieList = new Priorities_Tasks();
            return PrioritieList.GetList();
        }

        private List<User> GetUsersResponsible()
        {
            User UserList = new User();
            return UserList.GetList();
        }

        [Display(Name = "Nro. Tarea")]
        public int Task_id { get; set; }
        [Required(ErrorMessage = "El título es requerido")]
        [Display(Name = "Título")]
        public string Task_Title { get; set; }
        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string Task_description { get; set; }
        [Display(Name = "Usuario Responsable")]
        public Nullable<int> User_respon { get; set; }
        [Display(Name = "Prioridad")]
        public Nullable<int> Priority_id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Vencimiento")]
        public Nullable<System.DateTime> Expiration_date { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> State_id { get; set; }


        public List<Priorities_Tasks> Priorities = new List<Priorities_Tasks>();
        public List<User> Responsible = new List<User>();
        public List<Tasks_Progress> Progress = new List<Tasks_Progress>();

        public bool UpdateState(int task_id, int state_id)
        {
            string strSQL = "update Tasks set State_id = " + state_id + " where Task_id = " + task_id;
            using (SqlConnection Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TaskBackEnd"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(strSQL, Connection);
                    Connection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}