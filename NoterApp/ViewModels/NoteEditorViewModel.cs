using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ArqaamTestApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NoterApp.Extensions;
using NoterApp.Models;
using NoterApp.Services;

namespace NoterApp.ViewModels;

public partial class NoteEditorViewModel : ViewModelBase
{
    public readonly NoteManifest _manifest;

    [ObservableProperty] private string _title;

    [ObservableProperty] private string _body;

    [ObservableProperty] private bool _isDirty;

    private readonly Func<TagEditorViewModel, Task<List<string>>> openTagsEditor;
    public ObservableCollection<FullTag> NoteTagsList { get; }
    public IReadOnlyDictionary<string, string> colorsLookup { get; }
    public Dictionary<string, string> tagsLookup { get; }

    public NoteEditorViewModel(NoteManifest manifest, string content)
    {
        _manifest = manifest;
        _title = manifest.Title;
        _body = content;
        _isDirty = false;
        colorsLookup = AppColors.DarkHexLookup;
        tagsLookup = DataService.Instance.GetAllTags().ToDictionary(Tag => Tag.Name, Tag => Tag.ColorName);
        NoteTagsList = new ObservableCollection<FullTag>();
        foreach (var tag in _manifest.Tags)
        {
            NoteTagsList.Add(new FullTag { Name = tag, ColorHex = colorsLookup[tagsLookup[tag]] });
        }


        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName is nameof(Title) or nameof(Body))
            {
                IsDirty = true;
            }
        };
    }

    [RelayCommand]
    private async Task Save()
    {
        if (!IsDirty) return;

        _manifest.Title = Title;

        await DataService.Instance.SaveNoteAsync(_manifest, Body);

        IsDirty = false;
    }

    [RelayCommand]
    private async Task EditNoteTags()
    {
        var tagEditor = new TagEditorViewModel();
        tagEditor.SelectedTags = new List<string>(_manifest.Tags);
        tagEditor.OnLoaded();

        var list = await WindowManager.Instance.ShowDialog<TagEditorViewModel, List<string>>(tagEditor);
        
        if (list == null) return;
        
        _manifest.Tags = list;
        NoteTagsList.Clear();
        IsDirty = true;
        foreach (var tag in _manifest.Tags)
        {
            NoteTagsList.Add(new FullTag { Name = tag, ColorHex = colorsLookup[tagsLookup[tag]] });
        }
    }
}