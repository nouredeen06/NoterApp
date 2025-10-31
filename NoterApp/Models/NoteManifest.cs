using System;
using System.Collections.Generic;

namespace NoterApp.Models;

public class NoteManifest
{
    public Guid ID { get; set; }
    public string Title { get; set; }
    public DateTime DateCreated { get; set; }
    public List<string> Tags { get; set; } = new();
    public int CurrentVersion { get; set; }
    public List<NoteVersion> Versions { get; set; } = new();
}

public class NoteVersion
{
    public int Version { get; set; }
    public string FileName { get; set; }
    public DateTime TimeStamp { get; set; }
}