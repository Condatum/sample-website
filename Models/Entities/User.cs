using System.ComponentModel.DataAnnotations;

namespace ObsidianBlazor.Models.Entities;

/// <summary>A reader with an account.</summary>
public class User
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = "";

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; set; } = "";

    [Required]
    public string PasswordHash { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
