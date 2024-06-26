using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Services
{
    class OrderDetailsServices
    {
        public async Task<OrderDetailsModel> GetOrderDetailById(int orderID)
        {
            var client = new HttpClient();
            //string url = $"https://localhost:7104/api/OrderDetails/{orderID}";
            string url = string.Format(Config.ApiConfig.BaseUrl + "/OrderDetails/{0}", orderID);
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                OrderDetailsModel orderDetail = JsonConvert.DeserializeObject<OrderDetailsModel>(content);
                return orderDetail;
            }
            else
            {
                Console.WriteLine($"Không thể lấy thông tin của pet có ID {orderID} từ API. Mã trạng thái: " + response.StatusCode);
                return null;
            }
        }
    }
}
