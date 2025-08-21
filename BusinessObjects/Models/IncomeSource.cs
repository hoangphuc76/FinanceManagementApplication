using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class IncomeSource
{
    public int Id { get; set; }

    public string? SourceName { get; set; }

    public bool? IsDelete { get; set; }
}
