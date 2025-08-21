using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class ExpenseTransaction
{
    public int Id { get; set; }
    public int? UserId { get; set; }

    public int? BudgetId { get; set; }

    public string? Note { get; set; }

    public decimal Amount { get; set; } 

    public DateTime? Date { get; set; }

    public virtual BudgetItem? Budget { get; set; }

    public virtual User? User { get; set; }
    public string AmountStr => Util.Instance.FormatMoney(Amount);
    public override string ToString()
    {
        return $"ExpenseTransaction: Id={Id}, UserId={UserId}, BudgetId={BudgetId}, Note='{Note}', Amount={Amount}, Date={Date?.ToShortDateString()}, BudgetName={Budget?.BudgetName}, UserName={User?.Username}";
    }

}
