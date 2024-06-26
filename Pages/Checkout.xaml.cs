using Newtonsoft.Json;
using PetsApp.Models;
using System;
using System.Net.Http;
using System.Text;

namespace PetsApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Checkout : ContentPage
    {
        private readonly PetsModel _selectedPet;
        private string _selectedPaymentMethod = "";
        private bool _paymentMethodSelected = false;


        public Checkout(PetsModel selectedPet)
        {
            InitializeComponent();
            _selectedPet = selectedPet;
            UpdatePetDetails();
            string address = Preferences.Get("Address", "");
            string phone = Preferences.Get("Phone", "");
            string name = Preferences.Get("FullName", "");

            AddressEntry.Text = address;
            PhoneEntry.Text = phone;
            NameEntry.Text = name;
        }

        private void UpdatePetDetails()
        {
            // Cập nhật thông tin thú cưng vào các thành phần UI
            NameLabel.Text = _selectedPet.NamePets;
            Price.Text = _selectedPet.Price.ToString("C");
            ImageLa.Source = ImageSource.FromFile(_selectedPet.Image);
            Total.Text = _selectedPet.Price.ToString("C");

            SpeciesLabel.Text = _selectedPet.Species;
            double total = (_selectedPet.Price * 1) + 20;
            TotalLabel.Text = total.ToString("C");
        }

        private void PaymentMethodSelected(object sender, EventArgs e)
        {
            // Lấy phương thức thanh toán được chọn từ picker và lưu vào biến toàn cục
            var picker = (Picker)sender;
            _selectedPaymentMethod = picker.SelectedItem.ToString();
            _paymentMethodSelected = true;
        }

        private async void CheckoutButton(object sender, EventArgs e)
        {
            if (!_paymentMethodSelected)
            {
                await DisplayAlert("Alert", "Please select a payment method.", "OK");
                return;
            }
            string address = AddressEntry.Text;
            string recipientName = NameEntry.Text;
            string recipientPhone = PhoneEntry.Text;

            // Kiểm tra xem các trường có được điền đầy đủ không
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(recipientName) || string.IsNullOrEmpty(recipientPhone))
            {
                await DisplayAlert("Alert", "Please fill in all required fields.", "OK");
                return;
            }
            int userId = Preferences.Get("UserID", 0);
         
            double total = double.Parse(TotalLabel.Text.Replace("$", ""));
            DateTime date = DateTime.Now;

            // Gửi yêu cầu tạo đơn hàng đến API
            var httpClient = new HttpClient();
            //var apiUrl = $"https://localhost:7104/api/Orders?userId={userId}&total={total}&date={date}";
            string apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/Orders?userId={0}&total={1}&date={2}",
                             Uri.EscapeDataString(userId.ToString()), Uri.EscapeDataString(total.ToString()),
                             Uri.EscapeDataString(date.ToString("yyyy-MM-dd")));
            var response = await httpClient.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                var orderContent = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(orderContent);
                int orderId = order.Id;

                // Tạo đơn hàng chi tiết và gửi yêu cầu đến API
                var orderDetails = new OrderDetailsModel
                {
                    OrderID = orderId,
                    PetId = _selectedPet.ID, // Sử dụng Id của thú cưng đã chọn
                    Quantity = 1, // Số lượng
                    unit = "$", // Đơn vị
                    payment = _selectedPaymentMethod, // Phương thức thanh toán
                    Address = address, // Địa chỉ giao hàng
                    RecipientName = recipientName, // Tên người nhận
                    RecipientPhone = recipientPhone // Số điện thoại người nhận
                };

                var orderDetailsJson = JsonConvert.SerializeObject(orderDetails);
                var orderDetailsContent = new StringContent(orderDetailsJson, Encoding.UTF8, "application/json");
                var orderDetailsResponse = await httpClient.PostAsync(Config.ApiConfig.BaseUrl + "/OrderDetails", orderDetailsContent);

                if (orderDetailsResponse.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Order placed successfully!", "OK");
                    await Navigation.PushAsync(new Orders());
                }
                else
                {
                    await DisplayAlert("Error", "Failed to place order. Please try again later.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Failed to place order. Please try again later.", "OK");
            }
        }

        private async void Goback(object sender, TappedEventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
