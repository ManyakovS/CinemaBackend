using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public partial class FilmWorker
{
    [Key]
    public int FilmWorkerId { get; set; }

    public string LastName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? SecondName { get; set; }


}
