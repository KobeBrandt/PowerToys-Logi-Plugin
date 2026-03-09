namespace PowerToysPlugin.Input___Output.MouseUtilities;

using Loupedeck;
using Loupedeck.PowerToysPlugin;
using Loupedeck.PowerToysPlugin.Helpers;

public class FindMyMouse: ActionEditorCommand
{
    public FindMyMouse()
    {
        this.Name = "FindMyMouse";
        this.DisplayName = "Find My Mouse";
        this.GroupName = "Input & Output###Mouse Utilities";
        
        this.Description = "Activate the PowerToy for Find My Mouse (Not jet supported)";
        
        this.ActionEditor.AddControlEx(
            new ActionEditorListbox(name: "ControlsSelector", labelText: "Activation method:"));

        // Subscribe to events
        this.ActionEditor.ListboxItemsRequested += this.OnListboxItemsRequested;
    }
    private void OnListboxItemsRequested(Object sender, ActionEditorListboxItemsRequestedEventArgs e)
    {
        if (e.ControlName.EqualsNoCase("ControlsSelector"))
        {
            e.AddItem(name: "LeftControlTwice", displayName: "Press Left Control twice", description: "Not jet supported");
            e.AddItem(name: "RightControlTwice", displayName: "Press Right Control twice", description: "Not jet supported");
            e.AddItem(name: "CustomShortcut", displayName: "Custom shortcut", description: "Not jet supported");
            
            e.SetSelectedItemName("LeftControlTwice");
        }
    }
    
    protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
    {
        if (actionParameters.TryGetString("ControlsSelector", out var trigger))
        {
            // switch (trigger)
            // {
            //     case "LeftControlTwice":
            //         this.Plugin.ClientApplication.SendKeyboardShortcut( VirtualKeyCode.ControlLeft);
            //         break;
            //     case "RightControlTwice":
            //         this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.ControlRight);
            //         break;
            //     case "CustomShortcut":
            //         break;
            // }

            return true;
        }

        return false;
    }
    protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, int imageWidth, int imageHeight)
    {
        string image = "";
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