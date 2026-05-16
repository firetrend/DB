using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class Universe
{
    public int UniverseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
