namespace Loupedeck.PowerToysPlugin.Helpers;

public static class BitmapHelper
{
    public static BitmapImage MakeBitmapImage(String path, PluginImageSize imageSize)
    {
        var scale = 2;
        try
        {
            using var builder = new BitmapBuilder(imageSize);
            var cw = builder.Width;
            var ch = builder.Height;
            var dw = cw / scale;
            var dh = ch / scale;
            var dx = (cw - dw) / scale;
            var dy = (ch - dh) / scale;
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