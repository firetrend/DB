using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class Akasualtype
{
    public int UndyingId { get; set; }

    public string? Description { get; set; }

    public string? Tag { get; set; }

    public int? Type { get; set; }

    public virtual ICollection<Dclass> Dclasses { get; set; } = new List<Dclass>();
}
