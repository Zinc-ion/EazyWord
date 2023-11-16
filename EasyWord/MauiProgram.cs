﻿using Microsoft.Extensions.Logging;
using EasyWord.Data;
using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using EasyWord.Services;

namespace EasyWord;

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
			});

		builder.Services.AddMauiBlazorWebView();
        // CodeComment
        builder.Services.AddBootstrapBlazor();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddScoped<IWordStorage, WordStorage>();
        builder.Services.AddScoped<IPreferenceStorage, PreferenceStorage>();

        return builder.Build();
	}
}