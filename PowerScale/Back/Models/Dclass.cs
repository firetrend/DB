using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class Dclass
{
    public int DclassId { get; set; }

    public int? Durability { get; set; }

    public int? UndyingId { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual Akasualtype? Undying { get; set; }
}
