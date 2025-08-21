using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ExpenseTransaction
{
    public int? UserId { get; set; }

    public int? BudgetId { get; set; }

    public string? Note { get; set; }

    public decimal Amount { get; set; }

    public DateOnly? Date { get; set; }

    public virtual BudgetItem? Budget { get; set; }

    public virtual User? User { get; set; }
}
