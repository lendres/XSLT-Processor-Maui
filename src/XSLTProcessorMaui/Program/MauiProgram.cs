using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;

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
		#if WINDOWS
			builder.ConfigureLifecycleEvents(lifecycle =>  
			{
				lifecycle.AddWindows((builder) =>  
				{  
					builder.OnWindowCreated(window =>  
					{  
						window.Title = "XSLT Processor";

						nint handle			= WinRT.Interop.WindowNative.GetWindowHandle(window);
						WindowId id			= Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
						AppWindow appWindow	= Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
						switch (appWindow.Presenter)
						{
							case OverlappedPresenter overlappedPresenter:
								overlappedPresenter.IsMaximizable = false;
								break;
						}
					});  
				});  
			});
		#endif

		builder.Services.AddSingleton<ICommandLine, CommandLine>();
		builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

		#if DEBUG
			builder.Logging.AddDebug();
		#endif

		return builder.Build();
	}
}
