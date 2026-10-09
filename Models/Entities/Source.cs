using System.ComponentModel.DataAnnotations;

namespace ObsidianBlazor.Models.Entities;

/// <summary>A website novels are read on, e.g. Royal Road or Webnovel.</summary>
public class Source
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    /// <summary>Normalized host without "www.", e.g. "royalroad.com". Unique per source.</summary>
    [Required, StringLength(253)]
    public string Host { get; set; } = "";

    [Required, Url]
    public string BaseUrl { get; set; } = "";

    /// <summary>Returns the host of <paramref name="url"/> without "www.", or null if it isn't a valid link.</summary>
    public static string? NormalizeHost(string? url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri)
            ? (uri.Host.StartsWith("www.") ? uri.Host[4..] : uri.Host).ToLowerInvariant()
            : null;
}
