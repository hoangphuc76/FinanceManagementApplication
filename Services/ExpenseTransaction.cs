using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ExpenseTransactionService : IExpenseTransactionService
    {
        public void CreateExpenseTransaction(ExpenseTransaction transaction)
        {
            ExpenseTransactionDAO.CreateExpenseTransaction(transaction);
        }

        public void UpdateExpenseTransaction(ExpenseTransaction transaction)
        {
            ExpenseTransactionDAO.UpdateExpenseTransaction(transaction);
        }

        public void DeleteExpenseTransaction(int id)
        {
            ExpenseTransactionDAO.DeleteExpenseTransaction(id);
        }

        public ExpenseTransaction? GetExpenseTransaction(int id)
        {
            return ExpenseTransactionDAO.GetExpenseTransaction(id);
        }

        public IEnumerable<ExpenseTransaction> GetAllExpenseTransactions()
        {
            return ExpenseTransactionDAO.GetAllExpenseTransactions();
        }

        public IEnumerable<ExpenseTransaction> GetAllExpenseTransactionsById(int id)
        {
            return ExpenseTransactionDAO.GetAllExpenseTransactionsById(id);
        }
    }
}
