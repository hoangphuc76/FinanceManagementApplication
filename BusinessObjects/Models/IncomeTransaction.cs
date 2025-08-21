using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class IncomeTransaction
{
    public int? UserId { get; set; }

    public int? SourceId { get; set; }

    public int? Amount { get; set; }

    public DateOnly? Date { get; set; }

    public virtual IncomeSource? Source { get; set; }

    public virtual User? User { get; set; }
}
