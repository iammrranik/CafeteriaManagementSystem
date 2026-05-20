using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string IdCardNo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int UserTypeId { get; set; }

    public double WalletBalance { get; set; }

    public string? ProfileStatus { get; set; }

    public virtual ICollection<MealBooking> MealBookings { get; set; } = new List<MealBooking>();

    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();

    public virtual UserType UserType { get; set; } = null!;

    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
}
