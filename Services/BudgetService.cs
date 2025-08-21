using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BudgetService : IBudgetService
    {
        public List<BudgetItem> GetCurrentMonthBudgets(int userId)
        {
            return BudgetItemDAO.GetBudgetItems(userId);
        }

        public (string TotalExpense, int TotalExpensePercent, string TotalLimit) GetExpenseInfor(List<BudgetItem> budgets)
        {
            int totalExpense = 0;
            int limitBudget = 0;

            foreach (var item in budgets)
            {
                totalExpense += (int)item.CurrentAmount;
                limitBudget += (int)item.LimitAmount;
            }

            int totalExpensePercent = limitBudget != 0 ? (int)((float)totalExpense / limitBudget * 100) : 0;

            return (
                Util.Instance.FormatMoney(totalExpense), 
                totalExpensePercent,
                Util.Instance.FormatMoney(limitBudget)
            );
        }

    }
}
