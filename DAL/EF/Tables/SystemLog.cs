using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class SystemLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
