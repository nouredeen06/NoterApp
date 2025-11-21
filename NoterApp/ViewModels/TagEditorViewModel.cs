using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArqaamTestApp.Services;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Models;
using NoterApp.Services;

namespace NoterApp.ViewModels;

public partial class TagEditorViewModel : ViewModelBase
{
    public ObservableCollection<FullTag> TagsList { get; private set; }
    public List<string?> SelectedTags { get; set; }

    public TagEditorViewModel()
    {
    }

    public void OnLoaded()
    {
        var colorsLookups = AppColors.DarkHexLookup;
        var tags = DataService.Instance.GetAllTags();
        TagsList = new ObservableCollection<FullTag>();
        foreach (var tag in tags)
        {
            bool selected = SelectedTags.Contains(tag.Name);
            TagsList.Add(new FullTag { Selected = selected, Name = tag.Name, ColorHex = colorsLookups[tag.ColorName] });
        }
    }


    [RelayCommand]
    private void ToggleTagSelection(FullTag tag)
    {
        if (tag == null) return;

        bool isSelected = SelectedTags.Contains(tag.Name);

        if (isSelected)
        {
            SelectedTags.Remove(tag.Name);
        }
        else
        {
            SelectedTags.Add(tag.Name);
        }
    }

    [RelayCommand]
    private async Task CreateTag()
    {
        await WindowManager.Instance.ShowDialog<AddTagViewModel, bool>(new AddTagViewModel());
        var colorsLookups = AppColors.DarkHexLookup;
        var tags = DataService.Instance.GetAllTags();
        TagsList.Clear();
        foreach (var tag in tags)
        {
            bool selected = SelectedTags.Contains(tag.Name);
            TagsList.Add(new FullTag { Selected = selected, Name = tag.Name, ColorHex = colorsLookups[tag.ColorName] });
        }
    }
}