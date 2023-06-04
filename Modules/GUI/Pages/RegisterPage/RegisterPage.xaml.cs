namespace NPU.Pages.RegisterPage;

public partial class RegisterPage : NavigationPage
{
	public RegisterPage()
	{
		Navigation.PushAsync(new UserNamePage()).Wait();
		InitializeComponent();
	}
}