using FinanceManagementApp.Domain;
using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for UserSidebar.xaml
    /// </summary>
    public partial class UserSidebar : UserControl
    {
        public List<SampleItem> SampleList { get; set; }
        public UserSidebar()
        {
            InitializeComponent();
            DataContext = this;

            SampleList = new()
        {
            new SampleItem
            {
                Title = "Dashboard",
                SelectedIcon = PackIconKind.ViewDashboardEdit,
                UnselectedIcon = PackIconKind.ViewDashboard,
                //Notification = 1
            },
            new SampleItem
            {
                Title = "Expense",
                SelectedIcon = PackIconKind.BankTransferOut,
                UnselectedIcon = PackIconKind.BankTransferOut,
            },
            new SampleItem
            {
                Title = "Income",
                SelectedIcon = PackIconKind.BankTransferIn,
                UnselectedIcon = PackIconKind.BankTransferIn,
            },
            new SampleItem
            {
                Title = "Budget",
                SelectedIcon = PackIconKind.HandCoin,
                UnselectedIcon = PackIconKind.HandCoinOutline,
            },
            new SampleItem
            {
                Title = "Goals",
                SelectedIcon = PackIconKind.BullseyeArrow,
                UnselectedIcon = PackIconKind.BullseyeArrow,
            },
            new SampleItem
            {
                Title = "Settings",
                SelectedIcon = PackIconKind.Cog,
                UnselectedIcon = PackIconKind.CogOutline,
            },
        };
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //UserDataGrid.ItemsSource = userService.getAllUsers();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        => SampleList[0].Notification = SampleList[0].Notification is null ? 1 : null;

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
            => SampleList[0].Notification = SampleList[0].Notification is null ? "123+" : null;

        private void SidebarListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
        }

        public static readonly RoutedEvent SelectionChangedEvent =
            EventManager.RegisterRoutedEvent(
                "SelectionChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserSidebar));

        public event RoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }
    }
}
