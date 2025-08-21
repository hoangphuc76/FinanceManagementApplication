using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SavingTransaction
{
    public int? UserId { get; set; }

    public int? SavingGoalId { get; set; }

    public string? Note { get; set; }

    public int Amount { get; set; }

    public DateOnly? Date { get; set; }

    public virtual SavingGoal? SavingGoal { get; set; }

    public virtual User? User { get; set; }
}
