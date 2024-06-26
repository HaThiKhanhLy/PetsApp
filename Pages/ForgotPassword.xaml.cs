namespace PetsApp.Pages;

public partial class ForgotPassword : ContentPage
{
	public ForgotPassword()
	{
		InitializeComponent();
	}

    private async void SendOTP(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new OTPCode());
    }
}