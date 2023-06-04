using ClientInterfaces;
using NPU.Interfaces;

namespace NPU.Pages.ImageUploadPage;

public partial class ImageUploadPage : ContentPage
{
    public ImageUploadPage()
    {
        HandlerChanged += ImageUploadPage_HandlerChanged;

        InitializeComponent();
    }

    private void ImageUploadPage_HandlerChanged(object sender, EventArgs e)
    {
        var imageDataClient = Handler.MauiContext.Services.GetService<IImageDataClient>();
        var authenticatorProvider = Handler.MauiContext.Services.GetService<IAuthenticatorProvider>();
        BindingContext = new ImageUploadPageViewModel(imageDataClient, authenticatorProvider);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ImageDescriptionPage(BindingContext as ImageUploadPageViewModel));
    }
}