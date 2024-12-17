using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Chat.Client;
using Chat.ClientApp.Models;
using Chat.ClientApp.Services;
using Chat.ClientApp.Services.Contracts;
using ClientApp.ViewModels;
using ClientApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClientApp
{
    /// <summary>
    /// Класс App является основным классом приложения и реализует интерфейс Application.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Свойство ServiceProvider предоставляет доступ к сервисам приложения.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; } = null!;
        private Window _window = null!; 

        /// <summary>
        /// Метод Initialize инициализирует сервисы приложения.
        /// </summary>
        public override void Initialize()
        {
            var services = new ServiceCollection();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IServerService, ServerService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddScoped<IChatClient, ChatClient>();
            services.AddScoped<IMessageFormatter, MessageFormatter>();
            services.AddSingleton(
                new User(string.Empty, string.Empty)
                );

            _window = new MainWindow();
            services.AddSingleton(_window.StorageProvider);

            ServiceProvider = services.BuildServiceProvider();

            _window.DataContext = new MainWindowViewModel();

            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Метод OnFrameworkInitializationCompleted выполняется после завершения инициализации фреймворка.
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = _window;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}