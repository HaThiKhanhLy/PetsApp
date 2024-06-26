using Microsoft.IdentityModel.Tokens;

namespace PetsApp.Pages.Admin;

public partial class PetDetailAdmin : ContentPage
{
    public PetDetailAdmin(PetsModel petDetail)
    {
        InitializeComponent();
        BindingContext = petDetail; // Set the BindingContext to the petDetail object

        // Set the value for the Stock label if available
        if (!string.IsNullOrEmpty(petDetail.NamePets))
        {
            
            NamePets.Text = petDetail.NamePets;
            PriceLabel.Text = petDetail.Price.ToString();
            SpeciesLabel.Text = petDetail.Species;
            DescriptionLabel.Text = petDetail.Description;
            GenderLabel.Text = petDetail.Gender;
        }
    }
    private async void Goback(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}