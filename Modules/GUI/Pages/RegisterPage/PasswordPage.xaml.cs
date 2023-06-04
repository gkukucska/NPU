using ClientInterfaces;
using NPU.Utils.GUIConstants;

namespace NPU.Pages.RegisterPage;

public partial class PasswordPage : ContentPage
{
    private string _userName;
    private PasswordPageViewModel _viewModel;
	public PasswordPage(string username)
	{
        _userName = username;
        HandlerChanged += PasswordPage_HandlerChanged;
		InitializeComponent();
	}

    private void PasswordPage_HandlerChanged(object sender, EventArgs e)
    {
        IRegistrationClient registrationClient = Handler.MauiContext.Services.GetService<IRegistrationClient>();
        BindingContext = new PasswordPageViewModel(_userName,registrationClient);
        _viewModel=BindingContext as PasswordPageViewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.Register())
        {
            Navigation.PushAsync(new SuccessPage());
        }
    }
}