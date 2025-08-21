using BusinessObjects;
using FinanceManagementApp.Domain;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for BudgetManagement.xaml
    /// </summary>
    public partial class BudgetManagement : UserControl
    {
        private readonly IBudgetService _service;
        public BudgetManagement()
        {
            InitializeComponent();
            userHeaderControl.ChangedTitleAndSubTitle(ScreenType.BudgetManagement);
            _service = new BudgetService();
            loadBudgets();
        }

        private void loadBudgets()
        {
            try
            {
                 var budgets =  _service.GetCurrentMonthBudgets(UserSession.Instance.Id);
                var expsenseInfor = _service.GetExpenseInfor(budgets);

                lbBudget.ItemsSource = budgets;
                txtNumberOfBudget.Text = budgets.Count.ToString();
                txtTotalCurrentExpense.Text = expsenseInfor.TotalExpense.ToString();
                txtTotalExpensePercent.Value = (double)expsenseInfor.TotalExpensePercent;
                txtTotalExpsense.Text = expsenseInfor.TotalLimit.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
