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
}