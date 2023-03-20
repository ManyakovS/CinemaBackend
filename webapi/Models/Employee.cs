using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public partial class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    public string LastName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? SecondName { get; set; }
    public string? JobTitle { get; set; }

    [ForeignKey("user")]
    public long UserId { get; set; }
    public virtual User user { get; set; }
}
