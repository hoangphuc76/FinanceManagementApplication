using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class IncomeSource
{
    public int Id { get; set; }

    public string? SourceName { get; set; }

    public bool? IsDelete { get; set; }
}
