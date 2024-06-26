using System.Xml;

namespace PetsApp.Pages;

public partial class Profile : ContentPage
{
	public Profile()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Lấy giá trị FullName và Address từ Preferences
        string fullName = Preferences.Get("FullName", "");
        string email = Preferences.Get("Email", "");

        // Gán giá trị lấy được vào các Label tương ứng
        NameLabel.Text = fullName;
        EmailLabel.Text = email;
    }

    private async void Alert(object sender, TappedEventArgs e)
    {
        bool result = await DisplayAlert("Logout", "Are you sure you want to log out?", "OK", "Cancel");
        if (result)
        {
            Shell.Current.CurrentItem = new SignIn();
        }
    }


    private async void GotoMyProfile(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyProfile());
    }
}