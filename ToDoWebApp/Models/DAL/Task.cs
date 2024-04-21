using System;
using System.Collections.Generic;

namespace ToDoWebApp.Models.DAL;

public partial class Task
{
    public int TaskId { get; set; }

    public string? TaskName { get; set; }

    public string? TaskDescription { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ForDate { get; set; }

    public short? Completed { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
