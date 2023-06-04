using ClientInterfaces;
using NPU.Interfaces;
using System.Threading.Tasks;
using NPU.Utils.GUIConstants;
using System.Reflection.Metadata;

namespace NPU.GUI.LoginPage;

public partial class LoginPage : ContentPage, IAuthenticatorProvider
{

    private static LoginPageViewModel _viewModel;
    public LoginPage()
    {
        HandlerChanged += LoginPage_HandlerChanged;

        InitializeComponent();
    }

    public string UserName => LoginPageViewModel.StaticUsername;

    public string SessionToken => LoginPageViewModel.StaticSessionToken;

    public void ForceLogout()
    {
        Dispatcher.Dispatch(() => _viewModel?.CloseSession());
    }

    private void LoginPage_HandlerChanged(object sender, EventArgs e)
    {
        var authetnticatorClient = Handler.MauiContext.Services.GetService<IAuthenticatorClient>();
        var registrationClient = Handler.MauiContext.Services.GetService<IRegistrationClient>();
        BindingContext = new LoginPageViewModel(authetnticatorClient, registrationClient);
        _viewModel = BindingContext as LoginPageViewModel;
    }
}