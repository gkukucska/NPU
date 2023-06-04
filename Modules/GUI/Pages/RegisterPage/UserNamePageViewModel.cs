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
    internal partial class UserNamePageViewModel:ObservableObject
    {

        private IRegistrationClient _registratorClient;

        public UserNamePageViewModel(IRegistrationClient authetnticatorClient)
        {
            this._registratorClient = authetnticatorClient;
        }

        internal void ValidateUserName()
        {
            CanMoveNext = _registratorClient.ValidateRegistrationData(UserName, string.Empty);
        }

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private bool _canMoveNext;


    }
}
