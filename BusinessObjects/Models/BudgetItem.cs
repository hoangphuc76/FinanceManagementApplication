using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BudgetItem
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? BudgetName { get; set; }

    public int? LimitAmount { get; set; }

    public bool? IsOverBudget { get; set; }

    public bool? IsDelete { get; set; }

    public virtual User? User { get; set; }
}
