using Bosco.Core.Data;
using Bosco.Core.Services;
using Bosco.Core.ViewModels;
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
			.AddSingleton<CategoriesDb>()
			.AddSingleton<CategoryViewModel>()
			.AddSingleton<IView, ProductView>()
			.AddSingleton<IView, CategoryView>()
			.AddSingleton<MainWindowNavigation>();
	}

    protected override async void OnStartup(StartupEventArgs e)
    {
		await AppHost!.StartAsync();

		MainWindowNavigation navigation = AppHost.Services.GetRequiredService<MainWindowNavigation>();
		MainWindow MainWindow = new() { DataContext = navigation };
		MainWindow.Show();

        base.OnStartup(e);
    }

	protected override async void OnExit(ExitEventArgs e)
	{
		await AppHost!.StopAsync();

		base.OnExit(e);
	}
}
