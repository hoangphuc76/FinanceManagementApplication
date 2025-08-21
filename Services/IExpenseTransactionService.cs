using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IExpenseTransactionService
    {
        void CreateExpenseTransaction(ExpenseTransaction transaction);
        void UpdateExpenseTransaction(ExpenseTransaction transaction);
        void DeleteExpenseTransaction(int id);
        ExpenseTransaction? GetExpenseTransaction(int id);
        IEnumerable<ExpenseTransaction> GetAllExpenseTransactions();
    }
}
