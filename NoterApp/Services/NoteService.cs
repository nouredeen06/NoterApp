using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NoterApp.Models;

namespace NoterApp.Services;

public class NoteService
{
    private static readonly Lazy<NoteService> _instance = new(() => new NoteService());
    public static NoteService Instance => _instance.Value;

    private readonly string _notesDirectory;
    private readonly string _indexFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    private Dictionary<Guid, NoteIndexItem> _noteIndex;

    private NoteService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "NoterApp");
        _notesDirectory = Path.Combine(appFolder, "Notes");
        _indexFilePath = Path.Combine(appFolder, "index.json");
        Directory.CreateDirectory(_notesDirectory);

        _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        _noteIndex = LoadIndex();
    }

    public List<NoteIndexItem> GetAllNotesFromIndex()
    {
        return _noteIndex.Values.OrderByDescending(n => n.DateCreated).ToList();
    }

    public async Task<NoteManifest> CreateNewNoteAsync()
    {
        var newId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var manifest = new NoteManifest
        {
            ID = newId,
            Title = "Untitled Note",
            DateCreated = now,
            CurrentVersion = 1,
            Versions = new List<NoteVersion>
            {
                new NoteVersion { Version = 1, FileName = "1.txt", TimeStamp = now }
            }
        };

        var NoteDir = Path.Combine(_notesDirectory, newId.ToString());
        Directory.CreateDirectory(NoteDir);

        await File.WriteAllTextAsync(Path.Combine(NoteDir, "1.txt"), "Untitled Note");
        var manifestJson = JsonSerializer.Serialize(manifest, _jsonOptions);
        await File.WriteAllTextAsync(Path.Combine(NoteDir, "meta.json"), manifestJson);

        _noteIndex[newId] = new NoteIndexItem { Title = manifest.Title, DateCreated = now };
        await SaveIndexAsync();

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

        var newVersionNumber = manifest.CurrentVersion + 1;
        var newFileName = $"{newVersionNumber}.txt";
        var newContentPath = Path.Combine(noteDir, newFileName);
        await File.WriteAllTextAsync(newContentPath, newContent);

        manifest.CurrentVersion = newVersionNumber;
        manifest.Versions.Add(new NoteVersion
            { Version = newVersionNumber, FileName = newFileName, TimeStamp = now });

        var manifestJson = JsonSerializer.Serialize(manifest, _jsonOptions);
        await File.WriteAllTextAsync(Path.Combine(noteDir, "meta.json"), manifestJson);

        _noteIndex[manifest.ID].Title = manifest.Title;
        _noteIndex[manifest.ID].DateModified = now;
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
}