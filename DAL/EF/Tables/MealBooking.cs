using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class MealBooking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MenuItemId { get; set; }

    public int OrderedQuantity { get; set; }

    public double TotalPrice { get; set; }

    public DateTime BookingDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
