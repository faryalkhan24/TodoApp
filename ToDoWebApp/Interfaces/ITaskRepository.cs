using ToDoWebApp.Models;

namespace ToDoWebApp.Interfaces
{
    public interface ITaskRepository
    {
        List<Models.DAL.Task> GetAllTasks(int userID);
        List<Models.DAL.Task> GetAllCompletedTasks(int userID);
        List<Models.DAL.Task> GetAllActiveTasks(int userID);
        bool UpdateTaskStatus(int taskId, short complete);
        void Add(string title, string description, DateTime dueDate, int userId);
        bool Update(int taskId, string title, string description, DateTime duedate);
        bool Delete(int taskID);
        DashboardDTO GetDashboardTasks(int userId);
    }
}
