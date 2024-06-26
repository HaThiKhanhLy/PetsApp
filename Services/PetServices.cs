using Newtonsoft.Json;
using PetsApp.Pages;
using System.Text;


namespace PetsApp.Repository;

public class PetServices : IPetReponsitory
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = Config.ApiConfig.BaseUrl;
    public async Task<List<PetsModel>> GetAllPets()
    {
        var client = new HttpClient();
        string endpoint = "/Pet/";
        client.BaseAddress = new Uri(Config.ApiConfig.BaseUrl+endpoint);
        HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
        if (response.IsSuccessStatusCode)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            List<PetsModel> petsList = JsonConvert.DeserializeObject<List<PetsModel>>(content);
            return petsList;

        }
        else
        {
            Console.WriteLine("Không thể lấy danh sách thú cưng từ API. Mã trạng thái: " + response.StatusCode);
            return null;
        }
    }
    public async Task<PetsModel> GetPetById(int petId)
    {
        var client = new HttpClient();
        //string url = $"https://localhost:7104/api/Pet/{petId}";
        string url = string.Format(Config.ApiConfig.BaseUrl + "/Pet/{0}", petId);
        client.BaseAddress = new Uri(url);
        HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            PetsModel pet = JsonConvert.DeserializeObject<PetsModel>(content);
            return pet;
        }
        else
        {
            Console.WriteLine($"Không thể lấy thông tin của pet có ID {petId} từ API. Mã trạng thái: " + response.StatusCode);
            return null;
        }

    }
    public async Task<bool> DeletePet(int userId, int petId)
    {
        var client = new HttpClient();
        //string url = $"https://localhost:7104/api/Cart/{userId}/{petId}";
        string url = string.Format(Config.ApiConfig.BaseUrl + "/Cart/{0}/{1}", userId, petId);
        client.BaseAddress = new Uri(url);
        HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress);

        if (response.IsSuccessStatusCode)
        {
            // Server likely returns a success message, not a CartItem object
            return true;
        }
        else
        {
            Console.WriteLine($"Không thể xóa sản phẩm có ID {petId} từ API. Mã trạng thái: " + response.StatusCode);
            return false;
        }
    }
    public async Task<PetsModel> CreatePet(PetsModel newPet)
    {
        var client = new HttpClient();
        string url = Config.ApiConfig.BaseUrl+"/Pet/";
        client.BaseAddress = new Uri(url);

        // Serialize the newPet object to JSON
        string jsonPet = JsonConvert.SerializeObject(newPet);
        var content = new StringContent(jsonPet, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);

        if (response.IsSuccessStatusCode)
        {
            // Đọc nội dung của phản hồi và chuyển đổi thành đối tượng PetsModel
            string jsonResponse = await response.Content.ReadAsStringAsync();
            PetsModel createdPet = JsonConvert.DeserializeObject<PetsModel>(jsonResponse);

            return createdPet;
        }
        else
        {
            Console.WriteLine("Không thể tạo thú cưng mới thông qua API. Mã trạng thái: " + response.StatusCode);
            return null;
        }
    }


}
