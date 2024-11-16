using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using ClientApp.Contracts;
using ClientApp.Services;
using ClientApp.ViewModels;
using ClientApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;
        public override void Initialize()
        {
            var services = new ServiceCollection();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<INavigationService, NavigationService>();
            ServiceProvider = services.BuildServiceProvider();

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}