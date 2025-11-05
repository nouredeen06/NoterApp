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
    public ObservableCollection<FullTag> TagsList { get; }

    public ObservableCollection<NoteIndexItem> _allNotes { get; set; }

    public MainWindowViewModel? Shell { get; set; }

    public DashboardViewModel()
    {
        var colorsLookups = AppColors.DarkHexLookup;
        var tags = DataService.Instance.GetAllTags();
        TagsList = new ObservableCollection<FullTag>();
        foreach (var tag in tags)
        {
            TagsList.Add(new FullTag { Name = tag.Name, ColorHex = colorsLookups[tag.ColorName] });
        }

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

    public void ListNotes()
    {
        var notesFromIndex = DataService.Instance.GetAllNotesFromIndex();
        _allNotes = new ObservableCollection<NoteIndexItem>(notesFromIndex);
        _allNotes.Sort(NoteIndexItem => NoteIndexItem.DateModified, true);
    }
}