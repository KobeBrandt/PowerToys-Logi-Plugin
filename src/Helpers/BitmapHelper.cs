namespace Loupedeck.PowerToysPlugin.Helpers;

public static class BitmapHelper
{
    public static BitmapImage MakeBitmapImage(String path, Int32 imageWidth, Int32 imageHeight)
    {
        var scale = 3.5;
        var imageSizeDouble = imageWidth * scale;
        var imageSize = (Int32)imageSizeDouble;
        try
        {
            using var builder = new BitmapBuilder(imageSize, imageSize);
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