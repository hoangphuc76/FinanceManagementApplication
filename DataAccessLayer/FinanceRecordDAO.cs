using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class FinanceRecordDAO
    {
        public static List<FinanceRecord> GetFinanceRecords(int userId)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var FinanceRecords = context.FinanceRecords
                                    .Include(r => r.User)
                                    .Where(r => r.UserId == userId)
                                    .ToList();

                return FinanceRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewFinanceRecord(FinanceRecord financeRecord)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.FinanceRecords.Add(financeRecord);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateFinanceRecord(FinanceRecord financeRecord)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.FinanceRecords.Update(financeRecord);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteFinanceRecord(FinanceRecord financeRecord)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.FinanceRecords.Remove(financeRecord);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static FinanceRecord GetFinanceRecord(int userId, int month, int year)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                return context.FinanceRecords.FirstOrDefault(r => r.UserId == userId && r.Month == month && r.Year == year);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
