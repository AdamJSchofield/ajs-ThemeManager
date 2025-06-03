using Microsoft.JSInterop;
using MudBlazor.ThemeManager.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MudBlazor.ThemeManager.TestApp
{
    public interface IThemePresetStorage
    {
        Task<List<MudThemePresetInfo>> GetAllPresetInfoAsync();
        Task<MudThemePreset> GetActivePresetAsync();
        Task SetActivePresetAsync(MudThemePreset preset);
        Task<MudThemePreset> GetPresetAsync(MudThemePresetInfo presetInfo);
        Task AddPresetAsync(MudThemePreset preset);
        Task RemovePresetAsync(MudThemePresetInfo presetInfo);
        Task UpdatePresetAsync(MudThemePreset preset);
    }

    public class LocalBrowserPresetStorage(IJSRuntime js) : IThemePresetStorage
    {
        private readonly string _presetInfoKey = "MudBlazor.ThemeManager.PresetInfo";
        private readonly string _activePresetKey = "MudBlazor.ThemeManager.ActivePreset";

        private static IReadOnlyCollection<MudThemePresetInfo> _defaultPresetsInfo = new List<MudThemePresetInfo>
        {
            new MudThemePresetInfo { Name = "Default Light", Category = "Default", IsReadOnly = true, IsDarkModeDefault = false },
            new MudThemePresetInfo { Name = "Default Dark", Category = "Default", IsReadOnly = true, IsDarkModeDefault = true },
            new MudThemePresetInfo { Name = "Custom 1", Category = "Lights", IsReadOnly = false, IsDarkModeDefault = false },
            new MudThemePresetInfo { Name = "Custom 2", Category = "Darks", IsReadOnly = false, IsDarkModeDefault = true }
        };

        public async Task<List<MudThemePresetInfo>> GetAllPresetInfoAsync()
        {
            var presetInfoListJson = await GetItemAsync(_presetInfoKey);
            if (presetInfoListJson != null)
            {
                var presetInfoListWrapper = JsonSerializer.Deserialize<PresetInfoListWrapper>(presetInfoListJson);
                if (presetInfoListWrapper != null)
                {
                    return _defaultPresetsInfo.Concat(presetInfoListWrapper.Presets).ToList();
                }
            }

            return _defaultPresetsInfo.ToList();
        }

        public async Task<MudThemePreset> GetActivePresetAsync()
        {
            var activePresetInfoJson = await GetItemAsync(_activePresetKey);
            if (activePresetInfoJson != null)
            {
                var activePreset = Extension.DeserializePreset(activePresetInfoJson);
                if (activePreset != null)
                {
                    var presetJson = await GetItemAsync(GetPresetKey(activePreset.Name, activePreset.Category));
                    if (presetJson != null)
                    {
                        return Extension.DeserializePreset(presetJson);
                    }
                }
            }

            return null;
        }

        public async Task SetActivePresetAsync(MudThemePreset preset)
        {
            var presetJson = preset.ToJson();
            if (presetJson != null)
            {
                await SetItemAsync(_activePresetKey, presetJson);
            }
        }

        public async Task<MudThemePreset> GetPresetAsync(MudThemePresetInfo presetInfo)
        {
            // TODO: Check if default
            var presetJson = await GetItemAsync(GetPresetKey(presetInfo.Name, presetInfo.Category));
            if (presetJson != null)
            {
                return Extension.DeserializePreset(presetJson);
            }

            return null;
        }

        public async Task AddPresetAsync(MudThemePreset preset)
        {
            var newPresetInfo = preset.SliceToInfo();
            if (newPresetInfo != null)
            {
                var presetInfoListJson = await GetItemAsync(_presetInfoKey);
                if (presetInfoListJson != null)
                {
                    var presetInfoListWrapper = JsonSerializer.Deserialize<PresetInfoListWrapper>(presetInfoListJson) ?? new();
                    if (presetInfoListWrapper != null)
                    {
                        presetInfoListWrapper.Presets.Add(newPresetInfo);
                        var newInfoListJson = JsonSerializer.Serialize<PresetInfoListWrapper>(presetInfoListWrapper);
                        if (newInfoListJson != null)
                        {
                            await SetItemAsync(_presetInfoKey, newInfoListJson);
                            await SetItemAsync(_activePresetKey, newInfoListJson);
                        }
                    }
                }
                else
                {
                    var newWrapper = new PresetInfoListWrapper
                    {
                        Presets = new List<MudThemePresetInfo> { newPresetInfo }
                    };
                    var newInfoListJson = JsonSerializer.Serialize<PresetInfoListWrapper>(newWrapper);
                    if (newInfoListJson != null)
                    {
                        await SetItemAsync(_presetInfoKey, newInfoListJson);
                        await SetItemAsync(_activePresetKey, newInfoListJson);
                    }
                }
                
                var presetJson = preset.ToJson();
                if (presetJson != null)
                {
                    await SetItemAsync(GetPresetKey(newPresetInfo.Name, newPresetInfo.Category), presetJson);
                }
            }
        }

        public async Task RemovePresetAsync(MudThemePresetInfo presetInfo)
        {
            var presetInfoListJson = await GetItemAsync(_presetInfoKey);
            if (presetInfoListJson != null)
            {
                var presetInfoList = JsonSerializer.Deserialize<PresetInfoListWrapper>(presetInfoListJson) ?? new();
                if (presetInfoList != null)
                {
                    presetInfoList.Presets = presetInfoList.Presets.Where(p => p.Name != presetInfo.Name && p.Category != presetInfo.Category).ToList();
                    
                    var newInfoListJson = JsonSerializer.Serialize<PresetInfoListWrapper>(presetInfoList);
                    if (newInfoListJson != null)
                    {
                        await SetItemAsync(_presetInfoKey, newInfoListJson);
                        await RemoveItemAsync(GetPresetKey(presetInfo.Name, presetInfo.Category));
                    }
                }
            }
        }

        public async Task UpdatePresetAsync(MudThemePreset preset)
        {
            var presetJson = preset.ToJson();
            if (presetJson != null)
            {
                await SetItemAsync(GetPresetKey(preset.Name, preset.Category), presetJson);
            }
        }

        public async Task SetItemAsync(string key, string value) => await js.InvokeVoidAsync("localStorage.setItem", key, value);

        public async Task<string> GetItemAsync(string key) => await js.InvokeAsync<string>("localStorage.getItem", key);

        public async Task RemoveItemAsync(string key) => await js.InvokeVoidAsync("localStorage.removeItem", key);

        private string GetPresetKey(string name, string category) => $"{_presetInfoKey}.{category}.{name}";

        private class PresetInfoListWrapper
        {
            [DataMember(Name = "Presets")]
            public List<MudThemePresetInfo> Presets { get; set; } = new List<MudThemePresetInfo>();
        }
    }
}
