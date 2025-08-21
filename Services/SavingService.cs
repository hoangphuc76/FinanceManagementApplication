using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SavingService : ISavingService
    {
        Dictionary<int, string> months = new Dictionary<int, string>()
        {
            { 1, "Jan" },
            { 2, "Feb" },
            { 3, "Mar" },
            { 4, "Apr" },
            { 5, "May" },
            { 6, "Jun" },
            { 7, "Jul" },
            { 8, "Aug" },
            { 9, "Sep" },
            { 10, "Oct" },
            { 11, "Nov" },
            { 12, "Dec" }
        };
        public SavingService() { }

        public void CreateSavingGoal(SavingGoal savingGoal)
            => SavingGoalDAO.CreateNewSavingGoal(savingGoal);

        public void CreateSavingTransaction(SavingTransaction savingTransaction)
            => SavingTransactionDAO.CreateNewSavingTransaction(savingTransaction);

        public void DeleteSavingGoal(SavingGoal savingGoal)
            => SavingGoalDAO.DeleteSavingGoal(savingGoal);

        public void DeleteSavingTransaction(SavingTransaction savingTransaction)
            => SavingTransactionDAO.DeleteSavingTransaction(savingTransaction);

        public List<MonthlyExpense> GetMonthlyExpenses(int userId)
            => SavingGoalDAO.GetMonthlyExpenses(userId);

        public SavingGoal GetSavingGoalById(int id)
            => SavingGoalDAO.GetSavingGoalById(id);

        public List<SavingGoal> GetSavingGoals(int userId)
            => SavingGoalDAO.GetSavingGoals(userId);

        public SavingTransaction GetSavingTransactionById(int id)
            => SavingTransactionDAO.GetSavingTransactionById(id);

        public List<SavingTransaction> GetSavingTransactions(int userId)
            => SavingTransactionDAO.GetSavingTransactions(userId);

        public (List<string> labels, List<double> series) Labels(int userId)
        {
            // lbels 
            // boduebl
            List<MonthlyExpense> expenses = GetMonthlyExpenses(userId);
            int month = expenses.Last().Month;
            List<string> monthLabels = new List<string>();
            List<double> totalAmount = new List<double>();
            for(int i = 1; i <= month; i++)
            {
                monthLabels.Add($"{months[i]}");
                var monthlyExpenses = expenses.FirstOrDefault(m => m.Month == i);
                if(monthlyExpenses != null)
                {
                    totalAmount.Add(monthlyExpenses.TotalCurrentAmount);
                }
                else
                {
                    totalAmount.Add(0);
                }
            }
            return (monthLabels, totalAmount);
        }

        public void UpdateSavingGoal(SavingGoal savingGoal)
            => SavingGoalDAO.UpdateSavingGoal(savingGoal);

        public void UpdateSavingTransaction(SavingTransaction savingTransaction)
            => SavingTransactionDAO.UpdateSavingTransaction(savingTransaction);
            
        public SavingGoal GetCurrentTotalSavingGoalAndTotalGoalAmount(int userId, DateTime currentDate)
        => SavingGoalDAO.GetCurrentTotalSavingGoalAndTotalGoalAmount(userId, currentDate);
    }
}
