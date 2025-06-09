
using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager.Extensions;
using MudBlazor.ThemeManager.Models;

namespace MudBlazor.ThemeManager.Components
{
    public partial class MudThemePresetSelect : ComponentBase
    {
        private string? _newPresetName;

        [CascadingParameter]
        public MudThemeManager ThemeManager { get; set; }

        [Parameter]
        public IEnumerable<MudThemePresetInfo> ThemePresetInfos { get; set; } = Enumerable.Empty<MudThemePresetInfo>();

        [Parameter]
        public MudThemePresetInfo ThemePreset { get; set; } = new();

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public EventCallback<ThemePresetOnChangedEvent> ThemePresetChanged { get; set; }

        private Task OnPresetSelected(MudThemePresetInfo presetMetadata)
        {
            return ThemePresetChanged.InvokeAsync(new ThemePresetOnChangedEvent
            {
                EventType = ThemePresetOnChangedEventType.Selected,
                PresetInfo = presetMetadata
            });
        }

        private Task OnPresetRemoved(MudThemePresetInfo presetMetadata)
        {
            return ThemePresetChanged.InvokeAsync(new ThemePresetOnChangedEvent
            {
                EventType = ThemePresetOnChangedEventType.Deleted,
                PresetInfo = presetMetadata
            });
        }

        private Task OnPresetAdded(string name, string category)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(category))
            {
                var copyInfo = ThemeManager.ThemePreset?.SliceToInfo();
                if (copyInfo != null)
                {
                    copyInfo.Name = name;
                    copyInfo.Category = category;
                    return ThemePresetChanged.InvokeAsync(new ThemePresetOnChangedEvent
                    {
                        EventType = ThemePresetOnChangedEventType.Added,
                        PresetInfo = copyInfo
                    });
                }
            }
            return Task.CompletedTask;
        }
    }
}
