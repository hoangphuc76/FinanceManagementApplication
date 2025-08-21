using BusinessObjects;
using DataAccessLayer;
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
using System.Windows.Shapes;


namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            string repeatPassword = txtRepeatPassword.Password;

            // Kiểm tra các trường không được để trống
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(repeatPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra mật khẩu và xác nhận mật khẩu có trùng khớp không
            if (password != repeatPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra nếu người dùng đã tồn tại
            if (IsUserExists(email))
            {
                MessageBox.Show("Username already exists. Please choose a different username.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tạo đối tượng User mới
            User newUser = new User
            {
                Email = email,
                Password = password, // Bạn có thể mã hóa mật khẩu trước khi lưu
                
            };

            // Lưu người dùng vào cơ sở dữ liệu
            CreateNewUser(newUser);

            // Thông báo đăng ký thành công
            MessageBox.Show("Sign up successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Điều hướng đến màn hình đăng nhập (hoặc đóng cửa sổ hiện tại)
            this.Close(); // Hoặc mở một cửa sổ đăng nhập mới
        }

        // Kiểm tra người dùng đã tồn tại trong cơ sở dữ liệu chưa
        private bool IsUserExists(string email)
        {
            using (var context = new FinanceManagementApplicationContext())
            {
                return context.Users.Any(u => u.Email == email);
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove(); // Cho phép cửa sổ di chuyển
            }
        }
     


        // Tạo người dùng mới và lưu vào cơ sở dữ liệu
        private void CreateNewUser(User newUser)
        {
            using (var context = new FinanceManagementApplicationContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            // Tạo đối tượng SignUpWindow mới
            LoginWindow loginWindow = new LoginWindow();

            // Hiển thị cửa sổ SignUpWindow
            loginWindow.Show();

            // Đóng cửa sổ hiện tại (nếu cần)
            this.Close();
        }
    }
}