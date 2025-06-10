namespace MudBlazor.ThemeManager
{
    public abstract class MudThemePresetManager
    {
        public abstract Task<List<MudThemePresetInfo>> GetAllPresetInfoAsync();
        public abstract Task<MudThemePreset?> GetActivePresetAsync();
        public abstract Task SetActivePresetAsync(MudThemePresetInfo presetInfo);
        public abstract Task<MudThemePreset?> GetPresetAsync(MudThemePresetInfo presetInfo);
        public abstract Task AddPresetAsync(MudThemePreset preset);
        public abstract Task RemovePresetAsync(MudThemePresetInfo presetInfo);
        public abstract Task UpdatePresetAsync(MudThemePreset preset);
    }
}
