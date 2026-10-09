using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ObsidianBlazor.Models;

public static class EnumExtensions
{
    /// <summary>Returns the [Display(Name = ...)] of an enum value, or its name.</summary>
    public static string DisplayName(this Enum value) =>
        value.GetType().GetField(value.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name
        ?? value.ToString();
}
