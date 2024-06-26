using PetsApp.Repository;

namespace PetsApp.ViewModel
{
    public partial class HomeVM : ObservableObject
    {
        private readonly TypePetsServices _typePetsServices;
        private readonly BannerServices _bannerServices;
        private readonly PetServices _petServices;

        public ObservableCollection<PetsModel> Pets { get; set; }

        public ObservableCollection<TypePets> TypePets { get; set; }
        public ObservableCollection<Banner> Banners { get; set; }
        public ObservableCollection<CartItem> CartItems { get; set; } = new ObservableCollection<CartItem>();

        // Define AddToCartCommand outside the constructor
        public Command AddToCartCommand { get; }

        public HomeVM(TypePetsServices typePetsServices, BannerServices bannerServices, PetServices petServices)
        {
            _typePetsServices = typePetsServices;
            _bannerServices = bannerServices;
            _petServices = petServices;

            TypePets = new ObservableCollection<TypePets>(_typePetsServices.GetAllTypePets());
            Banners = new ObservableCollection<Banner>();
            Pets = new ObservableCollection<PetsModel>();

            // Initialize AddToCartCommand with the logic for adding items to cart
            AddToCartCommand = new Command(async (pet) =>
            {
                // Logic thêm thú cưng vào giỏ hàng

                // Ví dụ sử dụng:
                var existingItem = CartItems.FirstOrDefault(ci => ci.Pet.ID == ((PetsModel)pet).ID);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    CartItem newItem = new CartItem
                    {
                        Pet = (PetsModel)pet,
                        Quantity = 1
                    };
                    CartItems.Add(newItem);
                }

                // UI sẽ tự động cập nhật do thông báo ObservableCollection
            });
        }

        public async Task LoadPets()
        {
            var pets = await _petServices.GetAllPets();
            if (pets != null)
            {
                foreach (var pet in pets)
                {
                    Pets.Add(pet);
                }
            }
        }

        public async Task LoadBanner()
        {
            var banners = await _bannerServices.GetAllBanner();
            if (banners != null)
            {
                foreach (var banner in banners)
                {
                    Banners.Add(banner);
                }
            }
        }
    }
}
