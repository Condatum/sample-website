using System.ComponentModel.DataAnnotations;

namespace ObsidianBlazor.Models.Entities;

/// <summary>
/// A novel on a user's shelf: the user's own status, progress, rating and notes.
/// Each user can have a given novel on their shelf only once.
/// </summary>
public class LibraryEntry : IValidatableObject
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int NovelId { get; set; }
    public Novel Novel { get; set; } = default!;

    public ReadingStatus Status { get; set; } = ReadingStatus.PlanToRead;

    [Range(0, 100_000)]
    public int CurrentChapter { get; set; }

    /// <summary>Link to the last chapter read, used by "Continue reading".</summary>
    [Url]
    public string? LastReadUrl { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>Reading progress between 0 and 1, or null when the novel's chapter count is unknown.</summary>
    public double? Progress =>
        Novel?.TotalChapters is { } total and > 0
            ? Math.Clamp((double)CurrentChapter / total, 0, 1)
            : null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Novel?.TotalChapters is { } total && CurrentChapter > total)
            yield return new ValidationResult("Current chapter can't be past the last chapter.", [nameof(CurrentChapter)]);
    }
}
