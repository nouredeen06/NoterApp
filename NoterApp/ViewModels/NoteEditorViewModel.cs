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

    public ObservableCollection<Tag> NoteTagsList { get; }
    public IReadOnlyDictionary<string, string> colorsLookup { get; }
    public List<Tag> tags { get; }

    public NoteEditorViewModel(NoteManifest manifest, string content)
    {
        _manifest = manifest;
        _title = manifest.Title;
        _body = content;
        _isDirty = false;
        tags = DataService.Instance.GetAllTags();
        NoteTagsList = new ObservableCollection<Tag>(tags);


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
}