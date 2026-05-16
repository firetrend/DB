using System;
using System.Collections.Generic;

namespace PowerScale.Back.Models;

public partial class ViewUserRating
{
    public int? UserId { get; set; }

    public string? Nickname { get; set; }

    public int? Rep { get; set; }

    public bool? Adminstatus { get; set; }

    public long? TotalFights { get; set; }

    public string? UserStatus { get; set; }
}
