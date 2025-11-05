using CommunityToolkit.Mvvm.ComponentModel;

namespace NoterApp.ViewModels;

public partial class DialogViewModel : ViewModelBase
{
    [ObservableProperty] public string noteTitle;
    [ObservableProperty] private string dialogMessage;
    [ObservableProperty] private string dialogOk;
    [ObservableProperty] private string dialogCancel;


    public DialogViewModel()
    {
    }

    public void OnLoaded()
    {
        dialogMessage = $"Do you wish to save '{noteTitle}'?";
    }
}