namespace Loupedeck.PowerToysPlugin.Helpers.PowerToysWisperer.JSON;

using System.Text.Json.Serialization;

public class SettingValue<T>
{
    [JsonPropertyName("value")]
    public T Value { get; set; }
}

public class SettingsProperties
{
    [JsonPropertyName("hotkey")]
    public SettingValue<ActivationShortcut> Hotkey { get; set; }

    [JsonPropertyName("ActivationShortcut")]
    public System.Text.Json.JsonElement ActivationShortcut { get; set; }

    [JsonPropertyName("advanced-paste-ui-hotkey")]
    public ActivationShortcut AdvancedPasteUiHotkey { get; set; }

    [JsonPropertyName("paste-as-plain-hotkey")]
    public ActivationShortcut PasteAsPlainHotkey { get; set; }

    [JsonPropertyName("paste-as-markdown-hotkey")]
    public ActivationShortcut PasteAsMarkdownHotkey { get; set; }

    [JsonPropertyName("paste-as-json-hotkey")]
    public ActivationShortcut PasteAsJsonHotkey { get; set; }

    [JsonPropertyName("toggle-theme-hotkey")]
    public SettingValue<ActivationShortcut> ToggleThemeHotkey { get; set; }
    
    [JsonPropertyName("open_powerlauncher")]
    public ActivationShortcut open_powerlauncher { get; set; }

    [JsonPropertyName("DefaultOpenPowerLauncher")]
    public ActivationShortcut DefaultOpenPowerLauncher { get; set; }

    [JsonPropertyName("open_shortcutguide")]
    public ActivationShortcut open_shortcutguide { get; set; }

    [JsonPropertyName("DefaultOpenShortcutGuide")]
    public ActivationShortcut DefaultOpenShortcutGuide { get; set; }
}