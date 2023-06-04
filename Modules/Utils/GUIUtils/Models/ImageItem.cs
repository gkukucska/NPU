using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using SkiaSharp;

namespace NPU.Utils.GUIUtils
{
    public partial class ImageItem : ObservableObject
    {
        [ObservableProperty]
        private ImageSource _imageSource;
        [ObservableProperty]
        private string _description;
        [ObservableProperty]
        private string _createdby;
        [ObservableProperty]
        private string _imageID;
        [ObservableProperty]
        private bool _isLiked;

        byte[] _imageData;

        public ImageItem(byte[] imageData, string description, string imageID)
        {
            Description = description;
            ImageID = imageID;
            _imageData = imageData;
            ImageSource = ImageSource.FromStream(() => new MemoryStream(_imageData));
        }
    }
}
