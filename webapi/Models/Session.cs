using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public partial class Session
{
    [Key]
    public int SessionId { get; set; }

    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public TimeSpan TimeEnd { get; set; }

    [ForeignKey("film")]
    public int? FilmId { get; set; }
    public virtual Film film { get; set; }

    [ForeignKey("cinemaHall")]
    public int? CinemaHallId { get; set; }
    public virtual CinemaHall cinemaHall { get; set; }
}
