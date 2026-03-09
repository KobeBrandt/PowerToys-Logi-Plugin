namespace Loupedeck.PowerToysPlugin.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

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

        public static char GetChar(String shortcut)
        {
            Match matchKey = Regex.Match(shortcut, @"Key([A-Z`])");
            if (matchKey.Success)
            {
                char key = matchKey.Groups[1].Value[0];
                return key.ToLower();
                
            }
            Match matchSpace = Regex.Match(shortcut, @"Space");
            if (matchSpace.Success)
            {
                return ' ';
            } 
            Match matchSlash = Regex.Match(shortcut, @"Key/");
            if (matchSlash.Success)
            {
                return '/';
            } 
            return '⍼';
        }

        public static VirtualKeyCode GetVirtualKeyCode(String shortcut)
        {
            Match matchArrowLeft = Regex.Match(shortcut, @"ArrowLeft");
            if (matchArrowLeft.Success)
            {
                return VirtualKeyCode.ArrowLeft;
            } 
            Match matchArrowRicht = Regex.Match(shortcut, @"ArrowRigh");
            if (matchArrowRicht.Success)
            {
                return VirtualKeyCode.ArrowRight;
            } 
            return VirtualKeyCode.None;
        }
    }
}
