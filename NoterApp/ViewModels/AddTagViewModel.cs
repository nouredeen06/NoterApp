using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Models;

namespace NoterApp.ViewModels;

public partial class AddTagViewModel : ViewModelBase
{
    public ObservableCollection<ColorInfo> AllColors { get; }

    [ObservableProperty] private ColorInfo? _selectedColor;

    public AddTagViewModel()
    {
        AllColors = new ObservableCollection<ColorInfo>();
        foreach (var color in AppColors.DarkHexLookup)
        {
            AllColors.Add(new ColorInfo { name = color.Key, hex = color.Value });
            Console.WriteLine("added " + color.Key);
        }

        Console.WriteLine("added all colors");
        _selectedColor = null;
    }

    [RelayCommand]
    private void SelectColor(ColorInfo? color)
    {
        if (color == null) return;

        SelectedColor = (SelectedColor == color) ? null : color;
    }
}