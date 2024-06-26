using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using PetsApp.Models;
using System.Text;
using PetsApp.Repository; // Import CartItem model if necessary

namespace PetsApp.Services
{
    public class CartServices : ICartReponsitory
    {
        public async Task<CartItem> UpdateQuantity(int userId, int petId, int newQuantity)
        {
            var client = new HttpClient();

            // Tạo đối tượng chứa số lượng mới
            var quantityData = new
            {
                newQuantity
            };

            // Chuyển đối tượng thành chuỗi JSON
            var jsonQuantityData = JsonConvert.SerializeObject(quantityData);

            // Tạo nội dung yêu cầu
            var content = new StringContent(jsonQuantityData, Encoding.UTF8, "application/json");

            // Gửi yêu cầu PUT đến API
            //string url = $"https://localhost:7104/api/Cart/{userId}/{petId}";
            string url = string.Format(Config.ApiConfig.BaseUrl + "/Cart/{0}/{1}", userId, petId);
            var response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Đọc nội dung phản hồi
                var responseContent = await response.Content.ReadAsStringAsync();

                // Chuyển đổi nội dung phản hồi thành đối tượng CartItem
                var updatedCartItem = JsonConvert.DeserializeObject<CartItem>(responseContent);

                return updatedCartItem;
            }
            else
            {
                Console.WriteLine($"Không thể cập nhật số lượng cho pet có ID {petId} trong giỏ hàng của người dùng có ID {userId}. Mã trạng thái: {response.StatusCode}");
                return null;
            }
        }
    }
}
