using Newtonsoft.Json;
using System.Numerics;

namespace PetsApp.Pages;

public partial class Orders : ContentPage
{

    public Orders()
    {
        InitializeComponent();

        LoadOrders();


    }
    private async void LoadOrders()
    {
        try
        {
            int userId = Preferences.Get("UserID", 0);
            var httpClient = new HttpClient();
            // URL c?a API l?y danh sách ??n hàng
            var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/Orders/{0}", userId);
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderModel>>(content);

                // Gán danh sách ??n hàng làm ngu?n d? li?u cho ListView
                OrdersListView.ItemsSource = orders;
                
            }
            else
            {
                await DisplayAlert("Error", "Failed to load orders. Please try again later.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnOrderTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {
            if (e.Item is OrderModel selectedOrder)
            {
                var orderId = selectedOrder.Id;

                // G?i API ?? l?y chi ti?t ??n hàng d?a trên orderId
                var httpClient = new HttpClient();
                //var apiUrl = $"https://localhost:7104/api/OrderDetails/{orderId}";
                var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/OrderDetails/{0}", orderId);
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orderDetails = JsonConvert.DeserializeObject<List<OrderDetailsModel>>(content);

                    // Chuy?n h??ng sang trang OrderDetails và truy?n danh sách chi ti?t ??n hàng
                    await Navigation.PushAsync(new OrderDetails(orderDetails));
                }
                else
                {
                    await DisplayAlert("Error", "Failed to load order details. Please try again later.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

}