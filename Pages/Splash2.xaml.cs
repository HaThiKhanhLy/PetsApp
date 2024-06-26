namespace PetsApp.Pages;

public partial class Splash2 : ContentPage
{
	public Splash2()
	{
		InitializeComponent();
	}

    async void BtnContinue(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new Splash3());
    }

    private async void BtnBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
    }
}