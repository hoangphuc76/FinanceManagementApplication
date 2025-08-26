using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class IncomeService : IIncomeService
    {
        public IncomeService() { }

        public void CreateIncomeTransaction(IncomeTransaction incomeTransaction)
            => IncomeTransactionDAO.CreateNewIncomeTransaction(incomeTransaction);

        public void CreateNewIncomeSource(IncomeSource incomeSource)
            => IncomeSourceDAO.CreateNewIncomeSource(incomeSource);

        public void DeleteIncomeSource(IncomeSource incomeSource)
            => IncomeSourceDAO.DeleteIncomeSource(incomeSource);

        public void DeleteIncomeTransaction(IncomeTransaction incomeTransaction)
            => IncomeTransactionDAO.DeleteIncomeTransaction(incomeTransaction);

        public IncomeSource GetIncomeSourceById(int id)
            => IncomeSourceDAO.GetIncomeSourceById(id);

        public int? GetIncomeIdByIncomeName(string name)
            => IncomeSourceDAO.GetIncomeIdByIncomeName(name);

        public List<IncomeSource> GetIncomeSources()
            => IncomeSourceDAO.GetIncomeSources();
        public IncomeTransaction GetIncomeTransactionById(int id)
            => IncomeTransactionDAO.GetIncomeTransactionById(id);

        public List<IncomeTransaction> GetIncomeTransactions(int userId)
            => IncomeTransactionDAO.GetIncomeTransactions(userId);

        public void UpdateIncomeSource(IncomeSource incomeSource)
            => IncomeSourceDAO.UpdateIncomeSource(incomeSource);

        public void UpdateIncomeTransaction(IncomeTransaction incomeTransaction)
            => IncomeTransactionDAO.UpdateIncomeTransaction(incomeTransaction);
    }
}
