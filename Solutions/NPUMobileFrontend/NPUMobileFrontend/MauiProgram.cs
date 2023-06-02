using ClientInterfaces;
using NPU.Clients.AuthenticatorClient;
using NPU.Clients.RegistrationClient;
using NPU.Clients.ImageDataClient;
using NPU.GUI.LoginPage;
using System.Reflection;
using NPU.GUI.Pages;
using NPU.Interfaces;

namespace NPU.MobileFrontend;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<HomePageViewModel>();
        builder.Services.AddSingleton<IAuthenticatorClient, AuthenticatorClient>();
        builder.Services.AddSingleton<IRegistrationClient, RegistrationClient>();
        builder.Services.AddSingleton<IImageDataClient, ImageDataClient>();
        builder.Services.AddSingleton<IAuthenticatorProvider, LoginPage>();



        return builder.Build();
    }
}
