using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class ViewCharacterFull
{
    public int? CharacterId { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public int? Powerscale { get; set; }

    public string? Tier { get; set; }

    public string? Skills { get; set; }

    public string? UniverseName { get; set; }

    public string? UniverseType { get; set; }

    public int? Speed { get; set; }

    public int? Strenght { get; set; }

    public int? Iq { get; set; }

    public int? Battleiq { get; set; }

    public int? Durability { get; set; }

    public string? AkasualTag { get; set; }

    public int? AkasualType { get; set; }
}
