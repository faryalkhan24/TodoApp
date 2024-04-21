using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Diagnostics;
using ToDoWebApp.Models;
using ToDoWebApp.Models.DAL;
using ToDoWebApp.ViewModels;

namespace ToDoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private Interfaces.ITaskRepository _taskRepository;

        public HomeController()
        {
            _taskRepository = new TaskRepository();
        }

        public IActionResult Index()
        {
            try
            {
                LoginIdentity loginId = Helper.UserHelper.GetLoginIdentity(HttpContext.Session);
                
                if (loginId!= null)
                {
                    DashboardDTO dto =_taskRepository.GetDashboardTasks(loginId.UserID);
                    DashboardViewModel dashboardViewModel = new DashboardViewModel();
                    dashboardViewModel.UserName = loginId.UserName;
                    dashboardViewModel.TotalTask = dto.allTask;
                    dashboardViewModel.CompletedTasks = dto.completedTask;
                    dashboardViewModel.PendingTasks = dto.pendingTask;
                    return View(dashboardViewModel);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", "Home");
                
            }
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
