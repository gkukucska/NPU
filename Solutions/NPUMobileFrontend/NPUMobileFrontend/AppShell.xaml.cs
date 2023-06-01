using NPU.GUI.LoginPage;
using NPU.GUI.Pages;
using NPU.Utils.GUIConstants;

namespace NPU.MobileFrontend;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(GUIConstants.LOGINPAGE, typeof(LoginPage));
        Routing.RegisterRoute(GUIConstants.MAINPAGE, typeof(MainPage));
        Routing.RegisterRoute(GUIConstants.HOMEPAGE, typeof(NPU.GUI.Pages.HomePage));
        //Routing.RegisterRoute("settings", typeof(SettingsPage));
    }
}
