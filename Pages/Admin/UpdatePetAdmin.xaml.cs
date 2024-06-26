using Newtonsoft.Json;
using System.Text;

namespace PetsApp.Pages.Admin;

public partial class UpdatePetAdmin : ContentPage
{
    private PetsModel selectedPet;
    private int selectedPetTypeId = -1; // Thêm bi?n ?? l?u tr? ID c?a lo?i thú c?ng
    private IEnumerable<TypePets> typePets;
    public UpdatePetAdmin(PetsModel selectedPet)
    {
        InitializeComponent();
        this.selectedPet = selectedPet;

        // Hi?n th? thông tin c?a pet lên form
        if (selectedPet != null)
        {
            UpdateFormWithPetData(selectedPet);
        }

        // Load danh sách lo?i thú c?ng và hi?n th? tên trong Picker
        LoadTypePets();
    }
    public async Task<IEnumerable<TypePets>> GetTypePets()
    {
        var client = new HttpClient();
        string url = Config.ApiConfig.BaseUrl + "/PetTypes/";
        client.BaseAddress = new Uri(url);

        HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            IEnumerable<TypePets> typePets = JsonConvert.DeserializeObject<IEnumerable<TypePets>>(jsonResponse);
            return typePets;
        }
        else
        {
            Console.WriteLine("Không th? l?y danh sách các lo?i thú c?ng thông qua API. Mã tr?ng thái: " + response.StatusCode);
            return null;
        }
    }

    private async void LoadTypePets()
    {
        typePets = await GetTypePets(); // L?u tr? danh sách lo?i thú c?ng khi nó ???c t?i
        if (typePets != null)
        {
            // Thi?t l?p ItemsSource cho Picker v?i danh sách tên lo?i thú c?ng
            PetTypePicker.ItemsSource = typePets.Select(tp => tp.Name).ToList();

            // N?u selectedPetTypeID ?ã ???c thi?t l?p tr??c ?ó, ch?n lo?i thú c?ng t??ng ?ng trong Picker
            if (selectedPet != null)
            {
                PetTypePicker.SelectedItem = typePets.FirstOrDefault(tp => tp.ID == selectedPet.PetsTypeID)?.Name;
            }
        }
    }


    private void PetTypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        if (picker.SelectedItem != null)
        {
            var selectedTypeName = (string)picker.SelectedItem;
            if (typePets != null)
            {
                // Tìm lo?i thú c?ng ???c ch?n d?a trên tên
                var selectedTypePet = typePets.FirstOrDefault(tp => tp.Name == selectedTypeName);
                if (selectedTypePet != null)
                {
                    // L?u ID c?a lo?i thú c?ng ???c ch?n vào bi?n selectedPetTypeId
                    selectedPetTypeId = selectedTypePet.ID;
                    Console.WriteLine("Selected TypePet ID: " + selectedPetTypeId);
                }
            }
        }
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        // G?i ph??ng th?c c?p nh?t v?i ID c?a pet ???c ch?n
        var success = await UpdateButton_Clicked(selectedPet.ID);

        // Hi?n th? thông báo d?a trên k?t qu? c?a ph??ng th?c c?p nh?t
        if (success)
        {
            await DisplayAlert("Success", "Pet updated successfully!", "OK");
            await Navigation.PushAsync(new ListPetAdmin());
        }
        else
        {
            await DisplayAlert("Error", "Failed to update pet.", "OK");
        }
    }

    private async Task<bool> UpdateButton_Clicked(int petId)
    {
        try
        {
            var client = new HttpClient();
            //var apiUrl = $"https://localhost:7104/api/Pet/{petId}";
            var apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/Pet/{0}", petId);
            // T?o ??i t??ng m?i c?a PetsModel v?i thông tin ?ã nh?p
            PetsModel updatedPet = new PetsModel
            {
                ID = petId,
                NamePets = NamePetsEntry.Text,
                PetsTypeID = selectedPetTypeId, // S? d?ng selectedPetTypeId (ID c?a lo?i thú c?ng)
                Gender = GenderEntryU.Text,
                Size = double.Parse(SizeEntryU.Text),
                Age = int.Parse(AgeEntryU.Text),
                Description = DescriptionEntryU.Text,
                Species = SpeciesEntryU.Text,
                Image = ImageEntryU.Text,
                Price = int.Parse(PriceEntryU.Text),
                Stock = int.Parse(StockEntryU.Text),
                Unit = int.Parse(UnitEntryU.Text)
            };

            // G?i yêu c?u c?p nh?t pet
            var response = await client.PutAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(updatedPet), Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return false;
        }
    }

    private void UpdateFormWithPetData(PetsModel pet)
    {
        NamePetsEntry.Text = pet.NamePets;
        GenderEntryU.Text = pet.Gender;
        SizeEntryU.Text = pet.Size.ToString();
        AgeEntryU.Text = pet.Age.ToString();
        DescriptionEntryU.Text = pet.Description;
        SpeciesEntryU.Text = pet.Species;
        ImageEntryU.Text = pet.Image;
        PriceEntryU.Text = pet.Price.ToString();
        StockEntryU.Text = pet.Stock.ToString();
        UnitEntryU.Text = pet.Unit.ToString();
        // C?p nh?t các tr??ng d? li?u khác t??ng t?
    }

    private async void Goback(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}
