using System.ComponentModel.DataAnnotations;

namespace ObsidianBlazor.Models.Entities;

/// <summary>A web novel or light novel published on a <see cref="Source"/>.</summary>
public class Novel
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = "";

    [StringLength(120)]
    public string? Author { get; set; }

    public NovelKind Kind { get; set; } = NovelKind.WebNovel;

    /// <summary>Link to the novel's page on its source site.</summary>
    [Required, Url]
    public string Url { get; set; } = "";

    [Url]
    public string? CoverUrl { get; set; }

    [StringLength(4000)]
    public string? Synopsis { get; set; }

    /// <summary>Null while the novel is still ongoing.</summary>
    [Range(1, 100_000)]
    public int? TotalChapters { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public int SourceId { get; set; }
    public Source Source { get; set; } = default!;

    public ICollection<Tag> Tags { get; set; } = [];

    public ICollection<LibraryEntry> LibraryEntries { get; set; } = [];
}
