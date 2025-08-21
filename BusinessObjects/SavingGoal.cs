using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SavingGoal
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? CurrentAmount { get; set; }

    public int? GoalAmount { get; set; }

    public DateTime? GoalDate { get; set; }

    public bool? IsCompleted { get; set; }

    public bool? IsDelete { get; set; }

    public double ProgressGoalPercentage
    {
        get
        {
            return (double)(CurrentAmount / GoalAmount * 100);
        }
    }

    public string LeftMoneyGoal
    {
        get
        {
            return ((int)(GoalAmount - CurrentAmount)).ToString();
        }
    }

    public virtual User? User { get; set; }

}
