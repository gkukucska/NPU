﻿using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
using NPU.Utils.GUIConstants;

namespace NPU.GUI.LoginPage
{
    // All the code in this file is included in all platforms.
    public partial class LoginPageViewModel : ObservableObject
    {
        private IAuthenticatorClient _aurthenticatorClient;
        private IRegistrationClient _registrationClient;

        public LoginPageViewModel(IAuthenticatorClient authenticatorClient, IRegistrationClient registrationClient)
        {
            _aurthenticatorClient = authenticatorClient;
            _registrationClient = registrationClient;
        }

        [ObservableProperty]
        private string _status;

        [ObservableProperty]
        private static string _userName;
        private static string _staticusername;
        public static string StaticUsername => _staticusername;

        [ObservableProperty]
        private static string _sessionToken;
        public static string StaticSessionToken => _sessionToken;

        [ObservableProperty]
        private string _password;

        [RelayCommand]
        private async void Login(object parameters)
        {
            Status=string.Empty;
            try
            {
                var data = (List<string>)parameters;
                var token = await _aurthenticatorClient.OpenSessionAsync(UserName, Password);
                if (!string.IsNullOrEmpty(token))
                {
                    _sessionToken = token;
                    _staticusername = UserName;
                    await Shell.Current.GoToAsync(GUIConstants.HOMEPAGEROUTE);
                    return;
                }
                Status = "Failed to log in";
            }
            catch (Exception)
            {
                Status = "Failed to log in";
            }
        }

        [RelayCommand]
        private async void Register(object parameters)
        {
            await Shell.Current.GoToAsync(GUIConstants.REGISTERPAGEROUTE);
        }

        public async void CloseSession()
        {
            try
            {
                _aurthenticatorClient.CloseSession(UserName, SessionToken);
            }
            catch (Exception e)
            {
            }
            Status = "Session timed out, logged out";
            await Shell.Current.GoToAsync(GUIConstants.LOGINPAGEROUTE);
        }
    }
}