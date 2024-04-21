using ToDoWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace ToDoWebApp.Helper
{
    public static class UserHelper
    {
        public static LoginIdentity GetLoginIdentity(ISession session)
        {
            LoginIdentity loginIdentity = null;

            string loggedInUserName = session.GetString("LoggedInUsername") ?? string.Empty;
            int loggedInUser = session.GetInt32("LoggedInUserID") ?? 0;
            if(loggedInUser != 0 || !string.IsNullOrEmpty(loggedInUserName))
            {
                loginIdentity = new LoginIdentity();
                loginIdentity.UserID = loggedInUser;
                loginIdentity.UserName = loggedInUserName;
            }

            return loginIdentity;
        }
    }
}
