using ClientInterfaces;
using NPU.Interfaces;

namespace NPU.GUI.MyImagesPage;

public partial class MyImagesPage : ContentPage
{

    private static MyImagesPageViewModel _viewModel;
	public MyImagesPage()
    {
        HandlerChanged += ImageUploadPage_HandlerChanged;
        NavigatedTo += ImageUploadPage_NavigatedTo;
        InitializeComponent();
    }

    private void ImageUploadPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        _viewModel?.LoadNextImages();
    }

    private void ImageUploadPage_HandlerChanged(object sender, EventArgs e)
    {
        var imageDataClient = Handler.MauiContext.Services.GetService<IImageDataClient>();
        var authenticatorProvider = Handler.MauiContext.Services.GetService<IAuthenticatorProvider>();
        BindingContext = new MyImagesPageViewModel(imageDataClient, authenticatorProvider);
        _viewModel=(MyImagesPageViewModel)BindingContext;
    }

    private void ListView_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
    {
    }

    private void ListView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if ((sender as ListView).Height *0.7 <e.ScrollY)
        {
            Task.Run(() =>
            {
                _viewModel.LoadNextImage();
            });

        }
    }
}