using BusinessObjects;
using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.VisualBasic.ApplicationServices;
using Services;
using System;
using System.Collections.Generic;
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
using DataAccessLayer;
using Separator = LiveCharts.Wpf.Separator;

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        private string _note;
        private UserService _userService;
        public SavingService _savingService;
        ExpenseService _expenseService;

        public Dashboard()
        {
            InitializeComponent();
            _userService = new UserService();
            _savingService = new SavingService();
            _expenseService = new ExpenseService();
            DataContext = this;
            getSession();
            CalculateBalanceChangePerxentage();
            CalculateIncomeChangePerxentage();
            CalculateExpenseChangePercentage();
            GetCurrentSavingAndGoal();
            LoadDataForLastFiveMonths();
            LoadBudgetData();
            //LoadExpenseTransactions();
        }

        public void getSession()
        {
            if (UserSession.Instance == null)
            {
                MessageBox.Show("Your data is not available");
            }
            else
            {
                _totalBalance = UserSession.Instance.Balance.ToString();
            }
        }

        private string _totalBalance;
        private double _balanceChangePercentage;
        private bool _isBalanceIncreased;
        public string TotalBalance
        {
            get => _totalBalance;
            set
            {
                _totalBalance = value;
                OnPropertyChanged(nameof(TotalBalance));
            }
        }

        //Handle balance percentage
        private void CalculateBalanceChangePerxentage()
        {
            int id = UserSession.Instance.Id;
            _balanceChangePercentage = _userService.HandleBalanceComparison(id);
            BalanceChangePercentage = _balanceChangePercentage;
        }
        public double BalanceChangePercentage
        {
            get => _balanceChangePercentage;
            set
            {
                _balanceChangePercentage = value;
                OnPropertyChanged(nameof(BalanceChangePercentage));
                IsBalanceIncreased = _balanceChangePercentage > 0;
            }
        }

        public bool IsBalanceIncreased
        {
            get => _isBalanceIncreased;
            private set
            {
                _isBalanceIncreased = value;
                OnPropertyChanged(nameof(IsBalanceIncreased));
            }
        }

        private string _totalIncome;
        private double _incomeChangePercentage;
        private bool _isIncomeIncreased;
        private int getCurrentIncome()
        {
            DateTime currentDate = DateTime.Now;
            int userId = UserSession.Instance.Id;
            int currMonth = currentDate.Month;
            int currYear = currentDate.Year;

            FinanceRecord currRecord = _userService.GetFinanceRecord(userId, currMonth, currYear);

            int currIncome = (int)(currRecord.TotalIncome);
            return currIncome;
        }
        public string TotalIncome
        {
            get => getCurrentIncome().ToString();
            set
            {
                _totalIncome = value;
                OnPropertyChanged(nameof(TotalIncome));
            }
        }

        //Handle income percentage
        private void CalculateIncomeChangePerxentage()
        {
            int id = UserSession.Instance.Id;
            _incomeChangePercentage = _userService.HandleIncomeComparison(id);
            IncomeChangePercentage = _incomeChangePercentage;
        }
        public double IncomeChangePercentage
        {
            get => _incomeChangePercentage;
            set
            {
                _incomeChangePercentage = value;
                OnPropertyChanged(nameof(IncomeChangePercentage));
                IsIncomeIncreased = _incomeChangePercentage > 0;
            }
        }

        public bool IsIncomeIncreased
        {
            get => _isIncomeIncreased;
            private set
            {
                _isIncomeIncreased = value;
                OnPropertyChanged(nameof(IsIncomeIncreased));
            }
        }

        private string _totalExpense;
        private double _expenseChangePercentage;
        private bool _isExpenseIncreased;

        private int getCurrentExpense()
        {
            DateTime currentDate = DateTime.Now;
            int userId = UserSession.Instance.Id;
            int currMonth = currentDate.Month;
            int currYear = currentDate.Year;

            FinanceRecord currRecord = _userService.GetFinanceRecord(userId, currMonth, currYear);

            int currExpense = (int)(currRecord.TotalExpense);
            return currExpense;
        }

        public string TotalExpense
        {
            get => getCurrentExpense().ToString();
            set
            {
                _totalExpense = value;
                OnPropertyChanged(nameof(TotalExpense));
            }
        }

        // Handle expense percentage
        private void CalculateExpenseChangePercentage()
        {
            int id = UserSession.Instance.Id;
            _expenseChangePercentage = _userService.HandleExpenseComparison(id);
            ExpenseChangePercentage = _expenseChangePercentage;
        }

        public double ExpenseChangePercentage
        {
            get => _expenseChangePercentage;
            set
            {
                _expenseChangePercentage = value;
                OnPropertyChanged(nameof(ExpenseChangePercentage));
                IsExpenseIncreased = _expenseChangePercentage > 0;
            }
        }

        public bool IsExpenseIncreased
        {
            get => _isExpenseIncreased;
            private set
            {
                _isExpenseIncreased = value;
                OnPropertyChanged(nameof(IsExpenseIncreased));
            }
        }
        private string _totalCurrentSavingAmount;
        private string _savingNeeded;
        private void GetCurrentSavingAndGoal()
        {
            DateTime currentDate = DateTime.Now;
            int userId = UserSession.Instance.Id;
            int currMonth = currentDate.Month;
            int currYear = currentDate.Year;
            SavingGoal currentSavingGoalStatus = _savingService.GetCurrentTotalSavingGoalAndTotalGoalAmount(userId, currentDate);
            int? currSaving = currentSavingGoalStatus.CurrentAmount;
            int? goalAmount = currentSavingGoalStatus.GoalAmount;

            _totalCurrentSavingAmount = currSaving.ToString();
            _savingNeeded = (goalAmount - currSaving).ToString();
        }

        public string SavingNeeded
        {
            get => _savingNeeded;
            set
            {
                _savingNeeded = value;
                OnPropertyChanged(nameof(SavingNeeded));
            }
        }

        public string TotalCurrentSavingAmount
        {
            get => _totalCurrentSavingAmount;
            set
            {
                _totalCurrentSavingAmount = value;
                OnPropertyChanged(nameof(TotalCurrentSavingAmount));
            }
        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void LoadDataForLastFiveMonths()
        {
            var incomeValues = new ChartValues<double>();
            var expenseValues = new ChartValues<double>();
            var labels = new List<string>();

            for (int i = 4; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                int month = date.Month;
                int year = date.Year;
                int id = UserSession.Instance.Id;

                FinanceRecord financeRecord = _userService.GetFinanceRecord(id, month, year);

                int income = financeRecord?.TotalIncome ?? 0;
                int expense = financeRecord?.TotalExpense ?? 0;

                incomeValues.Add(income);
                expenseValues.Add(expense);
                labels.Add($"{month}/{year}");
            }

            var incomeColor = (Brush)new BrushConverter().ConvertFromString("#8470FF");
            var expenseColor = (Brush)new BrushConverter().ConvertFromString("#D6D2FF");

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Income",
                    Values = incomeValues,
                    Stroke = incomeColor,
                    Fill = incomeColor,
                    DataLabels = true
                },
                new ColumnSeries
                {
                    Title = "Expense",
                    Values = expenseValues,
                    Stroke = expenseColor,
                    Fill = expenseColor,
                    DataLabels = true
                }
            };
            Labels = labels.ToArray();
            YFormatter = value => $"{value:n0},000VND";
        }

        private SeriesCollection _budgetSeriesCollection;
        public SeriesCollection BudgetSeriesCollection
        {
            get => _budgetSeriesCollection;
            set
            {
                _budgetSeriesCollection = value;
                OnPropertyChanged(nameof(BudgetSeriesCollection));
            }
        }
        private List<Color> pieChartColors = new List<Color>
        {
            (Color)ColorConverter.ConvertFromString("#8470FF"),
            (Color)ColorConverter.ConvertFromString("#A498FF"),
            (Color)ColorConverter.ConvertFromString("#B7B7FF"),
            (Color)ColorConverter.ConvertFromString("#D6D2FF"),
            (Color)ColorConverter.ConvertFromString("#45454B"),
            (Color)ColorConverter.ConvertFromString("#56565E"),
            (Color)ColorConverter.ConvertFromString("#82828C"),
            (Color)ColorConverter.ConvertFromString("#FF9133"),
            (Color)ColorConverter.ConvertFromString("#A1A1A9"),
            (Color)ColorConverter.ConvertFromString("#D0D0D4")
        };
        private int _totalBudget;
        public int TotalBudget
        {
            get => _totalBudget;
            set
            {
                _totalBudget = value;
                OnPropertyChanged(nameof(TotalBudget));
            }
        }
        private string _formatedTotalBudget;
        public string FormatedTotalBudget
        {
            get => Util.Instance.FormatMoney(TotalBudget);
            set
            {
                _formatedTotalBudget = value;
                OnPropertyChanged(nameof(FormatedTotalBudget));
            }
        }

        private void LoadBudgetData()
        {
            var budgetItems = _userService.GetBudgetItems(UserSession.Instance.Id);
            BudgetSeriesCollection = new SeriesCollection();

            int totalBudget = 0;

            for (int i = 0; i < budgetItems.Count; i++)
            {
                var pieSeries = new PieSeries
                {
                    Title = budgetItems[i].BudgetName,
                    Values = new ChartValues<int> { budgetItems[i].LimitAmount },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y:#},000VND",
                    Fill = new SolidColorBrush(pieChartColors[i % pieChartColors.Count])
                };

                totalBudget += (budgetItems[i].LimitAmount);

                BudgetSeriesCollection.Add(pieSeries);
            }
            TotalBudget = totalBudget;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void Button_ToBudget_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}