using System.ComponentModel.DataAnnotations;

namespace ObsidianBlazor.Models.Entities;

/// <summary>A genre or label such as "Fantasy" or "Isekai", shared across novels.</summary>
public class Tag
{
    public int Id { get; set; }

    [Required, StringLength(40)]
    public string Name { get; set; } = "";

    public ICollection<Novel> Novels { get; set; } = [];
}
