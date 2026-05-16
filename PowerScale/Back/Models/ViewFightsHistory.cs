using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class ViewFightsHistory
{
    public int? FightsId { get; set; }

    public string? Fighter1 { get; set; }

    public string? Fighter2 { get; set; }

    public string? Fightexodus { get; set; }

    public string? Winner { get; set; }

    public string? ProposedBy { get; set; }
}
