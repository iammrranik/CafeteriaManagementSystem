using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class WalletTransaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public virtual User User { get; set; } = null!;
}
