using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NPU.GUI.LoginPage
{
    // All the code in this file is included in all platforms.
    public partial class LoginPageViewModel:ObservableObject
    {
        private static IAuthenticatorClient _client;

        public LoginPageViewModel(IAuthenticatorClient client)
        {
            _client = client;
        }

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [RelayCommand]
        private async void Login(object parameters)
        {
            var data = (List<string>)parameters;
            await _client.OpenSessionAsync(data[0], data[1]);
        }

        [RelayCommand]
        private async void Register(object parameters)
        {
            var data = (List<string>)parameters;
            await _client.OpenSessionAsync(data[0], data[1]);
        }
    }
}