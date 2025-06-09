namespace MudBlazor.ThemeManager.Models
{
    public class ThemePresetOnChangedEvent
    {
        public required ThemePresetOnChangedEventType EventType { get; set; }
        public required MudThemePresetInfo PresetInfo { get; set; }
    }

    public enum ThemePresetOnChangedEventType
    {
        Selected,
        Added,
        Deleted,
        Updated
    }
}
