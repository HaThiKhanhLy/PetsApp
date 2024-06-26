using Newtonsoft.Json;
using PetsApp.Repository;


namespace PetsApp.Services
{
    public class BannerServices: IBannerReponsitory
    {
        public async Task<List<Banner>> GetAllBanner()
        {
            var client = new HttpClient();
            string url = Config.ApiConfig.BaseUrl+"/Banner/";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                List<Banner> bannerList = JsonConvert.DeserializeObject<List<Banner>>(content);
                return bannerList;

            }
            else
            {
                Console.WriteLine("Không thể lấy danh sách thú cưng từ API. Mã trạng thái: " + response.StatusCode);
                return null;
            }
        }

      
    }
}
