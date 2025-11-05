using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Services;

namespace NoterApp.ViewModels;

public partial class NotesWorkspaceViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<NoteEditorViewModel> _openNotes;

    [ObservableProperty] private Action<NoteEditorViewModel?> _setActiveNote;

    public NotesWorkspaceViewModel(ObservableCollection<NoteEditorViewModel> openNotes,
        Action<NoteEditorViewModel?> setActiveNote)
    {
        _openNotes = openNotes;
        _setActiveNote = setActiveNote;
    }


    public async Task OpenNoteByID(Guid noteID)
    {
        var existingNote = OpenNotes.FirstOrDefault(n => n._manifest.ID == noteID);
        if (existingNote != null)
        {
            _setActiveNote(existingNote);
            return;
        }

        var (manifest, content) = await DataService.Instance.OpenNoteAsync(noteID);
        var newNoteViewModel = new NoteEditorViewModel(manifest, content);

        _openNotes.Add(newNoteViewModel);
        _setActiveNote(newNoteViewModel);
    }

    [RelayCommand]
    private void CreateNewNote()
    {
        var manifest = DataService.Instance.CreateNewNote();

        // 2. Create the ViewModel for this new, virtual note.
        var newNoteViewModel = new NoteEditorViewModel(manifest, "");

        // 3. Add the new tab to the list of open notes.
        _openNotes.Add(newNoteViewModel);

        // 4. Set it as the active note.
        _setActiveNote(newNoteViewModel);
    }
}