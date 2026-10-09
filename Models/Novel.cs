using System.ComponentModel.DataAnnotations;

namespace ObsidianBlazor.Models;

public enum NovelKind
{
    [Display(Name = "Web Novel")] WebNovel,
    [Display(Name = "Light Novel")] LightNovel
}

public enum ReadingStatus
{
    Reading,
    [Display(Name = "Plan to Read")] PlanToRead,
    [Display(Name = "On Hold")] OnHold,
    Completed,
    Dropped
}

public class Novel : IValidatableObject
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, StringLength(200)]
    public string Title { get; set; } = "";

    [StringLength(120)]
    public string? Author { get; set; }

    public NovelKind Kind { get; set; } = NovelKind.WebNovel;

    public ReadingStatus Status { get; set; } = ReadingStatus.Reading;

    [Required(ErrorMessage = "A link to the novel is required.")]
    [Url(ErrorMessage = "Enter a full link, e.g. https://www.royalroad.com/fiction/...")]
    public string SourceUrl { get; set; } = "";

    [Url(ErrorMessage = "Enter a full link to the chapter.")]
    public string? LastReadUrl { get; set; }

    [Url(ErrorMessage = "Enter a full image link.")]
    public string? CoverUrl { get; set; }

    [Range(0, 100_000)]
    public int CurrentChapter { get; set; }

    [Range(1, 100_000, ErrorMessage = "Total chapters must be at least 1.")]
    public int? TotalChapters { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }

    public List<string> Tags { get; set; } = [];

    [StringLength(2000)]
    public string? Notes { get; set; }

    public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>Host of the source link without "www.", e.g. "royalroad.com".</summary>
    public string SourceSite =>
        Uri.TryCreate(SourceUrl, UriKind.Absolute, out var uri)
            ? uri.Host.StartsWith("www.") ? uri.Host[4..] : uri.Host
            : "unknown";

    /// <summary>Reading progress between 0 and 1, or null when the chapter count is unknown.</summary>
    public double? Progress =>
        TotalChapters is > 0 ? Math.Clamp((double)CurrentChapter / TotalChapters.Value, 0, 1) : null;

    /// <summary>Where "Continue reading" should take the reader.</summary>
    public string ContinueUrl => string.IsNullOrWhiteSpace(LastReadUrl) ? SourceUrl : LastReadUrl;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TotalChapters is { } total && CurrentChapter > total)
            yield return new ValidationResult("Current chapter can't be past the last chapter.", [nameof(CurrentChapter)]);
    }

    /// <summary>Guesses a title from a link's last path segment, e.g. ".../mother-of-learning" → "Mother Of Learning".</summary>
    public static string? GuessTitleFromUrl(string? url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return null;

        var slug = uri.Segments.Select(s => s.Trim('/'))
            .LastOrDefault(s => s.Length > 0 && !s.All(char.IsDigit));
        if (slug is null) return null;

        slug = Uri.UnescapeDataString(slug).Split('_')[0];
        var words = slug.Split(['-', '+', ' '], StringSplitOptions.RemoveEmptyEntries)
            .Where(w => !w.All(char.IsDigit))
            .Select(w => char.ToUpperInvariant(w[0]) + w[1..]);
        var title = string.Join(' ', words);
        return title.Length > 0 ? title : null;
    }

    public Novel Clone()
    {
        var copy = (Novel)MemberwiseClone();
        copy.Tags = [.. Tags];
        return copy;
    }
}
