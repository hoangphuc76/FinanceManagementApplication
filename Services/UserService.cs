using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        public UserService() { }

        public void CreateNewFinanceRecord(FinanceRecord financeRecord)
            => FinanceRecordDAO.CreateNewFinanceRecord(financeRecord);

        public void CreateUser(User user) 
            => UserDAO.CreateNewUser(user);

        public void DeleteFinanceRecord(FinanceRecord financeRecord)
            => FinanceRecordDAO.DeleteFinanceRecord(financeRecord);

        public void DeleteUser(int id) 
            => UserDAO.DeleteUser(id);

        public FinanceRecord GetFinanceRecord(int id, int month, int year)
            => FinanceRecordDAO.GetFinanceRecord(id, month, year);

        public List<FinanceRecord> GetFinanceRecords(int userId)
            => FinanceRecordDAO.GetFinanceRecords(userId);

        public List<FinanceRecord> GetFinanceRecords()
        {
            throw new NotImplementedException();
        }

        public User GetUser(string email, string password) 
            => UserDAO.getUser(email, password);

        public void UpdateFinanceRecord(FinanceRecord financeRecord)
            => FinanceRecordDAO.UpdateFinanceRecord(financeRecord);

        public void UpdateUser(User user) 
            => UserDAO.UpdateUser(user);
            
        public double HandleBalanceComparison(int userId)
        {
            DateTime currentDate = DateTime.Now;
            DateTime prevDate = currentDate.AddMonths(-1);

            int currMonth = currentDate.Month;
            int currYear = currentDate.Year;

            int prevMonth = prevDate.Month;
            int prevYear = prevDate.Year;

            FinanceRecord currRecord = GetFinanceRecord(userId, currMonth, currYear);
            FinanceRecord prevRecord = GetFinanceRecord(userId, prevMonth, prevYear);

            if (prevRecord == null)
            {
                return 0.0;
            }

            int currBalance = (int)(currRecord.TotalIncome - currRecord.TotalExpense);
            int prevBalance = (int)(prevRecord.TotalIncome - prevRecord.TotalExpense);

            if (prevBalance != 0)
            {
                return ((double)(currBalance - prevBalance) / prevBalance);
            }
            else
            {
                return 1.0;
            }
        }

        public double HandleIncomeComparison(int userId)
        {
            DateTime currentDate = DateTime.Now;
            DateTime prevDate = currentDate.AddMonths(-1);

            int currMonth = currentDate.Month;
            int currYear = currentDate.Year;

            int prevMonth = prevDate.Month;
            int prevYear = prevDate.Year;

            FinanceRecord currRecord = GetFinanceRecord(userId, currMonth, currYear);
            FinanceRecord prevRecord = GetFinanceRecord(userId, prevMonth, prevYear);

            if (prevRecord == null)
            {
                return 0.0;
            }

            int currIncome = (int)(currRecord.TotalIncome);
            int prevIncome = (int)(prevRecord.TotalIncome);

            if (prevIncome != 0)
            {
                return ((double)(currIncome - prevIncome) / prevIncome);
            }
            else
            {
                return 1.0;
            }
        }

        public double HandleExpenseComparison(int userId)
        {
            DateTime currentDate = DateTime.Now;
            DateTime prevDate = currentDate.AddMonths(-1);

            int currMonth = currentDate.Month;
            int currYear = currentDate.Year;

            int prevMonth = prevDate.Month;
            int prevYear = prevDate.Year;

            FinanceRecord currRecord = GetFinanceRecord(userId, currMonth, currYear);
            FinanceRecord prevRecord = GetFinanceRecord(userId, prevMonth, prevYear);

            if (prevRecord == null)
            {
                return 0.0;
            }

            int currExpense = (int)(currRecord.TotalExpense);
            int prevExpense = (int)(prevRecord.TotalExpense);

            if (prevExpense != 0)
            {
                return ((double)(currExpense - prevExpense) / prevExpense);
            }
            else
            {
                return 1.0;
            }
        }

        public List<BudgetItem> GetBudgetItems(int userId)
            => BudgetItemDAO.GetBudgetItems(userId);
            
        public User GetUser(string email)
            => UserDAO.getUser(email);

        public FinanceRecord GetFinanceRecordByUserId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
