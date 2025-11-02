using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Extensions;
using NoterApp.Models;
using NoterApp.Services;

namespace NoterApp.ViewModels;

public partial class NoteEditorViewModel : ViewModelBase
{
    public readonly NoteManifest _manifest;

    [ObservableProperty] 
    private string _title;

    [ObservableProperty] private string _body;

    [ObservableProperty] private bool _isDirty;
    

    public NoteEditorViewModel(NoteManifest manifest, string content)
    {
        _manifest = manifest;
        _title = manifest.Title;
        _body = content;
        _isDirty = false;

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

        await NoteService.Instance.SaveNoteAsync(_manifest, Body);

        IsDirty = false;
    }
}