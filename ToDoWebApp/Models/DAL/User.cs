using System;
using System.Collections.Generic;

namespace ToDoWebApp.Models.DAL;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserEmail { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
