using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for IncomeManagement.xaml
    /// </summary>
    public partial class IncomeManagement : UserControl
    {
        private readonly IncomeService _incomeService = new IncomeService();
        private List<IncomeTransaction> _incomeTransactions = new List<IncomeTransaction>();
        private int _currentUserId = UserSession.Instance.Id; // Thay bằng userId thực tế khi tích hợp xác thực
        IncomeSource incomeSource = new IncomeSource();
        private IncomeTransaction _selectedIncomeTransactionForUpdate;
        private int _selectedIncomeIdForDelete;
        private List<IncomeTransaction> _allIncomeTransactions = new List<IncomeTransaction>();


        public IncomeManagement()
        {
            InitializeComponent();
            LoadIncomeTransactions();
            LoadIncomeSources();  // thêm dòng này
        }


        // Load danh sách giao dịch thu nhập lên DataGrid   
        private void LoadIncomeTransactions()
        {
            _allIncomeTransactions = _incomeService.GetIncomeTransactions(_currentUserId);
            _incomeTransactions = _allIncomeTransactions.ToList();

            dgIncomeTransactions.ItemsSource = null;
            dgIncomeTransactions.ItemsSource = _incomeTransactions;
        }


        // Tạo mới giao dịch thu nhập
        private void btnCreateIncome(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    MessageBox.Show("Amount is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtAmount.Text, out int amount))
                {
                    MessageBox.Show("Amount must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime? transactionDate = dpTransactionDate.SelectedDate;
                if (transactionDate == null)
                {
                    MessageBox.Show("Transaction date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSourceName.Text))
                {
                    MessageBox.Show("Source Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra nguồn thu nhập đã tồn tại chưa
                var existingSource = _incomeService.GetIncomeSources()
                    .FirstOrDefault(s => s.SourceName != null && s.SourceName.Trim().Equals(txtSourceName.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                IncomeSource usedSource;
                if (existingSource != null)
                {
                    usedSource = existingSource;
                }
                else
                {
                    var newIncomeSource = new IncomeSource
                    {
                        SourceName = txtSourceName.Text.Trim()
                    };
                    _incomeService.CreateNewIncomeSource(newIncomeSource);

                    // Sau khi lưu, lấy lại nguồn vừa tạo để lấy Id
                    usedSource = _incomeService.GetIncomeSources()
                        .FirstOrDefault(s => s.SourceName != null && s.SourceName.Trim().Equals(txtSourceName.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (usedSource == null)
                {
                    MessageBox.Show("Failed to create or retrieve income source.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newIncomeTransaction = new IncomeTransaction
                {
                    UserId = _currentUserId,
                    SourceId = usedSource.Id,
                    Amount = amount,
                    Date = transactionDate
                   
                };

                _incomeService.CreateIncomeTransaction(newIncomeTransaction);

                MessageBox.Show("Income transaction created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadIncomeTransactions();
                txtAmount.Text = string.Empty;
                txtSourceName.Text = string.Empty;
                dpTransactionDate.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Đóng dialog (nếu cần)
        private void Sample1_DialogHost_OnDialogClosing(object sender, RoutedEventArgs e)
        {
            // Có thể xử lý logic khi dialog đóng nếu cần
        }

        // Xóa giao dịch thu nhập được chọn
        public void DeleteSelectedIncomeTransaction()
        {
            if (dgIncomeTransactions.SelectedItem is IncomeTransaction selectedTransaction)
            {
                var result = MessageBox.Show("Are you sure you want to delete this transaction?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _incomeService.DeleteIncomeTransaction(selectedTransaction);
                    MessageBox.Show("Deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadIncomeTransactions();
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Sửa giao dịch thu nhập được chọn
        public void UpdateSelectedIncomeTransaction()
        {
            if (dgIncomeTransactions.SelectedItem is IncomeTransaction selectedTransaction)
            {
                if (string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    MessageBox.Show("Amount is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtAmount.Text, out int amount))
                {
                    MessageBox.Show("Amount must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime? transactionDate = dpTransactionDate.SelectedDate;
                if (transactionDate == null)
                {
                    MessageBox.Show("Transaction date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSourceName.Text))
                {
                    MessageBox.Show("Source Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Cập nhật IncomeSource nếu cần
                var updatedSource = new IncomeSource
                {
                    Id = selectedTransaction.SourceId ?? 0,
                    SourceName = txtSourceName.Text
                };
                _incomeService.UpdateIncomeSource(updatedSource);

                selectedTransaction.Amount = amount;
                selectedTransaction.Date = transactionDate;
                selectedTransaction.SourceId = updatedSource.Id;
                selectedTransaction.Source = updatedSource;

                _incomeService.UpdateIncomeTransaction(selectedTransaction);

                MessageBox.Show("Income transaction updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadIncomeTransactions();
                txtAmount.Text = string.Empty;
                txtSourceName.Text = string.Empty;
                dpTransactionDate.SelectedDate = null;
            }
            else
            {
                MessageBox.Show("Please select a transaction to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Khi chọn một dòng trên DataGrid, hiển thị thông tin lên các textbox để sửa/xóa
        private void dgIncomeTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgIncomeTransactions.SelectedItem is IncomeTransaction selectedTransaction)
            {
                txtAmount.Text = selectedTransaction.Amount?.ToString() ?? string.Empty;
                txtSourceName.Text = selectedTransaction.Source?.SourceName ?? string.Empty;
                dpTransactionDate.SelectedDate = selectedTransaction.Date;
            }
        }

        // Thêm các hàm xử lý sự kiện cho Update/Delete
        private void btnUpIncomeSelectionChange(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is IncomeTransaction transaction)
            {
                _selectedIncomeTransactionForUpdate = transaction;
                txtUpIncomeId.Text = transaction.Id.ToString();
                txtUpAmount.Text = transaction.Amount?.ToString() ?? string.Empty;
                txtUpSourceName.Text = transaction.Source?.SourceName ?? string.Empty;
                dpUpTransactionDate.SelectedDate = transaction.Date;
            }
        }

        private void btnUpdateIncome(object sender, RoutedEventArgs e)
        {
            if (_selectedIncomeTransactionForUpdate == null) return;

            if (!int.TryParse(txtUpAmount.Text, out int amount))
            {
                MessageBox.Show("Amount must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUpSourceName.Text))
            {
                MessageBox.Show("Source Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime? transactionDate = dpUpTransactionDate.SelectedDate;
            if (transactionDate == null)
            {
                MessageBox.Show("Transaction date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra nguồn thu nhập đã tồn tại chưa
            var existingSource = _incomeService.GetIncomeSources()
                .FirstOrDefault(s => s.SourceName != null && s.SourceName.Trim().Equals(txtUpSourceName.Text.Trim(), StringComparison.OrdinalIgnoreCase));

            IncomeSource usedSource;
            if (existingSource != null)
            {
                usedSource = existingSource;
            }
            else
            {
                var newIncomeSource = new IncomeSource
                {
                    SourceName = txtUpSourceName.Text.Trim()
                };
                _incomeService.CreateNewIncomeSource(newIncomeSource);
                usedSource = _incomeService.GetIncomeSources()
                    .FirstOrDefault(s => s.SourceName != null && s.SourceName.Trim().Equals(txtUpSourceName.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            _selectedIncomeTransactionForUpdate.Amount = amount;
            _selectedIncomeTransactionForUpdate.Date = transactionDate;
            _selectedIncomeTransactionForUpdate.SourceId = usedSource.Id;
            _selectedIncomeTransactionForUpdate.Source = usedSource;

            _incomeService.UpdateIncomeTransaction(_selectedIncomeTransactionForUpdate);

            MessageBox.Show("Income transaction updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadIncomeTransactions();
        }

        private void btnDelIncomeSelectionChange(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int id)
            {
                _selectedIncomeIdForDelete = id;
                txtDelIncomeId.Text = id.ToString();
            }
        }

        private void btnDelIncome(object sender, RoutedEventArgs e)
        {
            var transaction = _incomeTransactions.FirstOrDefault(t => t.Id == _selectedIncomeIdForDelete);
            if (transaction != null)
            {
                _incomeService.DeleteIncomeTransaction(transaction);
                MessageBox.Show("Deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadIncomeTransactions();
            }
        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            var startDate = dpStartDate.SelectedDate;
            var endDate = dpEndDate.SelectedDate;
            int? selectedSourceId = cbSourceFilter.SelectedValue != null
                ? (int)cbSourceFilter.SelectedValue
                : (int?)null;

            var transactions = _incomeService.GetIncomeTransactions(_currentUserId)
    .Where(t => (!startDate.HasValue || t.Date >= startDate.Value) &&
                (!endDate.HasValue || t.Date <= endDate.Value) &&
                (!selectedSourceId.HasValue || t.SourceId == selectedSourceId.Value))
    .ToList();


            dgIncomeTransactions.ItemsSource = transactions;
        }

        private void LoadIncomeSources()
        {
            var sources = _incomeService.GetIncomeSources();
            cbSourceFilter.ItemsSource = sources;
            cbSourceFilter.DisplayMemberPath = "SourceName";  
            cbSourceFilter.SelectedValuePath = "Id";         
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // clear filter controls
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
            cbSourceFilter.SelectedIndex = -1;

            // load lại tất cả income transactions
            var allTransactions = _incomeService.GetIncomeTransactions(_currentUserId);
            dgIncomeTransactions.ItemsSource = allTransactions;
        }


    }
}
