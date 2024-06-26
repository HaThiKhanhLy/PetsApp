namespace PetsApp.Pages;

public partial class SecureSuccess : ContentPage
{
	public SecureSuccess()
	{
		InitializeComponent();
	}

    private async void GotoHome(object sender, EventArgs e)
    {

		await Shell.Current.GoToAsync($"//{nameof(Home)}");
	}
}