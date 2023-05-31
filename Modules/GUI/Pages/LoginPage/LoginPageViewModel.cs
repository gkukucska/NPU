using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NPU.GUI.LoginPage
{
    // All the code in this file is included in all platforms.
    public partial class LoginPageViewModel:ObservableObject
    {
        private IAuthenticatorClient _aurthenticatorClient;
        private IRegistrationClient _registrationClient;

        public LoginPageViewModel(IAuthenticatorClient authenticatorClient,IRegistrationClient registrationClient)
        {
            _aurthenticatorClient = authenticatorClient;
            _registrationClient = registrationClient;
        }

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [RelayCommand]
        private async void Login(object parameters)
        {
            var data = (List<string>)parameters;
            await _aurthenticatorClient.OpenSessionAsync(data[0], data[1]);
        }

        [RelayCommand]
        private async void Register(object parameters)
        {
            var data = (List<string>)parameters;
            await _registrationClient.RegisterAsync(data[0], data[1]);
        }
    }
}