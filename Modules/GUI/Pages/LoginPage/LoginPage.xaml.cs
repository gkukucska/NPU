using ClientInterfaces;
using NPU.Interfaces;
using NPU.Utils.GUIConstants;

namespace NPU.GUI.LoginPage;

public partial class LoginPage : ContentPage, IAuthenticatorProvider
{

    private LoginPageViewModel _viewModel;
    public LoginPage()
    {
        HandlerChanged += LoginPage_HandlerChanged;

        InitializeComponent();
    }

    public string UserName => LoginPageViewModel.Username;

    public string SessionToken => LoginPageViewModel.Sessiontoken;

    private void LoginPage_HandlerChanged(object sender, EventArgs e)
    {
        var authetnticatorClient = Handler.MauiContext.Services.GetService<IAuthenticatorClient>();
        var registrationClient = Handler.MauiContext.Services.GetService<IRegistrationClient>();
        BindingContext = new LoginPageViewModel(authetnticatorClient, registrationClient);
    }
}