namespace Loupedeck.PowerToysPlugin.Helpers;

using System.Text.RegularExpressions;

public static class ShortcutHelper
{
    public static ModifierKey GetModifiers(String shortcut)
    {
        var keys = shortcut.Split('+').Select(k => k.Trim()).ToArray();

        var modifiers = ModifierKey.None;

        foreach (var key in keys)
        {
            switch (key.ToLower())
            {
                case "win":
                case "windows":
                    modifiers |= ModifierKey.Windows;
                    break;
                case "ctrl":
                case "control":
                    modifiers |= ModifierKey.Control;
                    break;
                case "alt":
                    modifiers |= ModifierKey.Alt;
                    break;
                case "shift":
                    modifiers |= ModifierKey.Shift;
                    break;
            }
        }

        return modifiers;
    }

    public static Char GetChar(String shortcut)
    {
        var matchKey = Regex.Match(shortcut, @"Key([A-Z`1-9!@#$%^&*])");
        if (matchKey.Success)
        {
            var key = matchKey.Groups[1].Value[0];
            return key.ToLower();
        }

        var matchSpace = Regex.Match(shortcut, @"Space");
        if (matchSpace.Success)
        {
            return ' ';
        }

        var matchSlash = Regex.Match(shortcut, @"Key/");
        if (matchSlash.Success)
        {
            return '/';
        }

        return '⍼';
    }

    public static VirtualKeyCode GetVirtualKeyCode(String shortcut)
    {
        var matchArrowLeft = Regex.Match(shortcut, @"ArrowLeft");
        if (matchArrowLeft.Success)
        {
            return VirtualKeyCode.ArrowLeft;
        }

        var matchArrowRicht = Regex.Match(shortcut, @"ArrowRigh");
        if (matchArrowRicht.Success)
        {
            return VirtualKeyCode.ArrowRight;
        }

        return VirtualKeyCode.None;
    }
}