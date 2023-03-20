using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

public partial class CinemaHall
{
    [Key]
    public int CinemaHallId { get; set; }

    public string? Name { get; set; }
    public int NumberOfSeats { get; set; }
}
