namespace PowerToysPlugin;

using Loupedeck;
using Loupedeck.PowerToysPlugin;
using Loupedeck.PowerToysPlugin.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

public abstract class PowerToy : ActionEditorCommand
{
    private String defaultShortcut;
    private readonly String _Icon;
    private readonly String _Name;
    private String image;

    // Helper classes for JSON deserialization
    public class PowerToysSettings
    {
        [JsonPropertyName("properties")]
        public SettingsProperties Properties { get; set; }
    }

    public class SettingsProperties
    {
        [JsonPropertyName("ActivationShortcut")]
        public ActivationShortcut ActivationShortcut { get; set; }
    }

    public class ActivationShortcut
    {
        [JsonPropertyName("win")] public Boolean Win { get; set; }
        [JsonPropertyName("ctrl")] public Boolean Ctrl { get; set; }
        [JsonPropertyName("alt")] public Boolean Alt { get; set; }
        [JsonPropertyName("shift")] public Boolean Shift { get; set; }
        [JsonPropertyName("code")] public Int32 Code { get; set; }
        [JsonPropertyName("key")] public String Key { get; set; }
    }

    public PowerToy(String name, String displayName, String shortcut, String groupName = null,
        String extraGroupName = null, String icon = null)
    {
        this._Name = name;
        // this.defaultShortcut = shortcut;
        this.DisplayName = displayName;
        this.GroupName = groupName;
        if (extraGroupName != null)
        {
            this.GroupName += "###" + extraGroupName;
        }


        if (icon != null)
        {
            this._Icon = icon;
        }
        else
        {
            this._Icon = name;
        }

        this.Description = "Activate the PowerToy for " + displayName + ".";


        // Add controls for user configuration
        this.ActionEditor.AddControlEx(
            new ActionEditorKeyboardKey("CustomShortcut", "Leave blank for default")
                .SetBehavior(ActionEditorKeyboardKeyBehavior.KeyboardKey));


        // Subscribe to events
        this.ActionEditor.ControlValueChanged += this.OnControlValueChanged;
    }

    protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, Int32 imageWidth,
        Int32 imageHeight)
    {
        try
        {
            this.image = PluginResources.FindFile(this._Icon + ".png");
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Failed to find image");
        }

        return BitmapHelper.MakeBitmapImage(this.image, imageWidth, imageHeight);
    }

    private void OnControlValueChanged(Object sender, ActionEditorControlValueChangedEventArgs e)
    {
        PluginLog.Info("e");
        e.ActionEditorState.SetVisibility("CustomShortcut", false);
        this.UpdateShortcutFromSettings();
    }
        

    protected override Boolean OnLoad()
    {
        this.UpdateShortcutFromSettings();
        return true;
    }

    private void UpdateShortcutFromSettings()
    {
        try
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var settingsPath = Path.Combine(localAppData, "Microsoft", "PowerToys", this._Name, "settings.json");

            if (!File.Exists(settingsPath))
            {
                PluginLog.Info($"Settings file not found for {this._Name} at {settingsPath}");
                return;
            }

            var jsonContent = File.ReadAllText(settingsPath);
            var settings = JsonSerializer.Deserialize<PowerToysSettings>(jsonContent);

            if (settings?.Properties?.ActivationShortcut != null)
            {
                var shortcutObj = settings.Properties.ActivationShortcut;
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
                    else
                    {
                        var keyChar = (Char)shortcutObj.Code;
                        shortcutStr += "Key" + char.ToUpper(keyChar);
                    }
                }

                if (!string.IsNullOrEmpty(shortcutStr))
                {
                    this.defaultShortcut = shortcutStr;
                    PluginLog.Info($"Updated shortcut for {this._Name} to {this.defaultShortcut}");
                }
            }
        }
        catch (Exception ex)
        {
            PluginLog.Error(ex, $"Failed to update shortcut from settings for {this._Name}");
        }
    }

    protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
    {
        if (actionParameters.TryGetString("CustomShortcut", out var actionParameter))
        {
            PluginLog.Info($"{this._Name} | Sending shortcut: {actionParameter}");
            try
            {
                // Use the built-in KeyboardShortcut command
                var shortcut = String.IsNullOrEmpty(actionParameter) ? this.defaultShortcut : actionParameter;
                if (shortcut == "None___0______")
                {
                    shortcut = this.defaultShortcut;
                }


                var letter = ShortcutHelper.GetChar(shortcut);
                if (letter != '⍼')
                {
                    this.Plugin.ClientApplication.SendKeyboardShortcut(letter, ShortcutHelper.GetModifiers(shortcut));
                    PluginLog.Info($"Sent shortcut: {letter} + {ShortcutHelper.GetModifiers(shortcut)}");
                }
                else
                {
                    this.Plugin.ClientApplication.SendKeyboardShortcut(ShortcutHelper.GetVirtualKeyCode(shortcut),
                        ShortcutHelper.GetModifiers(shortcut));
                    PluginLog.Info(
                        $"Sent shortcut: {ShortcutHelper.GetVirtualKeyCode(shortcut)} + {ShortcutHelper.GetModifiers(shortcut)}");
                }

                return true;
            }
            catch (Exception ex)
            {
                PluginLog.Error($"Failed: {ex.Message}");
                return false;
            }
        }

        return false;
    }
}