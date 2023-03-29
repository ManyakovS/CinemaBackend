using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public partial class Assets
{
    [Key]
    public long AssetsId { get; set; }

    public string Description { get; set; } = null!;
    public string Link { get; set; } = "";

    [ForeignKey("film")]
    public int FilmId { get; set; }
    public virtual Film film { get; set; }
}
