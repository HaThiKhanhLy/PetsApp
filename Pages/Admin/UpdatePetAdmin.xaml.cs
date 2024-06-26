using Newtonsoft.Json;
using System.Text;

namespace PetsApp.Pages.Admin;

public partial class UpdatePetAdmin : ContentPage
{
    private PetsModel selectedPet;
    private int selectedPetTypeId = -1; // Th�m bi?n ?? l?u tr? ID c?a lo?i th� c?ng
    private IEnumerable<TypePets> typePets;
    public UpdatePetAdmin(PetsModel selectedPet)
    {
        InitializeComponent();
        this.selectedPet = selectedPet;

        // Hi?n th? th�ng tin c?a pet l�n form
        if (selectedPet != null)
        {
            UpdateFormWithPetData(selectedPet);
        }

        // Load danh s�ch lo?i th� c?ng v� hi?n th? t�n trong Picker
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
            Console.WriteLine("Kh�ng th? l?y danh s�ch c�c lo?i th� c?ng th�ng qua API. M� tr?ng th�i: " + response.StatusCode);
            return null;
        }
    }

    private async void LoadTypePets()
    {
        typePets = await GetTypePets(); // L?u tr? danh s�ch lo?i th� c?ng khi n� ???c t?i
        if (typePets != null)
        {
            // Thi?t l?p ItemsSource cho Picker v?i danh s�ch t�n lo?i th� c?ng
            PetTypePicker.ItemsSource = typePets.Select(tp => tp.Name).ToList();

            // N?u selectedPetTypeID ?� ???c thi?t l?p tr??c ?�, ch?n lo?i th� c?ng t??ng ?ng trong Picker
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
                // T�m lo?i th� c?ng ???c ch?n d?a tr�n t�n
                var selectedTypePet = typePets.FirstOrDefault(tp => tp.Name == selectedTypeName);
                if (selectedTypePet != null)
                {
                    // L?u ID c?a lo?i th� c?ng ???c ch?n v�o bi?n selectedPetTypeId
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

        // Hi?n th? th�ng b�o d?a tr�n k?t qu? c?a ph??ng th?c c?p nh?t
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
            // T?o ??i t??ng m?i c?a PetsModel v?i th�ng tin ?� nh?p
            PetsModel updatedPet = new PetsModel
            {
                ID = petId,
                NamePets = NamePetsEntry.Text,
                PetsTypeID = selectedPetTypeId, // S? d?ng selectedPetTypeId (ID c?a lo?i th� c?ng)
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

            // G?i y�u c?u c?p nh?t pet
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
        // C?p nh?t c�c tr??ng d? li?u kh�c t??ng t?
    }

    private async void Goback(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}
