namespace NPU.Pages.ImageUploadPage;

public partial class ImageDescriptionPage : ContentPage
{
	public ImageDescriptionPage(ImageUploadPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}