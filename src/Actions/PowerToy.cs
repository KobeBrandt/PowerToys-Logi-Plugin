namespace PowerToysPlugin;

using Loupedeck;
using Loupedeck.PowerToysPlugin;
using Loupedeck.PowerToysPlugin.Helpers;
using Loupedeck.PowerToysPlugin.Helpers.PowerToysWisperer;

public abstract class PowerToy : PluginDynamicCommand
{
    private String defaultShortcut;
    private readonly String _Icon;
    private readonly String _Name;
    private String image;

    public PowerToy(String name, String displayName, String shortcut, String groupName = null,
        String extraGroupName = null, String icon = null)
        : base(displayName: displayName, description: "Activate the PowerToy for " + displayName + ".", groupName: groupName)
    {
        this._Name = name;
        this.defaultShortcut = shortcut;
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
    }

    protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
    {
        try
        {
            this.image = PluginResources.FindFile(this._Icon + ".png");
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Failed to find image");
        }
    
        return BitmapHelper.MakeBitmapImage(this.image, imageSize);
    }
    
        

    protected override Boolean OnLoad()
    {
        var settingsShortcut = PowerToysConnector.GetShortcutFromSettings(this._Name);
        if (!string.IsNullOrEmpty(settingsShortcut))
        {
            this.defaultShortcut = settingsShortcut;
        }
        return true;
    }

    protected override void RunCommand(String actionParameters)
    {
            PluginLog.Info($"{this._Name} | Sending shortcut: {this.defaultShortcut}");
            try
            {
                // Use the built-in KeyboardShortcut command
                var shortcut = String.IsNullOrEmpty(this.defaultShortcut) ? this.defaultShortcut : this.defaultShortcut;
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
                
            }
            catch (Exception ex)
            {
                PluginLog.Error($"Failed: {ex.Message}");
            }
    }
}