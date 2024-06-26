using Newtonsoft.Json;
using Syncfusion.Maui.DataForm;
using System.Text;

namespace PetsApp.Pages;

public partial class MyProfile : ContentPage
{
    private static readonly HttpClient client = new HttpClient();
    public MyProfile()
	{
		InitializeComponent();
        GetUserProfile();
        LoadProvinces();
    }

    private async void Gotoback(object sender, TappedEventArgs e)
    {
		await Shell.Current.Navigation.PopAsync();
        ;
    }

    private async void Goback(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        // L?y ID ng??i d�ng t? Preferences
        int userId = Preferences.Get("UserID", -1);

        // Ki?m tra xem ID c� h?p l? kh�ng
        if (userId == -1)
        {
            // X? l� tr??ng h?p kh�ng t�m th?y ID
            return;
        }

        // T?o ??i t??ng UserModels v� g�n c�c gi� tr? t? c�c tr??ng tr�n giao di?n
        UserModels updatedUser = new UserModels
        {
            ID = userId,
            FullName = FullNameEntry.Text,
            Phone = PhoneEntry.Text,
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text,
            Address = (AddressPicker.SelectedItem as Province)?.Name,
            DayOfBirth = DateOfBirthPicker.Date,
            Gender = GenderPicker.SelectedItem as string,
            Role="User",
            Status= "0"
        };

        // G?i d? li?u c?p nh?t th�ng qua API
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Chuy?n ??i ??i t??ng updatedUser th�nh JSON
                string json = JsonConvert.SerializeObject(updatedUser);

                // T?o n?i dung y�u c?u
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // G?i API ?? c?p nh?t th�ng tin ng??i d�ng
                var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/User/updateUser/{0}", userId);
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                // Ki?m tra xem y�u c?u c� th�nh c�ng kh�ng
                if (response.IsSuccessStatusCode)
                {
                    
                    // Hi?n th? th�ng b�o th�nh c�ng (ho?c x? l� t�y �)
                    await DisplayAlert("Success", "User profile updated successfully", "OK");
                }
                else
                {
                    // X? l� tr??ng h?p kh�ng th�nh c�ng khi g?i API
                    // V� d?: Hi?n th? th�ng b�o l?i
                    await DisplayAlert("Error", "Unable to update user profile. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
    private async void LoadProvinces()
    {
        try
        {
            // Adjust the URL as per your API endpoint
            string url = Config.ApiConfig.BaseUrl + "/Province";
            var response = await client.GetStringAsync(url);
            var provinces = JsonConvert.DeserializeObject<List<Province>>(response);

            // Bind the list of provinces to the Picker
            AddressPicker.ItemsSource = provinces;
            AddressPicker.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load provinces: {ex.Message}", "OK");
        }
    }
    private async void GetUserProfile()
    {
        // L?y ID ng??i d�ng t? Preferences
        int userId = Preferences.Get("UserID", -1);

        // Ki?m tra xem ID c� h?p l? kh�ng
        if (userId == -1)
        {
            // X? l� tr??ng h?p kh�ng t�m th?y ID
            return;
        }

        // T?o HttpClient
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/User/{0}", userId);
                // G?i API ?? l?y th�ng tin ng??i d�ng
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Ki?m tra xem y�u c?u c� th�nh c�ng kh�ng
                if (response.IsSuccessStatusCode)
                {
                    // ??c n?i dung c?a ph?n h?i
                    string content = await response.Content.ReadAsStringAsync();

                    // Chuy?n ??i JSON th�nh ??i t??ng User
                    UserModels user = JsonConvert.DeserializeObject<UserModels>(content);

                    // G�n th�ng tin ng??i d�ng v�o c�c tr??ng tr�n giao di?n
                    FullNameEntry.Text = user.FullName;
                    PhoneEntry.Text = user.Phone;
                    EmailEntry.Text = user.Email;
                    PasswordEntry.Text = user.Password;
                    //AddressEntry.Text = user.Address;
                    Preferences.Set("Address1", user.Address);

                    // Ki?m tra v� g�n gi?i t�nh ng??i d�ng v�o Picker
                    if (user.Gender != null)
                    {
                        GenderPicker.SelectedItem = user.Gender;
                    }

                    // X? l� ng�y sinh ng??i d�ng
                    if (user.DayOfBirth != null)
                    {
                        DateOfBirthPicker.Date = user.DayOfBirth.Value;
                    }
                    var provinces = AddressPicker.ItemsSource as List<Province>;
                    if (provinces != null)
                    {
                        var selectedProvince = provinces.FirstOrDefault(p => p.Name == user.Address);
                        AddressPicker.SelectedItem = selectedProvince;
                    }
                }
                else
                {
                    // X? l� tr??ng h?p kh�ng th�nh c�ng khi g?i API
                    // V� d?: Hi?n th? th�ng b�o l?i
                    await DisplayAlert("Error", "Unable to proceed with checkout. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}