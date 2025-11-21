using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ArqaamTestApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Models;
using NoterApp.Services;

namespace NoterApp.ViewModels;

public partial class AddTagViewModel : ViewModelBase
{
    public ObservableCollection<ColorInfo> AllColors { get; }


    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveTagCommand))]
    public string? _tagName;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveTagCommand))]
    public ColorInfo? _selectedColor;

    public AddTagViewModel()
    {
        AllColors = new ObservableCollection<ColorInfo>();
        foreach (var color in AppColors.DarkHexLookup)
        {
            AllColors.Add(new ColorInfo { name = color.Key, hex = color.Value });
        }

        _selectedColor = null;
    }

    [RelayCommand]
    private void SelectColor(ColorInfo? color)
    {
        if (color == null) return;

        SelectedColor = (SelectedColor == color) ? null : color;
    }

    [RelayCommand(CanExecute = nameof(CanSaveTag))]
    private async Task SaveTag()
    {
        await DataService.Instance.AddTagAsync(TagName, SelectedColor.name);
        WindowManager.Instance.CloseWindow(this);
    }

    public bool CanSaveTag() => !string.IsNullOrEmpty(TagName) && SelectedColor != null;
}