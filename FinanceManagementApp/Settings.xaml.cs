using BusinessObjects;
using DataAccessLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.NativeInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private readonly HttpClient _httpClient;
        private readonly string _payosApiUrl = "https://api-merchant.payos.vn";
        private readonly string _clientId = "111d56f4-daca-4a89-8f9d-f82062477168"; // Replace with your actual PayOS Client ID
        private readonly string _apiKey = "c315b03a-4b67-4616-9c92-6752f5855f15"; // Replace with your actual PayOS API Key
        private readonly string _checksumKey = "1b1464d721480a4ae414c213eca9812c2b05b439e41db819d2f7099fd10bb327"; // Replace with your actual PayOS Checksum Key
        
        public Settings()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_payosApiUrl);
            
            // Check if user is already Pro
            CheckProStatus();
        }
        
        private void CheckProStatus()
        {
            // Get current user from session
            var currentUser = UserSession.Instance;
            
            // Check if user is Pro
            bool isPro = currentUser.IsPro;
            
            // Update UI based on Pro status
            //btnUpgradeToPro.IsEnabled = !isPro;
            //btnUpgradeToPro.Content = isPro ? "Pro Activated" : "Nâng cấp ngay";
            //btnUpgradeToPro.Background = isPro ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
        }
        
        private async void btnUpgradeToPro_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long baseOrderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                int rand = new Random().Next(1000, 9999);
                string orderCode = $"{baseOrderCode}{rand}".Substring(0, 15);
                var amount = 20000; // 20,000 VND
                var description = "Nâng cấp gói Pro";
                
                // Create PayOS payment link
                var paymentUrl = await CreatePaymentLink(orderCode, amount, description);
                
                // Open payment URL in default browser
                if (!string.IsNullOrEmpty(paymentUrl))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = paymentUrl,
                        UseShellExecute = true
                    });
                    
                    // Show waiting message
                    MessageBox.Show("Đang chuyển đến trang thanh toán. Vui lòng hoàn tất thanh toán.", 
                        "Thanh toán", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Start checking payment status
                    await CheckPaymentStatus(orderCode);
                }
                else
                {
                    MessageBox.Show("Không thể tạo liên kết thanh toán. Vui lòng thử lại sau.", 
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", 
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static string GenerateSignature(string data, string checksumKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        private async Task<string> CreatePaymentLink(string code, int Amount, string Description)
        {
            try
            {
                string cancelUrl = "https://yourdomain.com/cancel";
                string returnUrl = "https://yourdomain.com/success";
                string rawData = $"amount={Amount}&cancelUrl={cancelUrl}&description={Description}&orderCode={code}&returnUrl={returnUrl}";
                string Signature = GenerateSignature(rawData, _checksumKey);

                // Create payment request data
                var paymentData = new
                {
                    orderCode = long.Parse(code),
                    amount = Amount,
                    description = Description,
                    cancelUrl = "https://yourdomain.com/cancel", // Replace with your actual cancel URL
                    returnUrl = "https://yourdomain.com/success", // Replace with your actual success URL
                    signature = Signature
                };
                
                // Convert to JSON
                var content = new StringContent(
                    JsonConvert.SerializeObject(paymentData),
                    Encoding.UTF8,
                    "application/json");
                
                // Add headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-client-id", _clientId);
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                
                // Send request to create payment link
                var response = await _httpClient.PostAsync("/v2/payment-requests", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(responseContent);
                    
                    return paymentResponse?.Data?.CheckoutUrl;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating payment link: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        
        private async Task CheckPaymentStatus(string orderCode)
        {
            try
            {
                // Wait a bit to allow user to complete payment
                await Task.Delay(30000); // 30 seconds
                
                // Check payment status
                var status = await GetPaymentStatus(orderCode);
                
                if (status == "PAID")
                {
                    // Update user to Pro
                    UpdateUserToPro();
                    
                    MessageBox.Show("Thanh toán thành công! Bạn đã nâng cấp lên gói Pro.", 
                        "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Update UI
                    CheckProStatus();
                }
                else
                {
                    MessageBox.Show("Thanh toán chưa hoàn tất hoặc đã bị hủy.", 
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kiểm tra trạng thái thanh toán: {ex.Message}", 
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async Task<string> GetPaymentStatus(string orderCode)
        {
            try
            {
                // Add headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-client-id", _clientId);
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                
                // Get payment status
                var response = await _httpClient.GetAsync($"/v2/payment-requests/{orderCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var statusResponse = JsonConvert.DeserializeObject<PaymentStatusResponse>(responseContent);
                    
                    return statusResponse?.Data?.Status;
                }
                
                return "UNKNOWN";
            }
            catch
            {
                return "ERROR";
            }
        }
        
        private void UpdateUserToPro()
        {
            try
            {
                // Get current user
                var currentUser = UserSession.Instance;
                
                // Update user in database
                var user = UserDAO.getUser(currentUser.Email);
                if (user != null)
                {
                    user.IsPro = true;
                    UserDAO.UpdateUser(user);
                    
                    // Update session
                    UserSession.Instance.IsPro = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin người dùng: {ex.Message}", 
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
    // Response models for PayOS API
    public class PaymentResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("data")]
        public PaymentData Data { get; set; }
    }
    
    public class PaymentData
    {
        [JsonProperty("checkoutUrl")]
        public string CheckoutUrl { get; set; }
        
        [JsonProperty("paymentLinkId")]
        public string PaymentLinkId { get; set; }
    }
    
    public class PaymentStatusResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("data")]
        public PaymentStatusData Data { get; set; }
    }
    
    public class PaymentStatusData
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
