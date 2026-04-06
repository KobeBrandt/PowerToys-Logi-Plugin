namespace PowerToysPlugin.Input___Output.MouseUtilities;

using Loupedeck;
using Loupedeck.PowerToysPlugin;
using Loupedeck.PowerToysPlugin.Helpers.PowerToysWisperer;

public class FindMyMouse : PowerToy
{
    private Int32 _activationMethod;

    public FindMyMouse()
        : base(name: "FindMyMouse", displayName: "Find My Mouse", shortcut: "", groupName: "Input & Output###Mouse Utilities")
    {
    }

    protected override Boolean OnLoad()
    {
        this._activationMethod = PowerToysConnector.GetActivationMethodFromSettings("FindMyMouse");
        return base.OnLoad();
    }

    protected override void RunCommand(String actionParameters)
    {
        switch (this._activationMethod)
        {
            case 0:
                PluginLog.Info($"Activation method: {this._activationMethod}");
                this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlLeft, ModifierKey.Control);
                Thread.Sleep(100);
                this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlLeft, ModifierKey.Control);
                break;
            case 1:
                this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlRight, ModifierKey.Control);
                Thread.Sleep(100);
                this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlRight, ModifierKey.Control);
                break;
            case 3:
                var shortcut = PowerToysConnector.GetShortcutFromSettings("FindMyMouse");
                if (!string.IsNullOrEmpty(shortcut))
                {
                    this.defaultShortcut = shortcut;
                }
                break;
        }
    }
}