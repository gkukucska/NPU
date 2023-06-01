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
        BindingContext = new HomePageViewModel();
    }

    private void ListView_Scrolled(object sender, ScrolledEventArgs e)
    {

    }
}