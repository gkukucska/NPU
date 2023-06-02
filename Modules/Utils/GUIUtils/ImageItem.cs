using CommunityToolkit.Mvvm.ComponentModel;

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
        private bool _isLiked;
        [ObservableProperty]
        private DateTime _createdAt;
    }
}
