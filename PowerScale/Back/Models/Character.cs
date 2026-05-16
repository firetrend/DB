using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class Character
{
    public string Name { get; set; } = null!;

    public string? Skills { get; set; }

    public int? Powerscale { get; set; }

    public string? Description { get; set; }

    public int? Age { get; set; }

    public int? DclassId { get; set; }

    public int? CharacteristicsId { get; set; }

    public int? UniverseId { get; set; }

    public int CharacterId { get; set; }

    public virtual Characteristic? Characteristics { get; set; }

    public virtual Dclass? Dclass { get; set; }

    public virtual ICollection<Fight> FightCharacter1s { get; set; } = new List<Fight>();

    public virtual ICollection<Fight> FightCharacter2s { get; set; } = new List<Fight>();

    public virtual Universe? Universe { get; set; }
}
