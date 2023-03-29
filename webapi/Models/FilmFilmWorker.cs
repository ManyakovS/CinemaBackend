using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

[Keyless]
public partial class FilmFilmWorker
{
    public string? JobTitle { get; set; }

    [ForeignKey("film")]
    public int FilmId { get; set; }
    public virtual Film film { get; set; }

    [ForeignKey("filmWorker")]
    public int FilmWorkerId { get; set; }
    public virtual FilmWorker filmWorker { get; set; }
}
