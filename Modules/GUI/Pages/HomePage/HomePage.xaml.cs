using ClientInterfaces;

namespace NPU.GUI.Pages;

public partial class HomePage : TabbedPage
{
    public HomePage()
    {
        HandlerChanged += HomePage_HandlerChanged;

        InitializeComponent();
    }

    private void HomePage_HandlerChanged(object sender, EventArgs e)
    {
        var authenticatorClient = Handler.MauiContext.Services.GetService<IAuthenticatorClient>();
        BindingContext = new HomePageViewModel(authenticatorClient);
    }
}