using ToDoWebApp.Interfaces;
using ToDoWebApp.Models.DAL;

namespace ToDoWebApp.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoApplicationContext _context;

        public UserRepository()
        {
            _context = new ToDoApplicationContext();
        }

        bool IUserRepository.CreateUser(string username, string email, string password)
        {
            try
            {
                User user = new User();
                user.UserName = username;
                user.UserEmail = email;
                user.Password = password;
                _context.Users.Add(user);
                _context.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        User IUserRepository.GetUser(string username)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == username);
        }

        bool IUserRepository.IsUserExists(string username)
        {
           return _context.Users.Any(x => x.UserName == username);
        }
    }
}
