using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SavingTransactionDAO
    {
        public static List<SavingTransaction> GetSavingTransactions(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var transaction = context.SavingTransactions
                                    .Include(st => st.SavingGoal)
                                    .Include(st => st.User)
                                    .Where(st => st.UserId == userId)
                                    .ToList();
                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewSavingTransaction(SavingTransaction savingTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingTransactions.Add(savingTransaction);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateSavingTransaction(SavingTransaction savingTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingTransactions.Update(savingTransaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteSavingTransaction(SavingTransaction savingTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingTransactions.Remove(savingTransaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SavingTransaction GetSavingTransactionById(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                return context.SavingTransactions.FirstOrDefault(st => st.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
