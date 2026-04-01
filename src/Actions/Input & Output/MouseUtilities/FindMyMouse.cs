namespace PowerToysPlugin.Input___Output.MouseUtilities;

using Loupedeck;
using Loupedeck.PowerToysPlugin;
using Loupedeck.PowerToysPlugin.Helpers;

public class FindMyMouse : ActionEditorCommand
{
    public FindMyMouse()
    {
        this.Name = "FindMyMouse";
        this.DisplayName = "Find My Mouse";
        this.GroupName = "Input & Output###Mouse Utilities";

        this.Description = "Activate the PowerToy for Find My Mouse (Not jet supported)";

        this.ActionEditor.AddControlEx(
            new ActionEditorListbox("ControlsSelector", "Activation method:"));

        // Subscribe to events
        this.ActionEditor.ListboxItemsRequested += this.OnListboxItemsRequested;
    }

    private void OnListboxItemsRequested(Object sender, ActionEditorListboxItemsRequestedEventArgs e)
    {
        if (e.ControlName.EqualsNoCase("ControlsSelector"))
        {
            e.AddItem("LeftControlTwice", "Press Left Control twice", "Not jet supported");
            e.AddItem("RightControlTwice", "Press Right Control twice", "Not jet supported");
            e.AddItem("CustomShortcut", "Custom shortcut", "Not jet supported");

            e.SetSelectedItemName("LeftControlTwice");
        }
    }

    protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
    {
        if (actionParameters.TryGetString("ControlsSelector", out var trigger))
        {
            switch (trigger)
            {
                case "LeftControlTwice":
                    PluginLog.Info("Left Control Twice");
                    this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlLeft, ModifierKey.Control);
                    Thread.Sleep(100);
                    this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlLeft, ModifierKey.Control);
                    break;
                case "RightControlTwice":
                    PluginLog.Info("Right Control Twice");
                    this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlRight,
                        ModifierKey.Control);
                    Thread.Sleep(100);
                    this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlRight,
                        ModifierKey.Control);
                    break;
                case "CustomShortcut":
                    PluginLog.Info("Custom Shortcut");
                    break;
            }

            return true;
        }

        return false;
    }

    protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, Int32 imageWidth,
        Int32 imageHeight)
    {
        var image = "";
        try
        {
            image = PluginResources.FindFile("FindMyMouse.png");
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Failed to find image");
        }

        return BitmapHelper.MakeBitmapImage(image, imageWidth, imageHeight);
    }
}