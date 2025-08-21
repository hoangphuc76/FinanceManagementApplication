using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IIncomeService
    {
        //Function for Income Source
        void CreateNewIncomeSource(IncomeSource incomeSource);
        void UpdateIncomeSource(IncomeSource incomeSource);
        void DeleteIncomeSource(IncomeSource incomeSource);
        List<IncomeSource> GetIncomeSources();
        IncomeSource GetIncomeSourceById(int id);

        //Function for Income Transaction
        void CreateIncomeTransaction(IncomeTransaction incomeTransaction);
        void UpdateIncomeTransaction(IncomeTransaction incomeTransaction);
        void DeleteIncomeTransaction(IncomeTransaction incomeTransaction);
        List<IncomeTransaction> GetIncomeTransactions(int userId);
        IncomeTransaction GetIncomeTransactionById(int id);
    }
}
