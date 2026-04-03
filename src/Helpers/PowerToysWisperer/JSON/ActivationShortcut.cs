namespace Loupedeck.PowerToysPlugin.Helpers.PowerToysWisperer.JSON;

using System.Text.Json.Serialization;

public class ActivationShortcut
{
    [JsonPropertyName("win")] public Boolean Win { get; set; }
    [JsonPropertyName("ctrl")] public Boolean Ctrl { get; set; }
    [JsonPropertyName("alt")] public Boolean Alt { get; set; }
    [JsonPropertyName("shift")] public Boolean Shift { get; set; }
    [JsonPropertyName("code")] public Int32 Code { get; set; }
    [JsonPropertyName("key")] public String Key { get; set; }
}