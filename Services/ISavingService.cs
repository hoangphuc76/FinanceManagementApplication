using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISavingService
    {   
        // function for saving goal
        void CreateSavingGoal(SavingGoal savingGoal);
        void UpdateSavingGoal(SavingGoal savingGoal);
        void DeleteSavingGoal(SavingGoal savingGoal);
        List<SavingGoal> GetSavingGoals(int userId);
        SavingGoal GetSavingGoalById(int id);
        (List<string> labels, List<double> series) Labels(int usreid);

        List<MonthlyExpense> GetMonthlyExpenses(int userId);

        //function for saving transaction
        void CreateSavingTransaction(SavingTransaction savingTransaction);
        void UpdateSavingTransaction(SavingTransaction savingTransaction);
        void DeleteSavingTransaction(SavingTransaction savingTransaction);
        List<SavingTransaction> GetSavingTransactions(int userId);
        SavingTransaction GetSavingTransactionById(int id);
    }
}
