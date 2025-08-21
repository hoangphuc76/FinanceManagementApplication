using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class FinanceRecord
{
    public int? UserId { get; set; }

    public int? TotalIncome { get; set; }

    public int? TotalExpense { get; set; }

    public DateOnly? From { get; set; }

    public DateOnly? To { get; set; }

    public virtual User? User { get; set; }
}
