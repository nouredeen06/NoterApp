using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace NoterApp.Services;

public class WindowManager
{
    private static readonly Lazy<WindowManager> _instance = new(() => new WindowManager());
    public static WindowManager Instance => _instance.Value;

    private readonly Dictionary<object, Window> _openWindows = new();

    public async Task ShowWindow<TViewModel>(TViewModel viewModel) where TViewModel : class
    {
        var window = CreateWindow(viewModel);
        window.Show();
        await Task.CompletedTask;
    }

    public async Task<TResult> ShowDialog<TViewModel, TResult>(TViewModel viewModel)
        where TViewModel : class
    {
        var window = CreateWindow(viewModel);

        var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow;

        return await window.ShowDialog<TResult>(mainWindow);
    }

    public void CloseWindow(object viewModel)
    {
        if (_openWindows.TryGetValue(viewModel, out var window))
        {
            window.Close();
        }
    }


    private Window CreateWindow<TViewModel>(TViewModel viewModel) where TViewModel : class
    {
        var viewModelType = viewModel.GetType();
        var viewTypeName = viewModelType.FullName.Replace("ViewModels", "Views");
        viewTypeName = viewTypeName.Replace("ViewModel", "View");
        var viewType = Type.GetType(viewTypeName);

        if (viewType == null)
        {
            throw new ArgumentException(
                $"Could not find a view for view model type '{viewModelType.FullName}'. Excepted type '{viewTypeName}'");
        }

        var view = (Window)Activator.CreateInstance(viewType);
        view.DataContext = viewModel;
        _openWindows[viewModel] = view;
        view.Closed += (s, e) => { _openWindows.Remove(viewModel); };
        return view;
    }
}