using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using NPU.Interfaces;
using NPU.Utils.GUIUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.GUI.MyImagesPage
{
    internal partial class MyImagesPageViewModel : ObservableObject
    {
        private IImageDataClient _imageDataClient;
        private IAuthenticatorProvider _authenticatorProvider;
        private static readonly int _initialImageCount=5;
        private object _lock=new object();

        [ObservableProperty]
        private ObservableCollection<ImageItem> _images = new ObservableCollection<ImageItem>();

        public MyImagesPageViewModel(IImageDataClient imageDataClient, IAuthenticatorProvider authenticatorProvider)
        {
            this._imageDataClient = imageDataClient;
            this._authenticatorProvider = authenticatorProvider;
            Images.CollectionChanged += ImagesChanged;
            LoadNextImages(_initialImageCount);
        }

        private void ImagesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Images.Count>10)
            {
                lock (_lock)
                {

                    while (Images.Count > 10)
                    {
                        Images.Remove(Images.First());
                    }
                }
            }
        }

        public async Task LoadNextImages(int? count=null)
        {
            count = count ?? _initialImageCount;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    (byte[] ImageData, string Description, string ImageID) imagedata;
                    if (Images.Count == 0)
                    {
                        imagedata = await _imageDataClient.GetFirstImageAsync(_authenticatorProvider.UserName, _authenticatorProvider.SessionToken);
                    }
                    else
                    {
                        imagedata = await _imageDataClient.GetNextImageDataAsync(_authenticatorProvider.UserName, _authenticatorProvider.SessionToken, Images.Last().ImageID);
                    }
                    if (imagedata.ImageData.Length==0)
                    {
                        return;
                    }
                    lock (_lock)
                    {
                        Images.Add(new ImageItem(imagedata.ImageData, imagedata.Description, imagedata.ImageID));
                    }
                    OnPropertyChanged(nameof(Images));
                }
                catch (Exception e)
                {

                }
            }
        }

        public void LoadNextImage()
            => LoadNextImages(1);
    }
}
