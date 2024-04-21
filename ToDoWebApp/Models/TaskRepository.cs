using System.Numerics;
using ToDoWebApp.Interfaces;
using ToDoWebApp.Models.DAL;

namespace ToDoWebApp.Models
{
    public class TaskRepository : ITaskRepository
    {
        private ToDoApplicationContext _context;

        public TaskRepository() { 
            _context = new ToDoApplicationContext();
        }
        void ITaskRepository.Add(string title, string description, DateTime dueDate, int userId)
        {
            try
            {
                    var task = new Models.DAL.Task();
                    task.TaskName = title;
                    task.TaskDescription = description;
                    task.CreatedOn = DateTime.Now;
                    task.ForDate = dueDate;
                    task.Completed = 0;
                    task.UserId = userId;

                    _context.Tasks.Add(task);
                    _context.SaveChanges();
                
            }
            catch(Exception ex)
            {

            }
        }

        bool ITaskRepository.Delete(int taskID)
        {
            try
            {
                DAL.Task? task = _context.Tasks.Find(taskID);
                if (task != null)
                {
                    _context.Tasks.Remove(task);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        List<DAL.Task> ITaskRepository.GetAllActiveTasks(int userID)
        {
            return _context.Tasks.Where(x => x.Completed == 0 && x.UserId == userID).ToList();
        }

        List<DAL.Task> ITaskRepository.GetAllCompletedTasks(int userID)
        {
            return _context.Tasks.Where(x => x.Completed == 1 && x.UserId == userID).ToList(); 
        }

        List<DAL.Task> ITaskRepository.GetAllTasks(int userID)
        {
            return _context.Tasks.Where(x => x.UserId == userID).ToList();
        }

        bool ITaskRepository.Update(int taskId, string title, string description, DateTime duedate)
        {
            try
            {
                var task = _context.Tasks.Find(taskId);
                if (task == null)
                {
                    return false;
                }

                task.TaskName = title;
                task.TaskDescription = description;
                task.ForDate = duedate;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool ITaskRepository.UpdateTaskStatus(int taskId, short complete)
        {
            try
            {
                var task = _context.Tasks.Find(taskId);
                if (task == null)
                {
                    return false;
                }

                task.Completed = complete;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        DashboardDTO ITaskRepository.GetDashboardTasks(int userId)
        {
            DashboardDTO dto = new DashboardDTO();
            dto.allTask = _context.Tasks.Where(x => x.UserId == userId).Count();
            dto.completedTask = _context.Tasks.Where(x => x.UserId == userId && x.Completed == 1).Count();
            dto.pendingTask = _context.Tasks.Where(x=>x.UserId == userId && x.Completed == 0).Count();
            return dto;

        }

    }
}
