using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform;
using NoterApp.Models;

namespace NoterApp.Services;

public class DataService
{
    public static DataService Instance { get; } = new();

    private readonly string _notesDirectory;
    private readonly string _indexFilePath;
    private readonly string _tagsFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    private Dictionary<Guid, NoteIndexItem> _noteIndex;

    private List<Tag> _tags;

    private List<ColorInfo> _colors;

    private DataService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "NoterApp");
        _notesDirectory = Path.Combine(appFolder, "Notes");
        _indexFilePath = Path.Combine(appFolder, "index.json");
        _tagsFilePath = Path.Combine(appFolder, "tags.json");
        Directory.CreateDirectory(_notesDirectory);

        _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        _noteIndex = LoadIndex();
        _tags = LoadTags();
        // _colors = LoadColors();
    }

    // -------------------------------------------------------------
    //
    //                      NOTES RELATED METHODS 
    //
    // -------------------------------------------------------------

    public List<NoteIndexItem> GetAllNotesFromIndex()
    {
        return _noteIndex.Values.OrderByDescending(n => n.DateCreated).ToList();
    }

    public NoteManifest CreateNewNote()
    {
        var now = DateTime.UtcNow;
        var manifest = new NoteManifest
        {
            ID = Guid.NewGuid(),
            Title = "Untitled Note",
            DateCreated = now,
            CurrentVersion = 1,
            Versions = new List<NoteVersion>
            {
                new NoteVersion { Version = 1, FileName = "1.txt", TimeStamp = now }
            }
        };

        return manifest;
    }


    public async Task<(NoteManifest Manifest, string Content)> OpenNoteAsync(Guid NoteID)
    {
        var noteDir = Path.Combine(_notesDirectory, NoteID.ToString());
        var manifestPath = Path.Combine(noteDir, "meta.json");

        if (!File.Exists(manifestPath))
        {
            throw new FileNotFoundException("Note manifest not found.", manifestPath);
        }

        var manifestJson = await File.ReadAllTextAsync(manifestPath);
        var manifest = JsonSerializer.Deserialize<NoteManifest>(manifestJson);

        var currentVersion = manifest.Versions.First(v => v.Version == manifest.CurrentVersion);
        var contentPath = Path.Combine(noteDir, currentVersion.FileName);

        var content = await File.ReadAllTextAsync(contentPath);

        return (manifest, content);
    }


    public async Task SaveNoteAsync(NoteManifest manifest, string newContent)
    {
        var now = DateTime.UtcNow;
        var noteDir = Path.Combine(_notesDirectory, manifest.ID.ToString());

        if (!Directory.Exists(noteDir))
        {
            Directory.CreateDirectory(noteDir);

            var contentPath = Path.Combine(noteDir, manifest.Versions.First().FileName);
            await File.WriteAllTextAsync(contentPath, newContent);

            var manifestJson = JsonSerializer.Serialize(manifest, _jsonOptions);
            await File.WriteAllTextAsync(Path.Combine(noteDir, "meta.json"), manifestJson);
        }
        else
        {
            var newVersionNumber = manifest.CurrentVersion + 1;
            var newFileName = $"{newVersionNumber}.txt";
            var newContentPath = Path.Combine(noteDir, newFileName);
            await File.WriteAllTextAsync(newContentPath, newContent);

            manifest.CurrentVersion = newVersionNumber;
            manifest.Versions.Add(new NoteVersion
                { Version = newVersionNumber, FileName = newFileName, TimeStamp = now });
            var manifestJson = JsonSerializer.Serialize(manifest, _jsonOptions);
            await File.WriteAllTextAsync(Path.Combine(noteDir, "meta.json"), manifestJson);
        }

        int SnippetLength = Math.Min(newContent.Length, 350);
        _noteIndex[manifest.ID] = new NoteIndexItem
        {
            ID = manifest.ID,
            Title = manifest.Title,
            BodySnippet = newContent.Substring(0, SnippetLength),
            DateModified = manifest.Versions.Last().TimeStamp,
            Tags = manifest.Tags
        };
        await SaveIndexAsync();
    }

    private Dictionary<Guid, NoteIndexItem>? LoadIndex()
    {
        if (!File.Exists(_indexFilePath))
        {
            return new Dictionary<Guid, NoteIndexItem>();
        }

        var json = File.ReadAllText(_indexFilePath);
        return JsonSerializer.Deserialize<Dictionary<Guid, NoteIndexItem>>(json) ??
               new Dictionary<Guid, NoteIndexItem>();
    }

    private async Task SaveIndexAsync()
    {
        var json = JsonSerializer.Serialize(_noteIndex, _jsonOptions);
        await File.WriteAllTextAsync(_indexFilePath, json);
    }

    // -------------------------------------------------------------
    //
    //                      TAGS RELATED METHODS 
    //
    // -------------------------------------------------------------

    public List<Tag> GetAllTags() => _tags;

    public async Task<Tag> AddTagAsync(string name, string color)
    {
        int newID = _tags.Any() ? _tags.Max(t => t.ID) + 1 : 0;
        var newTag = new Tag { ID = newID, Name = name.Trim(), ColorName = color };
        _tags.Add(newTag);
        await SaveTagsAsync();
        return newTag;
    }

    public async Task RemoveTagAsync(int tagID)
    {
        _tags.RemoveAll(t => t.ID == tagID);
        await SaveTagsAsync();
    }

    public async Task UpdateTagAsync(Tag tagToUpdate)
    {
        var existingTag = _tags.FirstOrDefault(t => t.ID == tagToUpdate.ID);
        if (existingTag != null)
        {
            existingTag.Name = tagToUpdate.Name;
            existingTag.ColorName = tagToUpdate.ColorName;
            await SaveTagsAsync();
        }
    }

    public List<Tag> LoadTags()
    {
        if (!File.Exists(_tagsFilePath)) return new List<Tag>();
        var json = File.ReadAllText(_tagsFilePath);
        return JsonSerializer.Deserialize<List<Tag>>(json) ?? new List<Tag>();
    }

    private async Task SaveTagsAsync()
    {
        var json = JsonSerializer.Serialize(_tags, _jsonOptions);
        await File.WriteAllTextAsync(_tagsFilePath, json);
    }

    public List<ColorInfo> GetColors() => _colors;
}