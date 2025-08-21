using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ExpenseTransactionDAO
    {
        public static void CreateExpenseTransaction(ExpenseTransaction transaction)
        {
            using var context = new FinanceManagementApplicationContext();
            context.ExpenseTransactions.Add(transaction);
            context.SaveChanges();
        }

        public static void DeleteExpenseTransaction(int id)
        {
            using var context = new FinanceManagementApplicationContext();
            var transaction = context.ExpenseTransactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                context.ExpenseTransactions.Remove(transaction);
                context.SaveChanges();
            }
        }
        public static void UpdateExpenseTransaction(ExpenseTransaction transaction)
        {
            using var context = new FinanceManagementApplicationContext();
            var existingTransaction = context.ExpenseTransactions.FirstOrDefault(t => t.UserId == transaction.UserId);
            if (existingTransaction != null)
            {
                existingTransaction.BudgetId = transaction.BudgetId;
                existingTransaction.Note = transaction.Note;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Date = transaction.Date;

                context.SaveChanges();
            }
        }

        public static ExpenseTransaction? GetExpenseTransaction(int id)
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions.FirstOrDefault(t => t.UserId == id);
        }

        public static IEnumerable<ExpenseTransaction> GetAllExpenseTransactions()
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions
                         .Include(t => t.Budget)
                         .ToList();
        }

        public static List<ExpenseTransaction> GetAllExpenseTransactionsById(int userId)
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions
                         .Where(t => t.UserId == userId)
                         .Select(t => new ExpenseTransaction
                         {
                             Id = t.Id,
                             BudgetId = t.BudgetId,
                             Amount = t.Amount,
                             Budget = new BudgetItem
                             {
                                 BudgetName = t.Budget.BudgetName
                             },
                             Date = t.Date,
                             Note = t.Note
                         })
                         .OrderByDescending(t => t.Date)
                         .ToList();
        }

        public static List<ExpenseTransaction> GetExpenseTransactions(int userId)
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions
                          .Include(e => e.Budget)
                          .Include(e => e.User)
                          .Where(e => e.UserId == userId)
                          .Select(e => new ExpenseTransaction
                          {
                              UserId = e.UserId,
                              BudgetId = e.BudgetId,
                              Amount = e.Amount,
                              Date = e.Date,
                              Budget = new BudgetItem
                              {
                                  Id = e.Budget.Id,
                                  BudgetName = e.Budget.BudgetName
                              }
                          })
                          .ToList();
        }


        public static void CreateNewTransaction(ExpenseTransaction transaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.ExpenseTransactions.Add(transaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateTransaction(ExpenseTransaction transaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.ExpenseTransactions.Update(transaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteTransaction(ExpenseTransaction transaction) 
        { 
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.ExpenseTransactions.Remove(transaction);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static ExpenseTransaction GetTransactionById(int userId)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                return context.ExpenseTransactions.FirstOrDefault(t => t.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
