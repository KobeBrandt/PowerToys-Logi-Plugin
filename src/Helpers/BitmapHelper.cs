namespace Loupedeck.PowerToysPlugin.Helpers;

public static class BitmapHelper
{
    public static BitmapImage MakeBitmapImage(String path, Int32 imageSize)
    {
        var scale = 2;
        var size = (Int32)(imageSize * scale);
        try
        {
            using var builder = new BitmapBuilder(size, size);
            builder.DrawImage(EmbeddedResources.ReadImage(path));
            return builder.ToImage();
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Failed to read image");
            return null;
        }
    }
}