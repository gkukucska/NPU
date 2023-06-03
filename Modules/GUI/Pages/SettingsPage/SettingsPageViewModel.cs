using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NPU.Utils.GUIConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.Pages.SettingsPage
{
    internal partial class SettingsPageViewModel: ObservableObject
    {
        private IAuthenticatorClient _authenticatorClient;

        public SettingsPageViewModel(IAuthenticatorClient authenticatorClient)
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
