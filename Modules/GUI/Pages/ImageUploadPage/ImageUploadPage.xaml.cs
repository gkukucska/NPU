using ClientInterfaces;
using NPU.Interfaces;

namespace NPU.Pages;

public partial class ImageUploadPage : ContentPage
{
	public ImageUploadPage()
    {
        HandlerChanged += HomePage_HandlerChanged;

        InitializeComponent();
    }

    private void HomePage_HandlerChanged(object sender, EventArgs e)
    {
        var authenticatorClient = Handler.MauiContext.Services.GetService<IAuthenticatorClient>();
        var imageDataClient = Handler.MauiContext.Services.GetService<IImageDataClient>();
        var authenticatorProvider = Handler.MauiContext.Services.GetService<IAuthenticatorProvider>();
        BindingContext = new ImageUploadPageViewModel(authenticatorClient,imageDataClient,authenticatorProvider);
    }
}