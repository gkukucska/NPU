using NPU.GUI.LoginPage;
using NPU.GUI.Pages;
using NPU.Pages.RegisterPage;
using NPU.Utils.GUIConstants;

namespace NPU.MobileFrontend;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(GUIConstants.LOGINPAGE, typeof(LoginPage));
        Routing.RegisterRoute(GUIConstants.MAINPAGE, typeof(MainPage));
        Routing.RegisterRoute(GUIConstants.HOMEPAGE, typeof(HomePage));
        Routing.RegisterRoute(GUIConstants.REGISTERPAGE, typeof(RegisterPage));
        //Routing.RegisterRoute("settings", typeof(SettingsPage));
    }
}
