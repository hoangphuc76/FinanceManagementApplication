using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class IncomeTransaction

{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? SourceId { get; set; }

    public int? Amount { get; set; }

    public DateTime? Date { get; set; }

    public virtual IncomeSource? Source { get; set; }

    public virtual User? User { get; set; }
}
