
using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager.Models;

namespace MudBlazor.ThemeManager.Components
{
    public partial class MudThemePresetSelect : ComponentBase
    {
        [Parameter]
        public IEnumerable<MudThemePresetInfo> ThemePresets { get; set; } = Enumerable.Empty<MudThemePresetInfo>();

        [Parameter]
        public MudThemePresetInfo SelectedThemePreset { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public bool SingleCategory { get; set; } = true;

        [Parameter]
        public EventCallback<ThemePresetOnChangedEventArgs> ThemePresetChanged { get; set; }

        private Task OnPresetSelected(MudThemePresetInfo presetMetadata)
        {
            return ThemePresetChanged.InvokeAsync(new ThemePresetOnChangedEventArgs
            {
                EventType = ThemePresetOnChangedEventType.Selected,
                PresetInfo = presetMetadata
            });
        }

        private Task OnPresetRemoved(MudThemePresetInfo presetMetadata)
        {
            return ThemePresetChanged.InvokeAsync(new ThemePresetOnChangedEventArgs
            {
                EventType = ThemePresetOnChangedEventType.Deleted,
                PresetInfo = presetMetadata
            });
        }
    }
}
