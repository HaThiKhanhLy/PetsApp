using PetsApp.Models;
using PetsApp.Pages.Admin;
using PetsApp.Repository;
namespace PetsApp.Pages;

public partial class SignIn : ContentPage
{
    readonly IUserRepository userServices= new UserServices();
	public SignIn()
	{
        InitializeComponent();


    }

    private void SignUp(object sender, TappedEventArgs e)
    {
        if (!RepasswordGrid.IsVisible)
        {
            RepasswordGrid.IsVisible = true;
            SignInUp.Text = "Sign in";
            btnSignInUp.Text = "Sign up";
        }
        else
        {
            RepasswordGrid.IsVisible = false;
            SignInUp.Text = "Sign up";
            btnSignInUp.Text = "Sign in";
        }
    }

    private async void ForgotPassword(object sender, TappedEventArgs e)
    {
       await Navigation.PushAsync(new ForgotPassword());
    }

    private async void GotoHome(object sender, EventArgs e)
    {
        /*await Shell.Current.GoToAsync($"//{nameof(Home)}");*/
        try
        {
            string email = Entry_Email.Text;
            if (!email.Contains("@"))
            {
                await DisplayAlert("Error", "Email invalid", "OK");
                return;
            };
            string password = Entry_Password.Text;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await Shell.Current.DisplayAlert("Error", "All fields required", "OK");
                return;
            }
            if (btnSignInUp.Text == "Sign up")
            {
                UserModels newUser = await userServices.Register(email, password);
                if (newUser == null)
                {
                    await DisplayAlert("Error", "Failed to register", "OK");
                    return;
                }
                await Shell.Current.GoToAsync($"//{nameof(Home)}");
                await DisplayAlert("Registration", "Successfully registered", "OK");
            }
            else {
                UserModels user = await userServices.Login(email, password);
                if (user == null)
            {
                await DisplayAlert("Error", "Credentials are incorrect", "OK");
                return;
            }
                Preferences.Set("UserID", user.ID);
                Preferences.Set("FullName", user.FullName);
                Preferences.Set("Address", user.Address);
                Preferences.Set("Phone", user.Phone);
                Preferences.Set("Email", user.Email);
                if (user.Role == "Admin")
                {
                    await Navigation.PushAsync(new NewPage1());
                }
                else if (user.Role == "User")
                {
                    await Shell.Current.GoToAsync($"//{nameof(Home)}");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Login", ex.Message, "OK");
        }
    }
}