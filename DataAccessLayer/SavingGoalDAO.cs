using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer
{
    public class SavingGoalDAO
    {
        public static List<SavingGoal> GetSavingGoals(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var savingGoals = context.SavingGoals
                                .Include(sg => sg.User)
                                .Where(sg => sg.UserId == userId)
                                .ToList();
                return savingGoals;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewSavingGoal(SavingGoal savingGoal)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingGoals.Add(savingGoal);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateSavingGoal(SavingGoal savingGoal)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingGoals.Update(savingGoal);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteSavingGoal(SavingGoal savingGoal)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingGoals.Remove(savingGoal);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SavingGoal GetSavingGoalById(int userId)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var SavingGoal = context.SavingGoals.FirstOrDefault(
                                sg => sg.UserId == userId);

                return SavingGoal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<MonthlyExpense> GetMonthlyExpenses(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                //var currentDate = DateTime.Now; // Convert DateTime to DateOnly for comparison

                var monthlyExpenses = context.SavingGoals
                    .Where(sg => sg.UserId == userId)
                    .GroupBy(sg => sg.GoalDate.Value.Month)
                    .Select(g => new MonthlyExpense
                    {
                        Month = g.Key,
                        TotalCurrentAmount = g.Sum(sg => sg.CurrentAmount ?? 0)
                    })
                    .OrderBy(me => me.Month)
                    .ToList();

                return monthlyExpenses;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving monthly expenses.", ex);
            }
        }

      public static int? GetCurrentTotalSavingAmount(int userId, DateTime date)
        {
            var context = new FinanceManagementApplicationContext();
            DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            int? totalCurrentAmount = context.SavingGoals.Where(sg => sg.UserId == userId &&
                     sg.GoalDate >= startOfMonth &&
                     sg.GoalDate <= endOfMonth &&
                     sg.IsCompleted == false).Sum(sg => sg.CurrentAmount);
            return totalCurrentAmount;
        }

        public static SavingGoal GetCurrentTotalSavingGoalAndTotalGoalAmount(int userId, DateTime date)
        {
            using (var context = new FinanceManagementApplicationContext())
            {
                DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var result = context.SavingGoals
                    .Where(sg => sg.UserId == userId &&
                                 sg.GoalDate >= startOfMonth &&
                                 sg.GoalDate <= endOfMonth &&
                                 sg.IsCompleted == false)
                    .GroupBy(sg => sg.UserId)
                    .Select(g => new
                    {
                        TotalCurrentAmount = g.Sum(sg => (int?)sg.CurrentAmount) ?? 0,
                        TotalGoalAmount = g.Sum(sg => (int?)sg.GoalAmount) ?? 0
                    })
                    .FirstOrDefault();

                return new SavingGoal
                {
                    UserId = userId,
                    CurrentAmount = result?.TotalCurrentAmount ?? 0,
                    GoalAmount = result?.TotalGoalAmount ?? 0
                };
            }
        }
    }
}
