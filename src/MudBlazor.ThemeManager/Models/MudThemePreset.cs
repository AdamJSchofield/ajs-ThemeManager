using MudBlazor.ThemeManager.Extensions;
using System.Text.Json;

namespace MudBlazor.ThemeManager.Models;

/// <summary>
/// Contains all details about a theme preset including the underlying theme and other properties.
/// Used when a full theme is saved or loaded for display.
/// </summary>
/// <remarks>Inherits <see cref="MudThemePresetBase"></see></remarks>
public class MudThemePreset : MudThemePresetInfo
{
    public MudTheme Theme { get; set; } = new();

    public bool RTL { get; set; }

    public string FontFamily { get; set; } = "Roboto";

    public int DefaultBorderRadius { get; set; } = 4;

    public int DefaultElevation { get; set; } = 1;

    public int AppBarElevation { get; set; } = 25;

    public int DrawerElevation { get; set; } = 2;

    public DrawerClipMode DrawerClipMode { get; set; } = DrawerClipMode.Never;

    public string? ToJson()
    {
        return JsonSerializer.Serialize(this, typeof(MudThemePreset), ThemeSerializerContext.Default);
    }
}

/// <summary>
/// Thin version of <see cref="MudThemePreset"></see> without underlying theme and other properties.
/// Used for passing meta state around in the UI without the overhead of a full theme object.
/// </summary>
/// <remarks>Inherits <see cref="MudThemePresetBase"></see></remarks>
public class MudThemePresetInfo
{
    public string Name { get; set; } = "Default Preset";

    public string Category { get; set; } = "Default Category";

    public bool IsReadOnly { get; set; }

    public bool IsDarkModeDefault { get; set; }
}