using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PetsApp.Repository;
using PetsApp.ViewModel;

namespace PetsApp.Pages
{
    public partial class Cart : ContentPage
    {
        //private readonly CartVM _cartVM;
        private readonly PetServices _petServices;
        private readonly CartServices _cartServices;

        public Cart()
        {
            InitializeComponent();
            //_cartVM = new CartVM();
            _petServices = new PetServices();
            _cartServices = new CartServices();
            BindingContext = this;

            LoadCart();
        }

        private async void LoadCart()
        {
            try
            {
                int userId = Preferences.Get("UserID", 0);
                var httpClient = new HttpClient();
                var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/Cart/{0}", userId);
                
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(content);
                        CartListView.ItemsSource = cartItems;
                    }
                    catch (JsonException)
                    {
                        // Xử lý khi dữ liệu không phải là JSON
                        await DisplayAlert("Lỗi", "Dữ liệu không hợp lệ.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "Cart is Empty.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }



        private async void DecreaseQuantityClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItem)button.BindingContext;

            if (cartItem.Quantity > 1)
            {
                // Giảm số lượng
                cartItem.Quantity--;

                // Gọi API trực tiếp
                //string url = $"https://localhost:7104/api/Cart/{cartItem.UserID}/{cartItem.PetID}";
                string url = string.Format(Config.ApiConfig.BaseUrl + "/Cart/{0}/{1}", cartItem.UserID, cartItem.PetID);

                await UpdateCartItemQuantity(url, cartItem.Quantity);
            }
        }

        private async void IncreaseQuantityClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItem)button.BindingContext;

            // Tăng số lượng
            cartItem.Quantity++;

            // Gọi API trực tiếp
            //string url = $"https://localhost:7104/api/Cart/{cartItem.UserID}/{cartItem.PetID}";
            string url = string.Format(Config.ApiConfig.BaseUrl+"/Cart/{0}/{1}", cartItem.UserID,cartItem.PetID);

            await UpdateCartItemQuantity(url, cartItem.Quantity);
        }

        private async Task UpdateCartItemQuantity(string url, int quantity)
        {
            try
            {
                var httpClient = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(quantity), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Load lại dữ liệu từ API để cập nhật CartVM
                    LoadCart();
                }
                else
                {
                    await DisplayAlert("Error", $"Failed to update cart item quantity. Status code: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void RemoveItemTapped(object sender, TappedEventArgs e)
        {
            var image = (Image)sender;
            var cartItem = (CartItem)image.BindingContext;

            try
            {
                // Hiển thị thông báo xác nhận
                var result = await DisplayAlert("Delete cart", "Are you sure you want to delete this product?", "OK", "Cancel");

                if (result)
                {
                    // Gọi phương thức DeletePet từ PetServices để xóa sản phẩm
                    await _petServices.DeletePet(cartItem.UserID, cartItem.PetID);

                    //// Hiển thị thông báo sản phẩm đã được xóa
                    //await DisplayAlert("Thông báo", "Sản phẩm đã được xóa thành công.", "OK");

                    // Load lại danh sách sản phẩm trong giỏ hàng
                    LoadCart();
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý ngoại lệ khi mất kết nối mạng
                await DisplayAlert("Lỗi", "Mất kết nối mạng, vui lòng kiểm tra lại kết nối của bạn", "OK");
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác
                await DisplayAlert("Lỗi", $"Đã xảy ra lỗi: {ex.Message}", "OK");
            }
        }
        private async void CheckoutClicked(object sender, EventArgs e)
        {
            // Lấy danh sách các mục trong giỏ hàng
            var cartItems = (List<CartItem>)CartListView.ItemsSource;

            if (cartItems != null && cartItems.Count > 0)
            {
                // Chuyển sang trang Checkout và chuyển danh sách các mục đã chọn sang trang đó
                await Navigation.PushAsync(new Checkout1(cartItems));
            }
            else
            {
                await DisplayAlert("Alert", "Your cart is empty. Please add items to checkout.", "OK");
            }
        }

        private async void Goback(object sender, TappedEventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while navigating back: {ex.Message}", "OK");
            }
        }

        private async void GobackHome(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("Home");
        }
    }
}
