namespace Loupedeck.PowerToysPlugin.Helpers;

public static class BitmapHelper
{
    public static BitmapImage MakeBitmapImage(String path, PluginImageSize imageSize)
    {
        var scale = 1.5;
        try
        {
            using var builder = new BitmapBuilder(imageSize);
            var cw = builder.Width;
            var ch = builder.Height;
            Int32 dw = (int) (cw / scale);
            var dh = (int)(ch / scale);
            var dx = (int)((cw - dw) / scale);
            var dy = (int)((ch - dh) / scale);
            builder.DrawImage(EmbeddedResources.ReadImage(path), dx, dy, dw, dh);
            return builder.ToImage();
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Failed to read image");
            return null;
        }
    }
    public static BitmapImage MakeBitmapImage(String path, Int32 imageWidth)
    {
        var scale = 4;
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