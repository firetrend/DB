using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class Fight
{
    public int? Character1Id { get; set; }

    public int? Character2Id { get; set; }

    public int? UserId { get; set; }

    public string? Fightexodus { get; set; }

    public int FightsId { get; set; }

    public virtual Character? Character1 { get; set; }

    public virtual Character? Character2 { get; set; }

    public virtual User? User { get; set; }
}
