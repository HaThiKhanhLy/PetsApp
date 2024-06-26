
namespace PetsApp.Pages;

public partial class StartPage : ContentPage
{
	public StartPage()
	{
		InitializeComponent();
	}

    async void BtnContinue(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Splash2());
    }

   
}