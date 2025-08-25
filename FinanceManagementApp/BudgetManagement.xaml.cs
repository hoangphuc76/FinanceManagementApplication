using BusinessObjects;
using DataAccessLayer;
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
using static System.Net.Mime.MediaTypeNames;

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for BudgetManagement.xaml
    /// </summary>
    public partial class BudgetManagement : UserControl
    {
        private readonly IBudgetService _service;
        private int currentBudgetId;
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
        private void btn_openUpdateDialog(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var budget = button.Tag as BudgetItem;
            
            currentBudgetId = budget.Id;
            txtBudgetNameUpdate.Text = budget.BudgetName;
            txtBudgetLimitUpdate.Text = budget.LimitAmount.ToString();
            UpdateBudget.IsOpen = true;
        }

        private void btn_updateBudget(object sender, RoutedEventArgs e)
        {
            try
            {
                var budget = new BudgetItem
                {
                    Id = currentBudgetId,
                    UserId = UserSession.Instance.Id,
                    BudgetName = txtBudgetNameUpdate.Text.ToString(),
                    LimitAmount = Int32.Parse(txtBudgetLimitUpdate.Text.ToString()),
                };
                BudgetItemDAO.UpdateBudget(budget);
                MessageBox.Show("Update budget successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                loadBudgets();
            }
        }

        private void btn_createBudget(object sender, RoutedEventArgs e)
        {
            try
            {
                var budget = new BudgetItem
                {
                    UserId = UserSession.Instance.Id,
                    BudgetName = txtBudgetName.Text.ToString(),
                    LimitAmount = Int32.Parse(txtBudgetLimit.Text.ToString()),
                };
                BudgetItemDAO.CreateNewBudget(budget);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
            finally
            {
                loadBudgets();
            }
        }
    }
}
