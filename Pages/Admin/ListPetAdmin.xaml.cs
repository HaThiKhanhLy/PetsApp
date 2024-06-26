using Newtonsoft.Json;

namespace PetsApp.Pages.Admin;

public partial class ListPetAdmin : ContentPage
{
    HttpClient client = new HttpClient();
    public ListPetAdmin()
	{
		InitializeComponent();
        GetAllPets();
        PetsListView.ItemSelected += OnItemSelected;

    }
    private async void GetAllPets()
    {
        try
        {
            var httpClient = new HttpClient();
            var apiUrl =Config.ApiConfig.BaseUrl+ "/Pet"; // Thay ??i URL c?a API t??ng ?ng
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var petAll = JsonConvert.DeserializeObject<List<PetsModel>>(content);
                PetsListView.ItemsSource = petAll;

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

    async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Get the selected pet from the list
        PetsModel selectedPet = e.SelectedItem as PetsModel;

        // Navigate to PetDetailAdmin page and pass the selected pet as parameter
        await Navigation.PushAsync(new PetDetailAdmin(selectedPet));

        // Deselect item
        ((ListView)sender).SelectedItem = null;
    }

    private async void NavigationCreatePet(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreatePetAdmin());
    }
    private async void StatusPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        string selectedAction = (string)picker.SelectedItem;

        if (selectedAction == "Update")
        {
            // L?y th� c?ng ???c ch?n t? picker
            PetsModel selectedPet = (PetsModel)picker.BindingContext;

            // Chuy?n h??ng ??n trang CreatePetAdmin v� truy?n th�ng tin c?a pet ?� ch?n
            await Navigation.PushAsync(new UpdatePetAdmin(selectedPet));
        }
        else if (selectedAction == "Delete")
        {
            // L?y th� c?ng ???c ch?n t? picker
            PetsModel selectedPet = (PetsModel)picker.BindingContext;

            // G?i ph??ng th?c ?? x�a th� c?ng
            bool isSuccess = await DeletePet(selectedPet.ID);

            if (isSuccess)
            {
                // X�a th�nh c�ng, c?p nh?t danh s�ch th� c?ng
                GetAllPets();
                await DisplayAlert("Success", "Pet deleted successfully.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete pet.", "OK");
            }
        }
    }

    private async Task<bool> DeletePet(int petId)
    {
        try
        {
            var apiUrl = string.Format(Config.ApiConfig.BaseUrl+"/Pet/{0}", petId);

            var response = await client.DeleteAsync(apiUrl);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return false;
        }
    }

}