using System;

namespace NoterApp.Models;

public class NotePreview
{
    public string? title { get; set; }
    public string? body { get; set; }
    public string? tag { get; set; }
    public string? tagColor { get; set; }
    public DateTime? datePinned { get; set; }
    public DateTime dateCreated { get; set; }
    public DateTime dateModified { get; set; }
}