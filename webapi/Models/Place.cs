using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public partial class Place
{
    [Key]
    public int PlaceId { get; set; }

    public int NumberOfPlace { get; set; }
    public int MumberOfRow { get; set; }

    [ForeignKey("cinemaHall")]
    public int CinemaHallId { get; set; }
    public virtual CinemaHall cinemaHall { get; set; }
}
