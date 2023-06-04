using ClientInterfaces;

namespace NPU.Pages.RegisterPage;

public partial class UserNamePage : ContentPage
{
    private UserNamePageViewModel _viewModel;
	public UserNamePage()
	{
        HandlerChanged += UserNamePage_HandlerChanged;
        InitializeComponent();
	}

    private void UserNamePage_HandlerChanged(object sender, EventArgs e)
    {
        var registrationClient = Handler.MauiContext.Services.GetService<IRegistrationClient>();
        BindingContext = new UserNamePageViewModel(registrationClient);
        _viewModel = BindingContext as UserNamePageViewModel;
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel?.ValidateUserName();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PasswordPage(_viewModel.UserName));
    }
}