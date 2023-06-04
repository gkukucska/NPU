using ClientInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.Pages.RegisterPage
{
    internal partial class PasswordPageViewModel:ObservableObject
    {
        private string _userName;
        private IRegistrationClient _registrationClient;

        public PasswordPageViewModel(string userName, IRegistrationClient registrationClient)
        {
            this._userName = userName;
            this._registrationClient = registrationClient;
        }

        [ObservableProperty]
        private string _password;
        internal bool Register()
        {
            return _registrationClient.RegisterAsync(_userName, _password).Result;
        }
    }
}
