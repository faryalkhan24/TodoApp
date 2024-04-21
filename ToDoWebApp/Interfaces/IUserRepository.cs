namespace ToDoWebApp.Interfaces
{
    public interface IUserRepository
    {
        bool IsUserExists(string username);

        bool CreateUser(string username, string email, string password);

        Models.DAL.User GetUser(string username);
    }
}
