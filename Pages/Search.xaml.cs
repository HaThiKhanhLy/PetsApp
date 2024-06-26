
using Newtonsoft.Json;
using PetsApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace PetsApp.Pages
{
    public partial class Search : ContentPage
    {
        public ObservableCollection<PetsModel> FoundPets { get; set; }

        public Search()
        {
            InitializeComponent();
            FoundPets = new ObservableCollection<PetsModel>();
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchBar = sender as SearchBar;
            string searchTerm = searchBar.Text;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var foundPets = await SearchPetsByName(searchTerm);
                if (foundPets != null)
                {
                    await Navigation.PushAsync(new SearchResult(foundPets));
                }
            }
            else
            {
                // Nếu ô tìm kiếm trống, ẩn danh sách sản phẩm và hiển thị nhãn "Not found pet"
                FoundPets.Clear();
                NotFoundLabel.IsVisible = true;
            }
        }



        private async Task<List<PetsModel>> SearchPetsByName(string name)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //string apiUrl = $"https://localhost:7104/api/Pet/ByName/{name}";
                    string apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/Pet/ByName/{0}", name);
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var foundPets = JsonConvert.DeserializeObject<List<PetsModel>>(content);
                        return foundPets;
                    }
                    else
                    {
                        // Xử lý trường hợp không thể gọi API thành công
                        NotFoundLabel.IsVisible = true;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ nếu có
                await DisplayAlert("Lỗi", $"An error occurred: {ex.Message}", "OK");
                return null;
            }
        }



        private async void Goback(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(Home)}");

        }

    }
}
