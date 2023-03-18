using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class CinemaHall
{
    public int CinemaHallId { get; set; }

    public string? Name { get; set; }

    public int NumberOfSeats { get; set; }
}
