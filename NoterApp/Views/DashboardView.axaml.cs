using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NoterApp.ViewModels;

namespace NoterApp.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var mainWindow = this.VisualRoot;

        if (mainWindow is MainWindow view)
        {
            view.BtnNewNote_OnClick(sender, e);
        }
    }
}