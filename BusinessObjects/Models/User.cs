using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public int? Balance { get; set; }

    public string? AvatarPath { get; set; }
    
    public bool IsPro { get; set; } = false;

    public virtual ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();

    public virtual ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();
}
