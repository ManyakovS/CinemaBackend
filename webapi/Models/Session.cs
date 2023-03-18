using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public TimeSpan TimeEnd { get; set; }

    public int? FilmId { get; set; }

    public int? CinemaHallId { get; set; }
}
