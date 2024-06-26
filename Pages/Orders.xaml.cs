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
            // URL c?a API l?y danh s�ch ??n h�ng
            var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/Orders/{0}", userId);
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderModel>>(content);

                // G�n danh s�ch ??n h�ng l�m ngu?n d? li?u cho ListView
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

                // G?i API ?? l?y chi ti?t ??n h�ng d?a tr�n orderId
                var httpClient = new HttpClient();
                //var apiUrl = $"https://localhost:7104/api/OrderDetails/{orderId}";
                var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/OrderDetails/{0}", orderId);
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orderDetails = JsonConvert.DeserializeObject<List<OrderDetailsModel>>(content);

                    // Chuy?n h??ng sang trang OrderDetails v� truy?n danh s�ch chi ti?t ??n h�ng
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