﻿using ClientInterfaces;
using NPU.Clients.AuthenticatorClient;
using NPU.Clients.RegistrationClient;
using System.Reflection;

namespace NPU.MobileFrontend;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
    {
        var builder =MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<IAuthenticatorClient, AuthenticatorClient>();
        builder.Services.AddSingleton<IRegistrationClient, RegistrationClient>();



        return builder.Build();
	}
}
