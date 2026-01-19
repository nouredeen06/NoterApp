using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Extensions;
using NoterApp.Models;
using NoterApp.Services;

namespace NoterApp.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    public ObservableCollection<Tag> TagsList { get; set; }

    public ObservableCollection<NoteDashItem> _allNotes { get; set; }

    public MainWindowViewModel? Shell { get; set; }

    public DashboardViewModel()
    {
        ListTags();
        ListNotes();
    }

    [RelayCommand]
    private void OpenNote(Guid noteID)
    {
        if (Shell != null && Shell.GoToNoteCommand.CanExecute(noteID))
        {
            Shell.GoToNoteCommand.Execute(noteID);
        }
    }

    public void ListTags()
    {
        var tags = DataService.Instance.GetAllTags();
        TagsList = new ObservableCollection<Tag>(tags);
    }

    public void ListNotes()
    {
        var colorsLookups = AppColors.DarkHexLookup;
        var notesFromIndex = DataService.Instance.GetAllNotesFromIndex();
        var tagsLookup = DataService.Instance.GetAllTags().ToDictionary(Tag => Tag.Name, Tag => Tag.ColorName);
        _allNotes = new ObservableCollection<NoteDashItem>();
        foreach (var note in notesFromIndex)
        {
            _allNotes.Add(new NoteDashItem
            {
                Title = note.Title, BodySnippet = note.BodySnippet, DateModified = note.DateModified,
                DateCreated = note.DateCreated, ID = note.ID, Group = note.Group, Tags = note.Tags
            });
        }

        _allNotes.Sort(NoteIndexItem => NoteIndexItem.DateModified, true);
    }
}

public class NoteDashItem : NoteIndexItem
{
    public Guid ID { get; set; }
    public string Title { get; set; }
    public string BodySnippet { get; set; }
    public DateTime DateModified { get; set; }
    public DateTime DateCreated { get; set; }
    public List<string> Tags { get; set; }
}