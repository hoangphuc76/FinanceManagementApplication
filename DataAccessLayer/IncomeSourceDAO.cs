using BusinessObjects;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class IncomeSourceDAO
    {
        public static List<IncomeSource> GetIncomeSources()
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var IncomeSources = context.IncomeSources.ToList();
                return IncomeSources;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void CreateNewIncomeSource(IncomeSource incomeSource)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.IncomeSources.Add(incomeSource);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateIncomeSource(IncomeSource incomeSource)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.IncomeSources.Update(incomeSource);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteIncomeSource(IncomeSource incomeSource)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.IncomeSources.Remove(incomeSource);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static IncomeSource GetIncomeSourceById(int id)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                return context.IncomeSources.FirstOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
