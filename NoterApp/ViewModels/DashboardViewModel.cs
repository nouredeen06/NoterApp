using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Extensions;
using NoterApp.Models;
using NoterApp.Services;

namespace NoterApp.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    public ObservableCollection<Tags> TagsList { get; }

    public ObservableCollection<NoteIndexItem> _allNotes { get; set; }


    public DashboardViewModel()
    {
        TagsList = new ObservableCollection<Tags>
        {
            new() { Name = "Tag 1", ColorHex = "#FF0000" },
            new() { Name = "Tagaaaaaaaaaa 2", ColorHex = "#CF7010" },
            new() { Name = "TagTag Tag 3", ColorHex = "#196082" }
        };
        ListNotes();
        
    }

    public void ListNotes()
    {
        var notesFromIndex = NoteService.Instance.GetAllNotesFromIndex();
        _allNotes = new ObservableCollection<NoteIndexItem>(notesFromIndex);
        _allNotes.Sort(NoteIndexItem => NoteIndexItem.DateModified, true);
    }
}