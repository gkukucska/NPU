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

        public ImageItem(byte[] imageSource, string description, string imageID)
        {
            Description = description;
            ImageID = imageID;
            var stream= new MemoryStream(imageSource);
            stream.Position = 0;
            var filename = Path.Combine(FileSystem.CacheDirectory, imageSource.GetHashCode() + ".dat") ;
            var filestream = new FileStream(filename, FileMode.CreateNew); 
            stream.CopyTo(filestream);
            filestream.Flush();
            ImageSource = ImageSource.FromFile(filename);
        }
    }
}
