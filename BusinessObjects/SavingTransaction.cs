using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SavingTransaction
{
    public int? Id { get; set; }
    public int? UserId { get; set; }

    public int? SavingGoalId { get; set; }

    public string? Note { get; set; }

    public int Amount { get; set; }

    public DateTime? Date { get; set; }

    public virtual SavingGoal? SavingGoal { get; set; }

    public virtual User? User { get; set; }
}
