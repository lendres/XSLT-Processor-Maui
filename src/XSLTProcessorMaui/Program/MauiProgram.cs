using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using DigitalProduction.Maui.UI;

namespace XSLTProcessorMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("IBMPlexMono-Bold.ttf", "IBMPlexMono-Bold");
				fonts.AddFont("IBMPlexMono-Regular.ttf", "IBMPlexMono-Regular");
			});

		LifecycleOptions lifecycleOptions = new()
		{
			EnsureOnScreen			= false,
			DisableMaximizeButton	= true,
			WindowTitle				= "XSLT Processor"
		};
		DigitalProduction.Maui.UI.LifecycleEventsInstaller.ConfigureLifecycleEvents(builder, lifecycleOptions);

		builder.Services.AddSingleton<ICommandLine, CommandLine>();
		builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

		#if DEBUG
			builder.Logging.AddDebug();
		#endif

		return builder.Build();
	}
}