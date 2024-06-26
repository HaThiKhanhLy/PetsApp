using Newtonsoft.Json;

namespace PetsApp.Pages.Admin;

public partial class ListUserAdmin : ContentPage
{
	public ListUserAdmin()
	{
		InitializeComponent();
        GetUsersAsync();

    }
    private async void GetUsersAsync()
    {
        try
        {
            var httpClient = new HttpClient();
            var apiUrl =Config.ApiConfig.BaseUrl+ "/User"; // Thay đổi URL của API tương ứng
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserModels>>(content);
                UserListView.ItemsSource= users;
                
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
    private async void Goback(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

}