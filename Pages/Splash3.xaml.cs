namespace PetsApp.Pages;

public partial class Splash3 : ContentPage
{
	public Splash3()
	{
		InitializeComponent();
	}

    private async void BtnContinue(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new SignIn());
    }

    private async void BtnBack(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Splash2());
    }
}