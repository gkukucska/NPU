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
        [RelayCommand]
        private async void Logout()
        {

            await Shell.Current.GoToAsync(GUIConstants.LOGINPAGEROUTE);
            await SecureStorage.SetAsync(GUIConstants.SESSIONTOKEN, "Invalid");
        }
    }
}
