using ClientInterfaces;

namespace NPU.GUI.LoginPage;

public partial class LoginPage : ContentView
{
    public LoginPage()
    {
        HandlerChanged += LoginPage_HandlerChanged;

        InitializeComponent();
    }

    private void LoginPage_HandlerChanged(object sender, EventArgs e)
    {
        var authetnticatorClient = Handler.MauiContext.Services.GetService<IAuthenticatorClient>();
        var registrationClient = Handler.MauiContext.Services.GetService<IRegistrationClient>();
        BindingContext = new LoginPageViewModel(authetnticatorClient, registrationClient);
    }
}