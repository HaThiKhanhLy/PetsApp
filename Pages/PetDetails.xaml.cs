using Newtonsoft.Json;
using PetsApp.Repository;
using PetsApp.ViewModel;

namespace PetsApp.Pages
{
    public partial class PetDetails : ContentPage
    {
        private readonly int _petId;
        private readonly PetsVM _petsVM;


        public PetDetails(int petID)
        {
            InitializeComponent();
            _petId = petID;
            _petsVM = new PetsVM(new TypePetsServices(), new PetServices());
            BindingContext = _petsVM;
           
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PetsVM petsVM)
            {
                await petsVM.LoadPetDetails(_petId);
            }
        }


        private async void Goback(object sender, TappedEventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }



        private async void BuyNowClicked(object sender, EventArgs e)
        {
            try
            {
                if (BindingContext is PetsVM petsVM)
                {
                    PetsModel selectedPet = await petsVM.LoadPetDetails(_petId); // Lấy thông tin thú cưng từ ViewModel
                    if (selectedPet != null)
                    {
                        await Navigation.PushAsync(new Checkout(selectedPet)); // Truyền thông tin thú cưng sang trang Checkout
                    }
                    else
                    {
                        await DisplayAlert("Error", "Unable to proceed with checkout. Please try again.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khi chuyển trang
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

      
        private async Task CreateOrUpdateCart(int petId)
        {
            try
            {
                // Lấy userId từ Preferences hoặc từ nơi lưu trữ thông tin đăng nhập khác
                int userId = Preferences.Get("UserID", 0);

                // Gọi API để lấy danh sách các mục trong giỏ hàng của người dùng
                //string cartUrl = $"https://localhost:7104/api/Cart/{userId}";
                string cartUrl = string.Format(Config.ApiConfig.BaseUrl + "/Cart/{0}", userId);
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
                            string createUrl = Config.ApiConfig.BaseUrl + "/Cart/";
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

        private async void OnAddToCartTapped(object sender, TappedEventArgs e)
        {
            var petId = (int)((TappedEventArgs)e).Parameter;
                await CreateOrUpdateCart(petId);
        }
    }
}
