using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using PetsApp.Models;
using PetsApp.Repository;
using PetsApp.ViewModel;

namespace PetsApp.Pages
{
    public partial class Checkout1 : ContentPage
    {
        private string _selectedPaymentMethod = "";
        private bool _paymentMethodSelected = false;
        private List<CartItem> _cartItems;
        private readonly PetServices _petServices;
        public Checkout1(List<CartItem> cartItems)
        {
            InitializeComponent();
            _cartItems = cartItems;
            _petServices = new PetServices();
            BindingContext = new CheckoutVM(cartItems);
            string address = Preferences.Get("Address", "");
            string phone = Preferences.Get("Phone", "");
            string name = Preferences.Get("FullName", "");
        
            AddressEntry.Text = address;
            PhoneEntry.Text = phone;
            NameEntry.Text = name;
        }
        private void PaymentMethodSelected(object sender, EventArgs e)
        {
            // L?y ph??ng th?c thanh toán ???c ch?n t? picker và l?u vào bi?n toàn c?c
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

            try
            {
                string address = AddressEntry.Text;
                string recipientName = NameEntry.Text;
                string recipientPhone = PhoneEntry.Text;

                // Ki?m tra xem các tr??ng có ???c ?i?n ??y ?? không
                if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(recipientName) || string.IsNullOrEmpty(recipientPhone))
                {
                    await DisplayAlert("Alert", "Please fill in all required fields.", "OK");
                    return;
                }

                int userId = Preferences.Get("UserID", 0);
                double total = double.Parse(TotalOrders.Text.Replace("$", ""));
                DateTime date = DateTime.Now;

                // G?i yêu c?u t?o ??n hàng ??n API
                var orderId = await CreateOrder(userId, total, date);

                if (orderId.HasValue)
                {
                    // Duy?t qua danh sách s?n ph?m trong gi? hàng và t?o chi ti?t ??n hàng
                    foreach (var cartItem in _cartItems)
                    {
                        // T?o m?i m?t ??i t??ng OrderDetail v?i các thông tin c?n thi?t
                        var orderDetail = new OrderDetailsModel
                        {
                            OrderID = orderId.Value,
                            PetId = cartItem.Pet.ID,
                            Quantity = cartItem.Quantity,
                            unit = "$", // ??n v?
                            payment = _selectedPaymentMethod, // Ph??ng th?c thanh toán
                            Address = address, // ??a ch? giao hàng
                            RecipientName = recipientName, // Tên ng??i nh?n
                            RecipientPhone = recipientPhone // S? ?i?n tho?i ng??i nh?n
                        };

                        // G?i yêu c?u t?o OrderDetail lên API
                        await CreateOrderDetail(orderDetail);

                        // Xóa s?n ph?m kh?i gi? hàng
                        await _petServices.DeletePet(cartItem.UserID, cartItem.PetID);
                    }

                    await DisplayAlert("Success", "Order placed successfully!", "OK");
                    await Navigation.PushAsync(new Orders());
                }
                else
                {
                    await DisplayAlert("Error", "Failed to place order. Please try again later.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


        private async Task<int?> CreateOrder(int userId, double total, DateTime date)
        {
            var httpClient = new HttpClient();
            //var apiUrl = $"https://localhost:7104/api/Orders?userId={userId}&total={total}&date={date}";
            string apiUrl = string.Format(Config.ApiConfig.BaseUrl+"/Orders?userId={0}&total={1}&date={2}",
                              Uri.EscapeDataString(userId.ToString()), Uri.EscapeDataString(total.ToString()),
                              Uri.EscapeDataString(date.ToString("yyyy-MM-dd")));

            var response = await httpClient.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                var orderContent = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(orderContent);
                return order?.Id;
            }
            else
            {
                return null;
            }
        }

        private async Task CreateOrderDetail(OrderDetailsModel orderDetail)
        {
           

            var httpClient = new HttpClient();
            var orderDetailsJson = JsonConvert.SerializeObject(orderDetail);
            var orderDetailsContent = new StringContent(orderDetailsJson, Encoding.UTF8, "application/json");
            var orderDetailsResponse = await httpClient.PostAsync(Config.ApiConfig.BaseUrl + "/OrderDetails", orderDetailsContent);

            if (!orderDetailsResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", "T?o ??n hàng b? l?i", "OK");
            }
        }

        private async void GotoBack(object sender, TappedEventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}