using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
using NPU.Utils.GUIConstants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.GUI.Pages
{
    public partial class HomePageViewModel : ObservableObject
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
            var sessiontoken = await SecureStorage.GetAsync(GUIConstants.SESSIONTOKEN);
            await _authenticatorClient.CloseSessionAsync(username, sessiontoken);
            await SecureStorage.SetAsync(GUIConstants.SESSIONTOKEN, "Invalid");
        }
    }
}
