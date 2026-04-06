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

            if (name is "Reparent" or "Thumbnail")
            {
                settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", "CropAndLock", "settings.json");
            }
            else if (name == "CommandPalette")
            {
                settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", "CommandPalette", "settings.json");
            }
            else if (name == "PowerToysRun")
            {
                settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", "PowerToys Run", "settings.json");
            }
            else if (name == "ScreenRuler")
            {
                settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", "Measure Tool", "settings.json");
            }
            else if (name == "ShortcutGuide")
            {
                settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", "Shortcut Guide", "settings.json");
            }
            else if (name == "MouseHighlighter" || name == "MousePointerCrosshairs" || name == "MouseJump" || name == "CursorWrap")
            {
                settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", "MouseUtils", "settings.json");
            }
            
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
            else if (name == "Reparent" && settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
            {
                if (settings.Properties.ActivationShortcut.TryGetProperty("ReparentHotkey", out var val))
                {
                    if (val.TryGetProperty("value", out var v))
                        shortcutObj = v.Deserialize<ActivationShortcut>();
                }
            }
            else if (name == "Reparent" && settings?.Properties?.ReparentHotkey?.Value != null)
            {
                shortcutObj = settings.Properties.ReparentHotkey.Value;
            }
            else if (name == "Thumbnail" && settings?.Properties?.ThumbnailHotkey?.Value != null)
            {
                shortcutObj = settings.Properties.ThumbnailHotkey.Value;
            }
            else if (name == "PowerToysRun")
            {
                if (settings?.Properties?.open_powerlauncher != null)
                {
                    shortcutObj = settings.Properties.open_powerlauncher;
                }
                else if (settings?.Properties?.DefaultOpenPowerLauncher != null)
                {
                    shortcutObj = settings.Properties.DefaultOpenPowerLauncher;
                }
                else
                {
                    var doc = JsonDocument.Parse(jsonContent);
                    if (doc.RootElement.TryGetProperty("properties", out var props))
                    {
                        if (props.TryGetProperty("open_powerlauncher", out var pl))
                        {
                            shortcutObj = pl.Deserialize<ActivationShortcut>();
                        }
                        else if (props.TryGetProperty("DefaultOpenPowerLauncher", out var dpl))
                        {
                            shortcutObj = dpl.Deserialize<ActivationShortcut>();
                        }
                        else if (props.TryGetProperty("ActivationShortcut", out var act) && act.ValueKind == JsonValueKind.Object)
                        {
                            if (act.TryGetProperty("open_powerlauncher", out var val))
                            {
                                if (val.TryGetProperty("value", out var v))
                                    shortcutObj = v.Deserialize<ActivationShortcut>();
                            }
                        }
                    }
                }
            }
            else if (name == "Screenshot" && settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
            {
                if (settings.Properties.ActivationShortcut.TryGetProperty("SnapshotHotkey", out var val))
                {
                    if (val.TryGetProperty("value", out var v))
                        shortcutObj = v.Deserialize<ActivationShortcut>();
                }
            }
            else if (name == "AdvancedPaste")
            {
                if (settings?.Properties?.AdvancedPasteUiHotkey != null)
                {
                    shortcutObj = settings.Properties.AdvancedPasteUiHotkey;
                }
                else if (settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
                {
                    if (settings.Properties.ActivationShortcut.TryGetProperty("AdvancedPasteHotkey", out var val))
                    {
                        if (val.TryGetProperty("value", out var v))
                            shortcutObj = v.Deserialize<ActivationShortcut>();
                    }
                }
            }
            else if (name == "LightSwitch")
            {
                if (settings?.Properties?.ToggleThemeHotkey?.Value != null)
                {
                    shortcutObj = settings.Properties.ToggleThemeHotkey.Value;
                }
            }
            else if (name == "ShortcutGuide")
            {
                if (settings?.Properties?.open_shortcutguide != null)
                {
                    shortcutObj = settings.Properties.open_shortcutguide;
                }
                else if (settings?.Properties?.DefaultOpenShortcutGuide != null)
                {
                    shortcutObj = settings.Properties.DefaultOpenShortcutGuide;
                }
            }
            else if (name == "QuickAccent")
            {
                var doc = JsonDocument.Parse(jsonContent);
                if (doc.RootElement.TryGetProperty("properties", out var props))
                {
                    if (props.TryGetProperty("activation_key", out var actKey))
                    {
                        if (actKey.ValueKind == JsonValueKind.Object)
                        {
                            if (actKey.TryGetProperty("value", out var val))
                            {
                                shortcutObj = val.Deserialize<ActivationShortcut>();
                            }
                        }
                        else if (actKey.ValueKind == JsonValueKind.Number && actKey.TryGetInt32(out var code) && code > 0)
                        {
                            shortcutObj = new ActivationShortcut { Code = code };
                        }
                        else if (actKey.ValueKind == JsonValueKind.Number && actKey.TryGetInt32(out var zeroCode) && zeroCode == 0)
                        {
                            shortcutObj = new ActivationShortcut { Code = 39 };
                        }
                    }
                }
            }
            else if (name == "FancyZones")
            {
                var doc = JsonDocument.Parse(jsonContent);
                if (doc.RootElement.TryGetProperty("properties", out var props))
                {
                    if (props.TryGetProperty("fancyzones_editor_hotkey", out var hotkey) && hotkey.ValueKind == JsonValueKind.Object)
                    {
                        if (hotkey.TryGetProperty("value", out var v))
                        {
                            shortcutObj = v.Deserialize<ActivationShortcut>();
                        }
                    }
                }
            }
            else if (name == "MouseHighlighter" && settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
            {
                if (settings.Properties.ActivationShortcut.TryGetProperty("ActivationShortcut", out var val))
                {
                    if (val.TryGetProperty("value", out var v))
                        shortcutObj = v.Deserialize<ActivationShortcut>();
                }
            }
            else if (name == "MousePointerCrosshairs" && settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
            {
                if (settings.Properties.ActivationShortcut.TryGetProperty("ActivationShortcut", out var val))
                {
                    if (val.TryGetProperty("value", out var v))
                        shortcutObj = v.Deserialize<ActivationShortcut>();
                }
            }
            else if (name == "MouseJump" && settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
            {
                if (settings.Properties.ActivationShortcut.TryGetProperty("ActivationShortcut", out var val))
                {
                    if (val.TryGetProperty("value", out var v))
                        shortcutObj = v.Deserialize<ActivationShortcut>();
                }
            }
            else if (name == "CursorWrap" && settings?.Properties?.ActivationShortcut.ValueKind == JsonValueKind.Object)
            {
                if (settings.Properties.ActivationShortcut.TryGetProperty("ActivationShortcut", out var val))
                {
                    if (val.TryGetProperty("value", out var v))
                        shortcutObj = v.Deserialize<ActivationShortcut>();
                }
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