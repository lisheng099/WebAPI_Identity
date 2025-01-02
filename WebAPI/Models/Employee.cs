using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Employee
{
    public Guid EmployeeId { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<News> NewsInsertEmployees { get; set; }
    public virtual ICollection<News> NewsUpdateEmployees { get; set; }
}
