using Newtonsoft.Json;
using PetsApp.Repository;

namespace PetsApp.Pages.Admin
{
    public partial class CreatePetAdmin : ContentPage
    {
        PetServices petServices;
        private int selectedPetTypeId = -1;
        private IEnumerable<TypePets> typePets; // Biến toàn cục để lưu trữ danh sách loại thú cưng

        public CreatePetAdmin()
        {
            InitializeComponent();
            petServices = new PetServices();

            // Gọi phương thức để lấy danh sách các loại thú cưng và gán cho Picker
            LoadTypePets();
        }

        private async void LoadTypePets()
        {
            typePets = await GetTypePets(); // Lưu trữ danh sách loại thú cưng khi nó được tải
            if (typePets != null)
            {
                List<string> typePetNames = typePets.Select(tp => tp.Name).ToList();
                PetTypePicker.ItemsSource = typePetNames;
            }
        }

        public async Task<IEnumerable<TypePets>> GetTypePets()
        {
            var client = new HttpClient();
            string url = Config.ApiConfig.BaseUrl+"/PetTypes/";
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
                Console.WriteLine("Không thể lấy danh sách các loại thú cưng thông qua API. Mã trạng thái: " + response.StatusCode);
                return null;
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
                    // Tìm loại thú cưng được chọn dựa trên tên
                    var selectedTypePet = typePets.FirstOrDefault(tp => tp.Name == selectedTypeName);
                    if (selectedTypePet != null)
                    {
                        // Lưu ID của loại thú cưng được chọn vào biến selectedPetTypeId
                        selectedPetTypeId = selectedTypePet.ID;
                        Console.WriteLine("Selected TypePet ID: " + selectedPetTypeId);
                    }
                }
            }
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            // Check if all required fields are filled
            if (string.IsNullOrWhiteSpace(NamePetsEntry.Text) ||
                selectedPetTypeId == -1 ||
                string.IsNullOrWhiteSpace(GenderEntry.Text) ||
                string.IsNullOrWhiteSpace(SizeEntry.Text) ||
                string.IsNullOrWhiteSpace(AgeEntry.Text) ||
                string.IsNullOrWhiteSpace(DescriptionEntry.Text) ||
                string.IsNullOrWhiteSpace(SpeciesEntry.Text) ||
                string.IsNullOrWhiteSpace(ImageEntry.Text) ||
                string.IsNullOrWhiteSpace(PriceEntry.Text) ||
                string.IsNullOrWhiteSpace(StockEntry.Text) ||
                string.IsNullOrWhiteSpace(UnitEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            // Check if Size, Age, Price, Stock, and Unit are valid numbers
            if (!double.TryParse(SizeEntry.Text, out double size) ||
                !int.TryParse(AgeEntry.Text, out int age) ||
                !int.TryParse(PriceEntry.Text, out int price) ||
                !int.TryParse(StockEntry.Text, out int stock) ||
                !int.TryParse(UnitEntry.Text, out int unit))
            {
                await DisplayAlert("Error", "Please enter valid numeric values for Size, Age, Price, Stock, and Unit.", "OK");
                return;
            }

            // Create a new PetsModel object with the entered information
            PetsModel newPet = new PetsModel
            {
                NamePets = NamePetsEntry.Text,
                PetsTypeID = selectedPetTypeId,
                Gender = GenderEntry.Text,
                Size = size,
                Age = age,
                Description = DescriptionEntry.Text,
                Species = SpeciesEntry.Text,
                Image = ImageEntry.Text,
                Price = price,
                Stock = stock,
                Unit = unit
            };

            // Call the CreatePet method to create a new pet
            PetsModel createdPet = await petServices.CreatePet(newPet);

            // Display a message based on the result of the pet creation
            if (createdPet != null)
            {
                await DisplayAlert("Success", "Pet created successfully!", "OK");
                await Navigation.PushAsync(new ListPetAdmin());
            }
            else
            {
                await DisplayAlert("Error", "Failed to create pet.", "OK");
            }
        }

        private async void Goback(object sender, TappedEventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

}
