using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using NoterApp.Models;
using NoterApp.ViewModels;

namespace NoterApp.Views;

public partial class TagEditorView : Window
{
    public TagEditorView()
    {
        InitializeComponent();

        btnApply.Click += (_, _) =>
        {
            if (DataContext is TagEditorViewModel viewModel)
            {
                Close(viewModel.SelectedTags);
            }
        };
        btnCancel.Click += (_, _) =>
        {
            if (DataContext is TagEditorViewModel viewModel)
            {
                Close();
            }
        };
    }
}