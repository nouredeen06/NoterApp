using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Models;
using NoterApp.Views;

namespace NoterApp.ViewModels;


public partial class MainWindowViewModel : ViewModelBase
{
   [ObservableProperty]
   private object _currentViewModel;
   
   public readonly DashboardViewModel _dashboardViewModel;
   public readonly SettingsViewModel _settingsViewModel;
   public readonly NoteEditorViewModel _noteEditorViewModel;

   

   public MainWindowViewModel()
   {
      _dashboardViewModel  = new DashboardViewModel();
      _settingsViewModel = new SettingsViewModel();
      _noteEditorViewModel = new NoteEditorViewModel();
      _currentViewModel = _noteEditorViewModel;
   }

   [RelayCommand]
   private void NavigateToDashboard()
   {
      CurrentViewModel = _dashboardViewModel;
   }
   [RelayCommand]
   private void NavigateToSettings()
   {
      CurrentViewModel = _settingsViewModel;
   }
}