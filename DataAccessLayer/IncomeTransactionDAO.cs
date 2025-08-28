using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class IncomeTransactionDAO
    {
        public static List<IncomeTransaction> GetIncomeTransactions(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var IncomeTransactions = context.IncomeTransactions
                                        .Include(t => t.User)
                                        .Include(t => t.Source) 
                                        .Where(t => t.UserId == userId)
                                        .ToList();
                return IncomeTransactions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewIncomeTransaction(IncomeTransaction incomeTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.IncomeTransactions.Add(incomeTransaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}\nInner Exception: {ex.InnerException?.Message}");
            }
        }

        public static void UpdateIncomeTransaction(IncomeTransaction incomeTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.IncomeTransactions.Update(incomeTransaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteIncomeTransaction(IncomeTransaction incomeTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.IncomeTransactions.Remove(incomeTransaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static IncomeTransaction GetIncomeTransactionById(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                return context.IncomeTransactions.FirstOrDefault(t => t.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
