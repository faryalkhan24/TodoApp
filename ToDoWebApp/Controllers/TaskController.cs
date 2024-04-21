using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ToDoWebApp.Models.DAL;

namespace ToDoWebApp.Controllers
{
    public class TaskController : Controller
    {
        private Interfaces.ITaskRepository _taskRepository;

        public TaskController()
        {
            _taskRepository = new Models.TaskRepository();
        }
        public IActionResult Index()
        {
            Models.LoginIdentity loginIdentity = Helper.UserHelper.GetLoginIdentity(HttpContext.Session);
            if (loginIdentity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View();
        }

        public ActionResult GetTasks(string tab)
        {
            List<Models.DAL.Task> tasks = new List<Models.DAL.Task>();
            Models.LoginIdentity loginIdentity = Helper.UserHelper.GetLoginIdentity(HttpContext.Session);
            if (loginIdentity != null)
            {
                switch (tab)
                {
                    case "active":
                        tasks = _taskRepository.GetAllActiveTasks(loginIdentity.UserID);
                        break;
                    case "completed":
                        tasks = _taskRepository.GetAllCompletedTasks(loginIdentity.UserID);
                        break;
                    default:
                        tasks = _taskRepository.GetAllTasks(loginIdentity.UserID);
                        break;
                }

                return PartialView("TaskList", tasks);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public ActionResult CompleteTask(int id, bool completed)
        {
            Models.LoginIdentity loginIdentity = Helper.UserHelper.GetLoginIdentity(HttpContext.Session);
            if (loginIdentity == null)
            {
                return StatusCode(500);
            }
            else
            {
                bool statusUpdated = _taskRepository.UpdateTaskStatus(id, (short)(completed ? 1 : 0));
                if (statusUpdated)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }

        [HttpPost]
        public IActionResult AddTask(string title, string description, string dueDate)
        {
            try
            {
                Models.LoginIdentity loginIdentity = Helper.UserHelper.GetLoginIdentity(HttpContext.Session);
                if (loginIdentity == null)
                {
                    return StatusCode(500);
                }
                else
                {

                    DateTime parsedDate;
                    bool isValid = DateTime.TryParse(dueDate, out parsedDate);
                    _taskRepository.Add(title, description, isValid ? parsedDate : DateTime.Now, loginIdentity.UserID);
                    return StatusCode(200);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpPost]
        public IActionResult UpdateTask(int taskId, string title, string description, string dueDate)
        {
            try
            {
                Models.LoginIdentity loginIdentity = Helper.UserHelper.GetLoginIdentity(HttpContext.Session);
                if (loginIdentity == null)
                {
                    return StatusCode(500);
                }
                else
                {
                    DateTime parsedDate;
                    bool isValid = DateTime.TryParse(dueDate, out parsedDate);
                    bool isUpdated = _taskRepository.Update(taskId, title, description, isValid ? parsedDate : DateTime.Now);
                    if (isUpdated)
                    {
                        return StatusCode(200);
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }


        [HttpPost]
        public IActionResult DeleteTask(int taskID)
        {
            Models.LoginIdentity loginIdentity = Helper.UserHelper.GetLoginIdentity(HttpContext.Session);
            if (loginIdentity == null)
            {
                return StatusCode(500);
            }
            else
            {
               bool isDeleted = _taskRepository.Delete(taskID);
                if (isDeleted)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }

    }
}
