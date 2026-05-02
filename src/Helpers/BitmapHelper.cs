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
            var dw = (Int32)(cw / scale);
            var dh = (Int32)(ch / scale);
            var dx = (Int32)((cw - dw) / scale);
            var dy = (Int32)((ch - dh) / scale);
            builder.DrawImage(EmbeddedResources.ReadImage(path), dx, dy, dw, dh);
            return builder.ToImage();
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Failed to read image");
            return null;
        }
    }
}