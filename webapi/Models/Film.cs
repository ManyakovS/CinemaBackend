using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Film
{
    public int FilmId { get; set; }

    public string Name { get; set; } = null!;

    public int Duration { get; set; }

    public DateTime RentalStartDate { get; set; }

    public DateTime RentalEndtDate { get; set; }

    public string? Director { get; set; }
}
