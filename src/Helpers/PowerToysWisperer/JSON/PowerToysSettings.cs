namespace Loupedeck.PowerToysPlugin.Helpers.PowerToysWisperer.JSON;

using System.Text.Json.Serialization;

public class PowerToysSettings
{
    [JsonPropertyName("properties")]
    public SettingsProperties Properties { get; set; }
}