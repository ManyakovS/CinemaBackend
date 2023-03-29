using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

[Keyless]
public partial class FilmGenre
{
    [ForeignKey("film")]
    public int FilmId { get; set; }
    public virtual Film film { get; set; }

    [ForeignKey("genre")]
    public int GenreId { get; set; }
    public virtual Genre genre { get; set; }
}
