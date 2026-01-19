using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using NoterApp.Services;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NoterApp.ViewModels;
using NoterApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace NoterApp;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; }
    public string colorHex = "#E22727";
    public static Window MainWindow { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            // var services = new ServiceCollection();
            // // services.AddSingleton<IWindowManager, WindowManager>();
            //
            //
            // //  services.AddTransient<DialogViewModel>();
            // services.AddTransient<DashboardViewModel>();
            // services.AddTransient<SettingsViewModel>();
            // services.AddTransient<TagEditorViewModel>();
            //
            // services.AddSingleton<MainWindowViewModel>();
            //
            // Services = services.BuildServiceProvider();
            //
            // var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            MainWindow = desktop.MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}