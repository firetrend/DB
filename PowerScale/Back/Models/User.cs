using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Nickname { get; set; } = null!;

    public int? Rep { get; set; }

    public bool? Adminstatus { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Fight> Fights { get; set; } = new List<Fight>();
}
