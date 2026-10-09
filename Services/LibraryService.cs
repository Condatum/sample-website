using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using ObsidianBlazor.Models;

namespace ObsidianBlazor.Services;

/// <summary>
/// Keeps the user's bookshelf in the browser's localStorage (no database yet).
/// Registered as scoped, so each browser circuit gets its own instance.
/// </summary>
public class LibraryService(IJSRuntime js)
{
    private const string StorageKey = "novelshelf.library.v1";

    public static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private List<Novel> novels = [];
    private Task? loading;

    /// <summary>Raised after any change so components can re-render.</summary>
    public event Action? Changed;

    public IReadOnlyList<Novel> Novels => novels;

    public Task EnsureLoadedAsync() => loading ??= LoadAsync();

    public Novel? Find(string id) => novels.FirstOrDefault(n => n.Id == id);

    public async Task AddAsync(Novel novel)
    {
        novel.AddedAt = novel.UpdatedAt = DateTimeOffset.UtcNow;
        novels.Add(novel);
        await SaveAsync();
    }

    public async Task UpdateAsync(Novel novel)
    {
        var index = novels.FindIndex(n => n.Id == novel.Id);
        if (index < 0) return;
        novel.UpdatedAt = DateTimeOffset.UtcNow;
        novels[index] = novel;
        await SaveAsync();
    }

    public async Task RemoveAsync(string id)
    {
        novels.RemoveAll(n => n.Id == id);
        await SaveAsync();
    }

    /// <summary>Moves the chapter counter by <paramref name="delta"/>, keeping it within bounds.</summary>
    public async Task BumpChapterAsync(string id, int delta = 1)
    {
        if (Find(id) is not { } novel) return;

        var next = Math.Max(0, novel.CurrentChapter + delta);
        if (novel.TotalChapters is { } total) next = Math.Min(next, total);
        novel.CurrentChapter = next;

        if (novel.Status == ReadingStatus.PlanToRead && next > 0) novel.Status = ReadingStatus.Reading;
        if (novel.TotalChapters is { } t && next == t) novel.Status = ReadingStatus.Completed;

        novel.UpdatedAt = DateTimeOffset.UtcNow;
        await SaveAsync();
    }

    /// <summary>Serializes the whole shelf so it can be saved as a backup file.</summary>
    public string ExportJson() => JsonSerializer.Serialize(novels, JsonOptions);

    /// <summary>
    /// Merges novels from a backup: entries with a matching id are replaced, the rest are added.
    /// Returns how many novels were imported.
    /// </summary>
    public async Task<int> ImportJsonAsync(string json)
    {
        var imported = JsonSerializer.Deserialize<List<Novel>>(json, JsonOptions) ?? [];
        imported = imported.Where(n => !string.IsNullOrWhiteSpace(n.Title) && !string.IsNullOrWhiteSpace(n.SourceUrl)).ToList();

        foreach (var novel in imported)
        {
            var index = novels.FindIndex(n => n.Id == novel.Id);
            if (index >= 0) novels[index] = novel;
            else novels.Add(novel);
        }

        await SaveAsync();
        return imported.Count;
    }

    public async Task ResetToSamplesAsync()
    {
        novels = SampleNovels.Create();
        await SaveAsync();
    }

    public async Task ClearAsync()
    {
        novels = [];
        await SaveAsync();
    }

    private async Task LoadAsync()
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (json is null)
        {
            // First visit: start with a few examples so the shelf isn't empty.
            novels = SampleNovels.Create();
            await SaveAsync();
            return;
        }

        try
        {
            novels = JsonSerializer.Deserialize<List<Novel>>(json, JsonOptions) ?? [];
        }
        catch (JsonException)
        {
            novels = [];
        }
    }

    private async Task SaveAsync()
    {
        await js.InvokeVoidAsync("localStorage.setItem", StorageKey, ExportJson());
        Changed?.Invoke();
    }
}
