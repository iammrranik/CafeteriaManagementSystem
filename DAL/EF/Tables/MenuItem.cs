using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class MenuItem
{
    public int Id { get; set; }

    public string ItemName { get; set; } = null!;

    public double Price { get; set; }

    public string Category { get; set; } = null!;

    public int AvailableQuantity { get; set; }

    public virtual ICollection<MealBooking> MealBookings { get; set; } = new List<MealBooking>();
}
