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
        var client = Handler.MauiContext.Services.GetService<IAuthenticatorClient>();
        BindingContext = new LoginPageViewModel(client);
    }
}