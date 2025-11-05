using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NoterApp.ViewModels;

namespace NoterApp.Views;

public partial class DialogView : Window
{
    public DialogView()
    {
        InitializeComponent();

        btnSave.Click += (_, _) =>
        {
            if (DataContext is DialogViewModel viewModel)
            {
                Close("save");
            }
        };
        btnDiscard.Click += (_, _) =>
        {
            if (DataContext is DialogViewModel viewModel)
            {
                Close("discard");
            }
        };

        btnCancel.Click += (_, _) =>
        {
            if (DataContext is DialogViewModel viewModel)
            {
                Close("cancel");
            }
        };
    }
}