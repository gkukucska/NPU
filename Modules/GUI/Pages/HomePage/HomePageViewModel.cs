using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
using NPU.Utils.GUIConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.GUI.Pages
{
    public partial class HomePageViewModel:ObservableObject
    {
        private IAuthenticatorClient _authenticatorClient;

        public HomePageViewModel(IAuthenticatorClient authenticatorClient)
        {
            _authenticatorClient = authenticatorClient;
        }

        [RelayCommand]
        private async void Logout()
        {
            await Shell.Current.GoToAsync(GUIConstants.LOGINPAGEROUTE);
            var username = await SecureStorage.GetAsync(GUIConstants.USERNAME);
            var sessiontoken=await SecureStorage.GetAsync(GUIConstants.SESSIONTOKEN);
            await _authenticatorClient.CloseSessionAsync(username,sessiontoken);
            await SecureStorage.SetAsync(GUIConstants.SESSIONTOKEN, "Invalid");
        }

        [RelayCommand]
        public async void TakePhoto()
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
                }
            }
        }
    }
}
