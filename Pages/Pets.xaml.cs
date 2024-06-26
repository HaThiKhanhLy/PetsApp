using Newtonsoft.Json;
using PetsApp.Repository;
using PetsApp.ViewModel;


namespace PetsApp.Pages
{
    public partial class Pets : ContentPage
    {
        private PetsVM _vm;

        public Pets()
        {
            InitializeComponent();
            _vm = new PetsVM(new TypePetsServices(), new PetServices());
            BindingContext = _vm; // Sử dụng _vm làm BindingContext
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.LoadPets();
        }

        //private async void Goback(object sender, TappedEventArgs e)
        //{
        //    await Shell.Current.Navigation.PopAsync();
        //}

        private async void OnPetItemTapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            var petModel = image?.BindingContext as PetsModel;

            if (petModel != null)
            {
                int petID = petModel.ID;

                // Chuyển hướng đến trang PetDetails với ID của thú cưng
                await Navigation.PushAsync(new PetDetails(petID));
            }
        }
        private async void OnAddToCartTapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            var petModel = image?.BindingContext as PetsModel;

            if (petModel != null)
            {
                int petID = petModel.ID;

                // Gọi hàm CreateCart với ID của thú cưng
                await CreateOrUpdateCart(petID);
            }
        }
        private async void GotoDetails(object sender, TappedEventArgs e)
        {
            var petId = (int)((TappedEventArgs)e).Parameter;
            await Navigation.PushAsync(new PetDetails(petId));
        }

        private async void OnPetItemTapped(object sender, TappedEventArgs e)
        {
            var petId = (int)((TappedEventArgs)e).Parameter;
            await Navigation.PushAsync(new PetDetails(petId));
        }
        private async Task CreateOrUpdateCart(int petId)
        {
            try
            {
                // Lấy userId từ Preferences hoặc từ nơi lưu trữ thông tin đăng nhập khác
                int userId = Preferences.Get("UserID", 0);

                // Gọi API để lấy danh sách các mục trong giỏ hàng của người dùng
                //string cartUrl = $"https://localhost:7104/api/Cart/{userId}";
                var cartUrl = string.Format(Config.ApiConfig.BaseUrl + "/Cart/{0}", userId);
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage cartResponse = await client.GetAsync(cartUrl);
                    if (cartResponse.IsSuccessStatusCode)
                    {
                        string cartContent = await cartResponse.Content.ReadAsStringAsync();
                        var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartContent);

                        // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
                        var existingCartItem = cartItems.FirstOrDefault(item => item.PetID == petId);
                        if (existingCartItem != null)
                        {
                            // Nếu sản phẩm đã có trong giỏ hàng, thì tăng số lượng lên
                            existingCartItem.Quantity++;

                            // Gửi yêu cầu PUT để cập nhật số lượng
                            //string updateUrl = $"https://localhost:7104/api/Cart/{userId}/{petId}";
                            string updateUrl = string.Format(Config.ApiConfig.BaseUrl + "/Cart/{0}/{1}", userId, petId);
                            StringContent updateContent = new StringContent(existingCartItem.Quantity.ToString(), System.Text.Encoding.UTF8, "application/json");
                            HttpResponseMessage updateResponse = await client.PutAsync(updateUrl, updateContent);
                            if (updateResponse.IsSuccessStatusCode)
                            {
                                await DisplayAlert("Success", "Cart updated successfully", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Error", "Failed to update cart", "OK");
                            }
                        }
                        else
                        {
                            // Nếu sản phẩm chưa có trong giỏ hàng, thì tạo mới
                            CartItem cart = new CartItem
                            {
                                PetID = petId,
                                UserID = userId,
                                Quantity = 1
                            };

                            // Chuyển đối tượng Cart thành JSON
                            string jsonCart = JsonConvert.SerializeObject(cart);

                            // Tạo nội dung của request là JSON
                            StringContent createContent = new StringContent(jsonCart, System.Text.Encoding.UTF8, "application/json");

                            // Gửi yêu cầu POST để tạo mới giỏ hàng
                            string createUrl = "https://localhost:7104/api/Cart/";
                            HttpResponseMessage createResponse = await client.PostAsync(createUrl, createContent);
                            if (createResponse.IsSuccessStatusCode)
                            {
                                await DisplayAlert("Success", "Cart created successfully", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Error", "Failed to create cart", "OK");
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to retrieve cart items", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ nếu có
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

    }
}
