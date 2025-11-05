using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ArqaamTestApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Models;
using NoterApp.Services;
using NoterApp.Views;

namespace NoterApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private object _currentViewModel;

    public DashboardViewModel _dashboardViewModel { get; }
    public SettingsViewModel _settingsViewModel { get; }
    public TagEditorViewModel _tagEditorViewModel { get; }

    [ObservableProperty] private ObservableCollection<NoteEditorViewModel> _openNotes;
    [ObservableProperty] private NoteEditorViewModel? _activeNote;


    private readonly NotesWorkspaceViewModel _notesWorkspaceLogic;

    public MainWindowViewModel()
    {
        _dashboardViewModel = new DashboardViewModel();
        _tagEditorViewModel = new TagEditorViewModel();
        _dashboardViewModel.Shell = this;

        _openNotes = new ObservableCollection<NoteEditorViewModel>();
        _notesWorkspaceLogic = new NotesWorkspaceViewModel(_openNotes, note => ActiveNote = note);

        _currentViewModel = _dashboardViewModel;
    }

    [RelayCommand]
    private void test()
    {
        WindowManager.Instance.ShowWindow(new AddTagViewModel());
    }

    [RelayCommand]
    private void NavigateToDashboard() => CurrentViewModel = _dashboardViewModel;

    [RelayCommand]
    private void NavigateToSettings() => CurrentViewModel = _settingsViewModel;

    [RelayCommand]
    public async Task GoToNote(Guid noteID)
    {
        await _notesWorkspaceLogic.OpenNoteByID(noteID);

        CurrentViewModel = ActiveNote;
    }

    [RelayCommand]
    private void CreateNewNote()
    {
        _notesWorkspaceLogic.CreateNewNoteCommand.Execute(null);
        CurrentViewModel = ActiveNote;
    }

    [RelayCommand]
    private void SwitchToNote(NoteEditorViewModel noteToActivate)
    {
        if (noteToActivate != null)
        {
            ActiveNote = noteToActivate;
            CurrentViewModel = ActiveNote;
        }
    }

    [RelayCommand]
    private async void CloseNote(NoteEditorViewModel noteToClose)
    {
        if (noteToClose.IsDirty)
        {
            var viewModel = new DialogViewModel();
            viewModel.noteTitle = noteToClose.Title;
            viewModel.OnLoaded();
            string result = await WindowManager.Instance.ShowDialog<DialogViewModel, string>(viewModel);

            if (result == "save")
            {
                await noteToClose.SaveCommand.ExecuteAsync(null);
            }
            else if (result == "cancel")
            {
                return;
            }
        }

        int index = OpenNotes.IndexOf(noteToClose);
        OpenNotes.Remove(noteToClose);

        if (ActiveNote == noteToClose)
        {
            if (OpenNotes.Any())
            {
                ActiveNote = OpenNotes[Math.Max(0, index - 1)];
            }
            else
            {
                ActiveNote = null;
                _dashboardViewModel.ListNotes();
                CurrentViewModel = _dashboardViewModel;
            }
        }
    }

    partial void OnActiveNoteChanged(NoteEditorViewModel? value)
    {
        if (value != null)
        {
            CurrentViewModel = value;
        }
        else if (!OpenNotes.Any())
        {
            _dashboardViewModel.ListNotes();
            CurrentViewModel = _dashboardViewModel;
        }
    }
}