using BusinessObjects;
using DataAccessLayer;
using FinanceManagementApp.Domain;
using MaterialDesignThemes.Wpf;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for ExpenseManagement.xaml
    /// </summary>
    public partial class ExpenseManagement : UserControl
    {
        private readonly IExpenseTransactionService _service;

        // Public Property to hold the selected transaction
        public string CurrentDate => DateTime.Now.ToString("dd/MM/yyyy");


        public ExpenseManagement()
        {
            _service = new ExpenseTransactionService();
            InitializeComponent();
            LoadExpenseTransactions();
        }

        private void LoadExpenseTransactions()
        {
            dgExpenseTransactions.ItemsSource = ExpenseTransactionDAO.GetAllExpenseTransactionsById(UserSession.Instance.Id);
            var budgetItems = BudgetItemDAO.GetBudgetItems(UserSession.Instance.Id);
            cbBudget.ItemsSource = budgetItems;
            cbBudget.SelectedValuePath = "Id";
            cbBudget.DisplayMemberPath = "BudgetName";
            cbUpBudget.ItemsSource = budgetItems;
            cbUpBudget.SelectedValuePath = "Id";
            cbUpBudget.DisplayMemberPath = "BudgetName";
        }

        
        private void btnUpSelectionChange(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is ExpenseTransaction transaction)
            {
                txtUpExpenseId.Text = transaction.Id.ToString();
                cbUpBudget.SelectedValue = transaction.BudgetId;
                txtUpAmount.Text = transaction.Amount.ToString();
                txtUpNote.Text = transaction.Note;
                dpUpTransactionDate.SelectedDate = transaction.Date;
            }
        }

        private void btnDelSelectionChange(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is int Id)
            {
                txtDelExpenseId.Text = Id.ToString();
            }
        }

        private void btnUpdate(object sender, RoutedEventArgs e)
        {
            if (cbUpBudget.SelectedValue == null || string.IsNullOrEmpty(txtUpAmount.Text) || string.IsNullOrEmpty(txtUpNote.Text) || dpUpTransactionDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill all information.");
                return;
            }

            ExpenseTransaction et = new ExpenseTransaction
            {
                Id = Int32.Parse(txtUpExpenseId.Text),
                UserId = 1,
                BudgetId = (int)cbUpBudget.SelectedValue,
                Amount = Int32.Parse(txtUpAmount.Text),
                Note = txtUpNote.Text,
                Date = dpUpTransactionDate.SelectedDate
            };

            try
            {
                _service.UpdateExpenseTransaction(et);
                MessageBox.Show("Update successfully!");
                LoadExpenseTransactions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreateExpense(object sender, RoutedEventArgs e)
        {
            if (cbBudget.SelectedValue == null || string.IsNullOrEmpty(txtAmount.Text) || string.IsNullOrEmpty(txtNote.Text) || dpTransactionDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill all information.");
                return;
            }

            ExpenseTransaction et = new ExpenseTransaction
            {
                UserId = 1,
                BudgetId = (int)cbBudget.SelectedValue,
                Amount = Int32.Parse(txtAmount.Text),
                Note = txtNote.Text,
                Date = dpTransactionDate.SelectedDate
            };

            try
            {
                _service.CreateExpenseTransaction(et);
                LoadExpenseTransactions();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelExpense(object sender, RoutedEventArgs e)
        {
            _service.DeleteExpenseTransaction(Int32.Parse(txtDelExpenseId.Text));
            LoadExpenseTransactions();
        }
            private void OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            cbBudget.SelectedValue = null;
            txtAmount.Text = "";
            txtNote.Text = "";
            dpTransactionDate.SelectedDate = null;
        }
    }
}

