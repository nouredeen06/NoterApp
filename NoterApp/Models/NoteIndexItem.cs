using System;
using System.Collections.Generic;

namespace NoterApp.Models;

public class NoteIndexItem
{
    public Guid ID { get; set; }
    public string Title { get; set; }
    public string BodySnippet { get; set; }
    public DateTime DateModified { get; set; }
    public DateTime DateCreated { get; set; }
    public List<string> Tags { get; set; }
    public string Group { get; set; }
}