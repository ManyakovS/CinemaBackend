using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapi.Data;

namespace webapi.Models;

public partial class Session
{
    public Session()
    {
        
    }
    public Session(string _Date, string _Time, string _TimeEnd, int _FilmId, int _CinemaHallId)
    {
        Date = DateTime.Parse(_Date);
        Time = TimeSpan.Parse(_Time);
        TimeEnd = TimeSpan.Parse(_TimeEnd);
        FilmId = _FilmId;
        CinemaHallId = _CinemaHallId;
    }
    public Session(string _Date, string _Time, string _TimeEnd)
    {
        Date = DateTime.Parse(_Date);
        Time = TimeSpan.Parse(_Time);
        TimeEnd = TimeSpan.Parse(_TimeEnd);
    }



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
