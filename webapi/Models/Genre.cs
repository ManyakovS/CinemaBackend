using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public partial class Genre
{
    [Key]
    public int GenreID { get; set; }

    public string Name { get; set; }

}
