using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public partial class Ticket
{
    [Key]
    public int TicketId { get; set; }

    public string State { get; set; } = null!;

    [ForeignKey("session")]
    public int SessionId { get; set; }
    public virtual Session session { get; set; }

    [ForeignKey("place")]
    public int PlaceId { get; set; }
    public virtual Place place { get; set; }

    [ForeignKey("employee")]
    public int? EmployeeId { get; set; }
    public virtual Employee employee { get; set; }

    [ForeignKey("user")]
    public long UserId { get; set; } = 0;
    public virtual User user { get; set; }
}
