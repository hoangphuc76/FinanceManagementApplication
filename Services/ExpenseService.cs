using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ExpenseService : IExpenseService
    {
        public ExpenseService() { }
        public void CreateNewBudget(BudgetItem budget) 
            => BudgetItemDAO.CreateNewBudget(budget);

        public void CreateNewExpenseTransaction(ExpenseTransaction expenseTransaction) 
            => ExpenseTransactionDAO.CreateNewTransaction(expenseTransaction);

        public void DeleteBudget(BudgetItem budget) 
            => BudgetItemDAO.DeleteBudget(budget);

        public void DeleteExpenseTransaction(ExpenseTransaction expenseTransaction)
            => ExpenseTransactionDAO.DeleteTransaction(expenseTransaction);

        public BudgetItem GetBudgetItemById(int id)
            => BudgetItemDAO.GetBudgetItemById(id);

        public List<BudgetItem> GetBudgets(int userId)
            => BudgetItemDAO.GetBudgetItems(userId);

        public List<BudgetItem> GetBudgets()
        {
            throw new NotImplementedException();
        }

        public ExpenseTransaction GetExpenseTransactionById(int id)
            => ExpenseTransactionDAO.GetTransactionById(id);

        public List<ExpenseTransaction> GetExpenseTransactions(int userId)
            => ExpenseTransactionDAO.GetExpenseTransactions(userId);

        public List<ExpenseTransaction> GetExpenseTransactions()
        {
            throw new NotImplementedException();
        }

        public void UpdateBudget(BudgetItem budget)
            => BudgetItemDAO.UpdateBudget(budget);

        public void UpdateExpenseTransaction(ExpenseTransaction expenseTransaction)
            => ExpenseTransactionDAO.UpdateTransaction(expenseTransaction);
    }
}
