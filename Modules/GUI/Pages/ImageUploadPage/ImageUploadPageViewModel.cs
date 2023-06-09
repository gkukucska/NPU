﻿using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NPU.Interfaces;
using NPU.Utils.GUIConstants;
using NPU.Utils.GUIUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.Pages.ImageUploadPage
{
    public partial class ImageUploadPageViewModel : ObservableObject
    {
        private IImageDataClient _imageDataClient;
        private IAuthenticatorProvider _authenticatorProvider;

        public ImageUploadPageViewModel(IImageDataClient imageDataClient, IAuthenticatorProvider authenticatorProvider)
        {
            this._authenticatorProvider = authenticatorProvider;
            this._imageDataClient = imageDataClient;
        }

        [ObservableProperty]
        private ImageSource _imageSource;

        private byte[] _imageData;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private DateTime _lastUpdateTime;

        [RelayCommand]
        public async void TakePhoto()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        // save the file into local storage
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream sourceStream = await photo.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localFilePath);

                        await sourceStream.CopyToAsync(localFileStream);
                        var memorystream = new MemoryStream();
                        localFileStream.CopyTo(memorystream);
                        _imageData = memorystream.ToArray();
                        ImageSource = ImageSource.FromStream(() => memorystream);
                    }
                }
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
        }

        [RelayCommand]
        public async Task<FileResult> PickAndShow()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(PickOptions.Images);
                if (result != null)
                {
                    var memorystream = new MemoryStream();
                    result.OpenReadAsync().Result.CopyTo(memorystream);
                    _imageData = memorystream.ToArray();
                    ImageSource = ImageSource.FromFile(result.FullPath);
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }

        [RelayCommand]
        public async Task SaveImage()
        {
            try
            {
                await _imageDataClient.SaveImageDataAsync(_authenticatorProvider.UserName,
                                              _authenticatorProvider.SessionToken,
                                              _imageData,
                                              Description ??string.Empty
                                               );
                Shell.Current.GoToAsync(GUIConstants.HOMEPAGEROUTE);
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
            return;
        }

        [RelayCommand]
        public void CancelImageSave()
        {
            try
            {
                this.ImageSource = null;
                Description = null;
                Shell.Current.GoToAsync(GUIConstants.HOMEPAGEROUTE);
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
        }
    }
}
