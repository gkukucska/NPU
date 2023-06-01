namespace NPU.MobileFrontend;

public partial class MainPage : Shell
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}