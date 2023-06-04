using NPU.Utils.GUIConstants;

namespace NPU.Pages.RegisterPage;

public partial class SuccessPage : ContentPage
{
	public SuccessPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(GUIConstants.LOGINPAGEROUTE);
    }
}