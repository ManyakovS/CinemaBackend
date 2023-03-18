using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string LastName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? SecondName { get; set; }

    public string? JobTitle { get; set; }
}
