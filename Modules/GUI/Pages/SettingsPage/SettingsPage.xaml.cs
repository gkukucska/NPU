using ClientInterfaces;

namespace NPU.Pages.SettingsPage;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
    {
        HandlerChanged += SettingsPage_HandlerChanged;

        InitializeComponent();
    }

    private void SettingsPage_HandlerChanged(object sender, EventArgs e)
    {
        var authenticatorClient = Handler.MauiContext.Services.GetService<IAuthenticatorClient>();
        BindingContext = new SettingsPageViewModel(authenticatorClient);
    }
}