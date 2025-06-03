using System.Text.Json;

namespace MudBlazor.ThemeManager.Extensions;

public static class Extension
{
    private static readonly PaletteSerializerContext PaletteSerializerContext = new();
    private static readonly ThemeSerializerContext ThemeSerializerContext = new();

    public static MudTheme DeepClone(this MudTheme source)
    {
        var themeType = typeof(MudTheme);
        var serializeStr = JsonSerializer.Serialize(source, themeType, ThemeSerializerContext);
        var copyObj = (MudTheme?)JsonSerializer.Deserialize(serializeStr, themeType, ThemeSerializerContext);

        return copyObj ?? new MudTheme();
    }

    public static MudThemePreset DeepClone(this MudThemePreset source)
    {
        var themeType = typeof(MudThemePreset);
        var serializeStr = JsonSerializer.Serialize(source, themeType, ThemeSerializerContext);
        var copyObj = (MudThemePreset?)JsonSerializer.Deserialize(serializeStr, themeType, ThemeSerializerContext);

        return copyObj ?? throw new InvalidOperationException("Failed to deep clone MudThemePreset.");
    }

    public static MudThemePresetInfo SliceToInfo(this MudThemePreset source)
    {
        var themeType = typeof(MudThemePresetInfo);
        var serializeStr = JsonSerializer.Serialize(source, themeType, ThemeSerializerContext);
        var copyObj = (MudThemePresetInfo?)JsonSerializer.Deserialize(serializeStr, themeType, ThemeSerializerContext);

        return copyObj ?? throw new InvalidOperationException("Failed to deep clone MudThemePreset to MudThemePresetInfo.");
    }

    public static PaletteDark? DeepClone(this PaletteDark source) => DeepClonePalette(source);

    public static PaletteLight? DeepClone(this PaletteLight source) => DeepClonePalette(source);

    private static T? DeepClonePalette<T>(T source) where T : Palette
    {
        var paletteType = typeof(T);
        var serializeStr = JsonSerializer.Serialize(source, paletteType, PaletteSerializerContext);
        var copyObj = (T?)JsonSerializer.Deserialize(serializeStr, paletteType, PaletteSerializerContext);

        return copyObj;
    }

    public static string SerializePreset(MudThemePreset source)
    {
        var themeType = typeof(MudThemePreset);
        return JsonSerializer.Serialize(source, themeType, ThemeSerializerContext);
    }

    public static MudThemePreset? DeserializePreset(string serializeStr)
    {
        var themeType = typeof(MudThemePreset);
        return (MudThemePreset?)JsonSerializer.Deserialize(serializeStr, themeType, ThemeSerializerContext);
    }

    public static string SerializePresetInfo(MudThemePresetInfo source)
    {
        var themeType = typeof(MudThemePresetInfo);
        return JsonSerializer.Serialize(source, themeType, ThemeSerializerContext);
    }

    public static MudThemePresetInfo? DeserializePresetInfo(string serializeStr)
    {
        var themeType = typeof(MudThemePresetInfo);
        return (MudThemePresetInfo?)JsonSerializer.Deserialize(serializeStr, themeType, ThemeSerializerContext);
    }
}