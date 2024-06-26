namespace PetsApp.Pages;

public partial class SetPassword : ContentPage
{
	public SetPassword()
	{
		InitializeComponent();
	}

    private async void SaveNewPassword(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new SecureSuccess());
    }
}