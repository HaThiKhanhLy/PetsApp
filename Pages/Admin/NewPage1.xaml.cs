using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetsApp.Pages.Admin;

namespace PetsApp.Pages;

public partial class NewPage1 : ContentPage
{
    public List<PetsModel> Pets { get; set; }
    public NewPage1()
	{
        InitializeComponent();
        GetTotalOrder();
        GetTotalProducts();
        GetTotalRevenue();
        LoadPets();

    }


    private void ListPet_Clicked(object sender, EventArgs e)
    {

    }

    private void ListUser_Clicked(object sender, EventArgs e)
    {

    }

    private async void Logout_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Logout", "Are you sure you want to log out?", "OK", "Cancel");
        if (result)
        {
            Shell.Current.CurrentItem = new SignIn();
        }
    }

    private async void ListOrder_Clicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrdersAdmin());
    }

    private async void GotoListPet(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ListPetAdmin());
    }

    private async void GotoUserList(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ListUserAdmin());
    }
    private async void GetTotalOrder()
    {
        try
        {
            HttpClient client = new HttpClient();
            string apiUrl = Config.ApiConfig.BaseUrl + "/Orders"; // Thay th? URL API c?a b?n
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JArray orders = JArray.Parse(json);
                int totalOrder = orders.Count;
                totalOrderLabel.Text = "Total orders: " + totalOrder.ToString();
            }
            else
            {
                totalOrderLabel.Text = "Không th? l?y s? ??n hàng";
            }
        }
        catch (Exception ex)
        {
            totalOrderLabel.Text = "L?i: " + ex.Message;
        }
    }
    private async void GetTotalProducts()
    {
        try
        {
            HttpClient client = new HttpClient();
            string apiUrl = Config.ApiConfig.BaseUrl + "/Pet"; // Thay th? URL API c?a b?n
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JArray pets = JArray.Parse(json);
                int totalProducts = pets.Count;
                totalProductsLabel.Text = "Total products: " + totalProducts.ToString();
            }
            else
            {
                totalProductsLabel.Text = "Không th? l?y s? l??ng s?n ph?m";
            }
        }
        catch (Exception ex)
        {
            totalProductsLabel.Text = "L?i: " + ex.Message;
        }
    }
    private async void GetTotalRevenue()
    {
        try
        {
            HttpClient client = new HttpClient();
            string apiUrl = Config.ApiConfig.BaseUrl + "/Orders"; // Thay th? URL API c?a b?n
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JArray orders = JArray.Parse(json);

                double totalRevenue = 0;
                foreach (var order in orders)
                {
                    totalRevenue += (double)order["total"];
                }

                revenueLabel.Text = "Revenue: " + totalRevenue.ToString("C"); // Hi?n th? t?ng doanh thu d??i d?ng ti?n t?
            }
            else
            {
                revenueLabel.Text = "Không th? l?y t?ng doanh thu";
            }
        }
        catch (Exception ex)
        {
            revenueLabel.Text = "L?i: " + ex.Message;
        }
    }
    private async void LoadPets()
    {
        try
        {
            HttpClient client = new HttpClient();
            string apiUrl = Config.ApiConfig.BaseUrl + "/Pet/Top10HighestId"; // Thay ??i URL API c?a b?n
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<PetsModel> pets = JsonConvert.DeserializeObject<List<PetsModel>>(json); // S?a ??i thành List<PetsModel>
                PetFavorites.ItemsSource = pets; // S?a ??i thành pets
            }
            else
            {
                // Hi?n th? thông báo c?nh báo
                await DisplayAlert("Error", "Unable to retrieve data from the server.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Hi?n th? thông báo l?i n?u có l?i khi g?i API
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }


}