using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoterApp.Extensions;
using NoterApp.Models;

namespace NoterApp.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    public ObservableCollection<Tags> TagsList { get; }
    public ObservableCollection<NotePreview> NotePreviewList { get; set; }
    public string lorem;

    public DashboardViewModel()
    {
        lorem =
            "Sit minim nisi nulla. Elit nulla tempor esse ex mollit voluptate nisi anim culpa nostrud irure cupidatat nulla. Lorem reprehenderit nulla ea. Adipisicing aute pariatur dolore voluptate et nulla qui commodo nostrud ea. Ad Lorem et id sit commodo minim amet duis.";
        TagsList = new ObservableCollection<Tags>
        {
            new() { Name = "Tag 1", ColorHex = "#FF0000" },
            new() { Name = "Tagaaaaaaaaaa 2", ColorHex = "#CF7010" },
            new() { Name = "TagTag Tag 3", ColorHex = "#196082" }
        };

        NotePreviewList = new ObservableCollection<NotePreview>
        {
            new()
            {
                title = "1st", body = lorem, tag = "Tag 1", tagColor = "#FF0000",
                dateCreated = new DateTime(2025, 10, 24, 12, 0, 0), dateModified = DateTime.Today, datePinned = null
            }
        };
        NotePreviewList.Sort(NotePreview => NotePreview.dateModified, true);
    }

    [RelayCommand]
    public void createnote()
    {
     
     

        NotePreviewList.Add(new()
        {
            title = $"note {NotePreviewList.Count +1 }",
            body = lorem,
            tag = "TagTag Tag 3",
            tagColor = "#196082",
            dateCreated = DateTime.Now,
            dateModified = DateTime.Today,
            datePinned = null
        });

        NotePreviewList.Sort(NotePreview => NotePreview.dateModified, true);
    }
}