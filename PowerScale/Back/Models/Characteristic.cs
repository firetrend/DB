using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class Characteristic
{
    public int CharacteristicsId { get; set; }

    public int? Speed { get; set; }

    public int? Strenght { get; set; }

    public int? Iq { get; set; }

    public int? Battleiq { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
