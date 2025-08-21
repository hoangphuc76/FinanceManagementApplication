using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBudgetService
    {
        List<BudgetItem> GetCurrentMonthBudgets(int userId);

        (string TotalExpense, int TotalExpensePercent, string TotalLimit) GetExpenseInfor(List<BudgetItem> budgets);
    }
}
