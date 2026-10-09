using ObsidianBlazor.Models;

namespace ObsidianBlazor.Services;

/// <summary>Example entries shown the first time someone opens the library.</summary>
public static class SampleNovels
{
    public static List<Novel> Create()
    {
        var now = DateTimeOffset.UtcNow;
        return
        [
            new()
            {
                Title = "Lord of the Mysteries",
                Author = "Cuttlefish That Loves Diving",
                Kind = NovelKind.WebNovel,
                Status = ReadingStatus.Reading,
                SourceUrl = "https://www.webnovel.com/book/lord-of-the-mysteries_11022733006234505",
                CurrentChapter = 412,
                TotalChapters = 1432,
                Rating = 5,
                Tags = ["Mystery", "Fantasy", "Steampunk"],
                UpdatedAt = now.AddHours(-3)
            },
            new()
            {
                Title = "Mother of Learning",
                Author = "nobody103",
                Kind = NovelKind.WebNovel,
                Status = ReadingStatus.Completed,
                SourceUrl = "https://www.royalroad.com/fiction/21220/mother-of-learning",
                CurrentChapter = 109,
                TotalChapters = 109,
                Rating = 5,
                Tags = ["Time Loop", "Magic"],
                UpdatedAt = now.AddDays(-12)
            },
            new()
            {
                Title = "Mushoku Tensei: Jobless Reincarnation",
                Author = "Rifujin na Magonote",
                Kind = NovelKind.LightNovel,
                Status = ReadingStatus.OnHold,
                SourceUrl = "https://ncode.syosetu.com/n9669bk/",
                CurrentChapter = 8,
                TotalChapters = 26,
                Tags = ["Isekai", "Adventure"],
                UpdatedAt = now.AddDays(-30)
            },
            new()
            {
                Title = "Omniscient Reader's Viewpoint",
                Author = "singNsong",
                Kind = NovelKind.WebNovel,
                Status = ReadingStatus.PlanToRead,
                SourceUrl = "https://www.novelupdates.com/series/omniscient-readers-viewpoint/",
                CurrentChapter = 0,
                TotalChapters = 551,
                Tags = ["Apocalypse", "Action"],
                UpdatedAt = now.AddDays(-2)
            },
            new()
            {
                Title = "Re:Zero − Starting Life in Another World",
                Author = "Tappei Nagatsuki",
                Kind = NovelKind.LightNovel,
                Status = ReadingStatus.Reading,
                SourceUrl = "https://yenpress.com/series/rezero-starting-life-in-another-world-light-novel",
                CurrentChapter = 14,
                TotalChapters = null,
                Rating = 4,
                Tags = ["Isekai", "Psychological"],
                UpdatedAt = now.AddDays(-1)
            }
        ];
    }
}
