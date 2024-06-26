using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PetsApp.Models;
using PetsApp.Pages;
using PetsApp.Repository;

namespace PetsApp.ViewModel
{
    public partial class PetsVM : ObservableObject
    {
        private readonly PetServices _petServices;

        public ObservableCollection<PetsModel> Pets { get; set; }
        public ObservableCollection<TypePets> TypePets { get; set; }
        private int _id;
        private string _name;
        private decimal _price;
        private string _species;
        private string _description;
        private string _image;
        private int _age;
        private string _gender;
        private double _size;
        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string NamePets
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string Species
        {
            get => _species;
            set => SetProperty(ref _species, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        public double Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }
        public PetsVM(TypePetsServices typePetsServices, PetServices petServices)
        {
            _petServices = petServices;
            TypePets = new ObservableCollection<TypePets>(typePetsServices.GetAllTypePets());
            Pets = new ObservableCollection<PetsModel>();
            
        }

  

        public async Task LoadPets()
        {
            var pets = await _petServices.GetAllPets();
            if (pets != null)
            {
                foreach (var pet in pets)
                {
                    Pets.Add(new PetsModel
                    {
                        ID=pet.ID,
                        NamePets = pet.NamePets,
                        Price = pet.Price,
                        Species = pet.Species,
                        Image=pet.Image,
                        Description = pet.Description,
                        Gender = pet.Gender,
                        Age = pet.Age
                        // Đảm bảo cập nhật các thuộc tính khác của PetsModel tại đây nếu cần
                    });
                }
            }
        }
        public async Task<PetsModel> LoadPetDetails(int petId)
        {
            try
            {
                var petDetails = await _petServices.GetPetById(petId); // Lấy chi tiết về thú cưng từ cơ sở dữ liệu hoặc nguồn dữ liệu khác
                if (petDetails != null)
                {
                    // Cập nhật các thuộc tính của ViewModel với chi tiết về thú cưng
                    ID = petDetails.ID;
                    NamePets = petDetails.NamePets;
                    Price = petDetails.Price;
                    Species = petDetails.Species;
                    Description = petDetails.Description;
                    Image = petDetails.Image;
                    Age = petDetails.Age;
                    Gender = petDetails.Gender;
                    Size = petDetails.Size;

                    return petDetails; // Trả về đối tượng PetsModel đã được cập nhật
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy thú cưng
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                Console.WriteLine($"An error occurred while loading pet details: {ex.Message}");
                return null;
            }
        }


    }
}
