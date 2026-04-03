namespace Loupedeck.PowerToysPlugin.Helpers.PowerToysWisperer;

using System.Text.Json;

using JSON;

public static class PowerToysConnector
{
    public static String GetShortcutFromSettings(String name)
    {
        try
        {
            
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", name, "settings.json");
            
            if (!File.Exists(settingsPath))
            {
                PluginLog.Error($"{name} | Settings file not found at {settingsPath}");
                return null;
            }

            var jsonContent = File.ReadAllText(settingsPath);
            var settings = JsonSerializer.Deserialize<PowerToysSettings>(jsonContent);
            
            
            ActivationShortcut shortcutObj = null;

            if (settings?.Properties?.Hotkey?.Value != null)
            {
                shortcutObj = settings.Properties.Hotkey.Value;
            }
            else if (settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
            {
                // Try as wrapped ({"value": ...})
                if (settings.Properties.ActivationShortcut.TryGetProperty("value", out var wrappedValue) &&
                    wrappedValue.ValueKind == JsonValueKind.Object)
                {
                    shortcutObj = wrappedValue.Deserialize<ActivationShortcut>();
                }
                else
                {
                    // Try as unwrapped
                    shortcutObj = settings.Properties.ActivationShortcut.Deserialize<ActivationShortcut>();
                }
            }

            if (shortcutObj != null && (shortcutObj.Win || shortcutObj.Ctrl || shortcutObj.Alt || shortcutObj.Shift || shortcutObj.Code > 0))
            {
                var shortcutStr = "";

                if (shortcutObj.Win)
                {
                    shortcutStr += "Windows+";
                }

                if (shortcutObj.Ctrl)
                {
                    shortcutStr += "Ctrl+";
                }

                if (shortcutObj.Alt)
                {
                    shortcutStr += "Alt+";
                }

                if (shortcutObj.Shift)
                {
                    shortcutStr += "Shift+";
                }

                if (shortcutObj.Code > 0)
                {
                    if (shortcutObj.Code == 32)
                    {
                        shortcutStr += "Space";
                    }
                    else if (shortcutObj.Code == 188)
                    {
                        shortcutStr += "Key,";
                    }
                    else if (shortcutObj.Code == 190)
                    {
                        shortcutStr += "Key.";
                    }
                    else if (shortcutObj.Code == 191)
                    {
                        shortcutStr += "Key/";
                    }
                    else if (shortcutObj.Code == 186)
                    {
                        shortcutStr += "Key;";
                    }
                    else if (shortcutObj.Code == 222)
                    {
                        shortcutStr += "Key'";
                    }
                    else if (shortcutObj.Code == 219)
                    {
                        shortcutStr += "Key[";
                    }
                    else if (shortcutObj.Code == 221)
                    {
                        shortcutStr += "Key]";
                    }
                    else if (shortcutObj.Code == 220)
                    {
                        shortcutStr += "Key\\";
                    }
                    else if (shortcutObj.Code == 189)
                    {
                        shortcutStr += "Key-";
                    }
                    else if (shortcutObj.Code == 187)
                    {
                        shortcutStr += "Key=";
                    }
                    else if (shortcutObj.Code == 192)
                    {
                        shortcutStr += "Key`";
                    }
                    else if (shortcutObj.Code >= 37 && shortcutObj.Code <= 40)
                    {
                        var arrowKeys = new[] { "ArrowLeft", "ArrowUp", "ArrowRight", "ArrowDown" };
                        shortcutStr += arrowKeys[shortcutObj.Code - 37];
                    }
                    else
                    {
                        var keyChar = (Char)shortcutObj.Code;
                        shortcutStr += "Key" + char.ToUpper(keyChar);
                    }
                }

                if (!string.IsNullOrEmpty(shortcutStr))
                {
                    PluginLog.Info($"{name} | Set shortcut to: {shortcutStr} ");
                    return shortcutStr;
                }
            }
            
            PluginLog.Error($"{name} | No activation shortcut found");
            return null;
        }
        catch (Exception ex)
        {
            PluginLog.Error(ex, $"{name} | Failed to update shortcut from settings");
            return null;
        }
    }
}