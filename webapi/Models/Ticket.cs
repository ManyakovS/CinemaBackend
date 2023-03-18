using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public string State { get; set; } = null!;

    public int SessionId { get; set; }

    public int PlaceId { get; set; }

    public int? EmployeeId { get; set; }
}
