using BusinessObjects;
using FinanceManagementApp.Domain;
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
    /// Interaction logic for UserHeader.xaml
    /// </summary>
    public partial class UserHeader : UserControl
    {
        public UserHeader()
        {
            InitializeComponent();
            txtUsername.Text = UserSession.Instance.Username;
        }

        public void ChangedTitleAndSubTitle(ScreenType screenType)
        {
            switch(screenType.ToString())
            {
                case "BudgetManagement":
                    txtName.Text = "Budget";
                    txtSubtitle.Text = "Create and track your budgets";
                    break;
                case "Dashboard":
                    txtName.Text = "Welcome back";
                    txtSubtitle.Text = "It is the best time to manage your finances";
                    break;
                case "Goal":
                    txtName.Text = "Goals";
                    txtSubtitle.Text = "Create financial goals and manage your savings";
                    break;
            }
        }
    }
}
