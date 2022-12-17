using Bosco.Core.Services;
using Bosco.XAML.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace Bosco;

public partial class App : Application
{
	public static IHost? AppHost { get; private set; }
	public App()
	{
		AppHost = Host.CreateDefaultBuilder()
			.ConfigureServices(ServiceConfiguration)
			.Build();
	}
	private void ServiceConfiguration(HostBuilderContext context, IServiceCollection services)
	{
		services
			.AddSingleton<MainWindowNavigation>()
			.AddSingleton<IFrontendNotifier, FrontendNotifier>()
			.AddSingleton<IView,ProductView>();
	}

    protected override async void OnStartup(StartupEventArgs e)
    {
		await AppHost!.StartAsync();

		MainWindowNavigation navigation = AppHost.Services.GetRequiredService<MainWindowNavigation>();
		MainWindow MainWindow = new() { DataContext = navigation };
		MainWindow.Show();

        base.OnStartup(e);
    }
}
