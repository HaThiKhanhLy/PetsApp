using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PetsApp.Repository
{
    public class UserServices : IUserRepository
    {
        public async Task<UserModels> Login(string email, string password)
        {
            var client = new HttpClient();
            string url = Config.ApiConfig.BaseUrl+"/User/" + email + "/" + password;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                UserModels user = JsonConvert.DeserializeObject<UserModels>(content);
                return await Task.FromResult(user);

            }
            return null;
        }

        public async Task<UserModels> Register(string email, string password)
        {
            var newUser = new UserModels
            {
                Email = email,
                Password = password,
                Role = "User"
            };
            string json = JsonConvert.SerializeObject(newUser);
            var client = new HttpClient();
            //string url = "https://localhost:7104/api/User/";
            var url = Config.ApiConfig.BaseUrl + "/User";
            client.BaseAddress = new Uri(url);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(client.BaseAddress, httpContent);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                UserModels registeredUser = JsonConvert.DeserializeObject<UserModels>(content);
                return registeredUser;
            }

            return null;
        }

    }
}
