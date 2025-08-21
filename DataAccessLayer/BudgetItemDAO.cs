using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BudgetItemDAO
    {
        public static List<BudgetItem> GetBudgetItems(int userId)
        {
            var budgetItems = new List<BudgetItem>();
            try
            {
                using var context = new FinanceManagementApplicationContext();
                budgetItems = context.BudgetItems
                                    .Where(b => b.UserId == userId)
                                    .Select(b => new BudgetItem
                                    {
                                        Id = b.Id,
                                        UserId = b.UserId,
                                        BudgetName = b.BudgetName,
                                        LimitAmount = b.LimitAmount,
                                        CurrentAmount = context.ExpenseTransactions.Where(et => et.BudgetId == b.Id).Sum(et => (Int32)et.Amount)
                                    })
                                    .ToList();
                return budgetItems; 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewBudget(BudgetItem budget)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.BudgetItems.Add(budget);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateBudget(BudgetItem budget)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.BudgetItems.Update(budget);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteBudget(BudgetItem budget)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.BudgetItems.Remove(budget);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static BudgetItem GetBudgetItemById(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var budget = context.BudgetItems.FirstOrDefault(b => b.UserId == userId);

                return budget;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}