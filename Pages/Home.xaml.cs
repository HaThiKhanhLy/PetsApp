using Newtonsoft.Json;
using PetsApp.Repository;
using PetsApp.ViewModel;

namespace PetsApp.Pages;

public partial class Home : ContentPage
{
	private readonly HomeVM _homeVM;
    
    
    public ObservableCollection<Pets> PetsCollection { get; set; }

    public Home(HomeVM homeVM)
	{
		InitializeComponent();
		_homeVM = homeVM;
		BindingContext = _homeVM;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _homeVM.LoadBanner();
        await _homeVM.LoadPets();
    }

    private async void GotoSearch(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new Search());

    }

   /* private async void SeeAllPets(object sender, TappedEventArgs e)
    {
		await Navigation.PushAsync(new Pets(_petsVM));
    }*/

    private async void GotoCart(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Cart());
    }

    private async void SeeAllPets(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Pets());
    }
    private async void GotoDetails(object sender, TappedEventArgs e)
    {
        var petId = (int)((TappedEventArgs)e).Parameter;
        await Navigation.PushAsync(new PetDetails(petId));
    }

    private async void OnAddToCartTapped(object sender, TappedEventArgs e)
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

    private async Task CreateOrUpdateCart(int petId)
    {
        try
        {
            // L?y userId t? Preferences ho?c t? n?i l?u tr? thông tin ??ng nh?p khác
            int userId = Preferences.Get("UserID", 0);

            // G?i API ?? l?y danh sách các m?c trong gi? hàng c?a ng??i dùng
            //string cartUrl = $"https://localhost:7104/api/Cart/{userId}";
            var cartUrl = string.Format(Config.ApiConfig.BaseUrl+"/Cart/{0}", userId);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage cartResponse = await client.GetAsync(cartUrl);
                if (cartResponse.IsSuccessStatusCode)
                {
                    string cartContent = await cartResponse.Content.ReadAsStringAsync();
                    var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartContent);

                    // Ki?m tra xem s?n ph?m ?ã có trong gi? hàng ch?a
                    var existingCartItem = cartItems.FirstOrDefault(item => item.PetID == petId);
                    if (existingCartItem != null)
                    {
                        // N?u s?n ph?m ?ã có trong gi? hàng, thì t?ng s? l??ng lên
                        existingCartItem.Quantity++;

                        // G?i yêu c?u PUT ?? c?p nh?t s? l??ng
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
                        // N?u s?n ph?m ch?a có trong gi? hàng, thì t?o m?i
                        CartItem cart = new CartItem
                        {
                            PetID = petId,
                            UserID = userId,
                            Quantity = 1
                        };

                        // Chuy?n ??i t??ng Cart thành JSON
                        string jsonCart = JsonConvert.SerializeObject(cart);

                        // T?o n?i dung c?a request là JSON
                        StringContent createContent = new StringContent(jsonCart, System.Text.Encoding.UTF8, "application/json");

                        // G?i yêu c?u POST ?? t?o m?i gi? hàng
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
            // X? lý các ngo?i l? n?u có
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

}