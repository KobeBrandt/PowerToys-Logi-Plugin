namespace PowerToysPlugin;

using Loupedeck;
using Loupedeck.PowerToysPlugin;
using Loupedeck.PowerToysPlugin.Helpers;

public abstract class PowerToy : ActionEditorCommand
{
    private readonly String _DefaultShortcut;
    private readonly String _Icon;
    private readonly String _Name;
    private String image;

    public PowerToy(String name, String displayName, String shortcut, String groupName = null,
        String extraGroupName = null, String icon = null)
    {
        this._Name = name;
        this._DefaultShortcut = shortcut;
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

    private void OnControlValueChanged(Object sender, ActionEditorControlValueChangedEventArgs e) =>
        e.ActionEditorState.SetVisibility("CustomShortcut", false);

    protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
    {
        if (actionParameters.TryGetString("CustomShortcut", out var actionParameter))
        {
            PluginLog.Info($"Sending shortcut: {actionParameter}");
            try
            {
                // Use the built-in KeyboardShortcut command
                var shortcut = String.IsNullOrEmpty(actionParameter) ? this._DefaultShortcut : actionParameter;
                if (shortcut == "None___0______")
                {
                    shortcut = this._DefaultShortcut;
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