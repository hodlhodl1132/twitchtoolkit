using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Windows;
using Verse;

namespace TwitchToolkit.Settings
{
    [StaticConstructorOnStartup]
    public static class Settings_ToolkitExtensions
    {
        static Settings_ToolkitExtensions()
        {
            GetExtensions = new List<ToolkitExtension>();
        }

        public static void RegisterExtension(ToolkitExtension extension)
        {
            GetExtensions.Add(extension);
        }

        public static List<ToolkitExtension> GetExtensions { get; private set; }
    }

    public class ToolkitExtension
    {
        public Mod mod;
        public Type windowType;

        public ToolkitExtension(Mod mod, Type windowType)
        {
            this.mod = mod;
            this.windowType = windowType;
        }
    }

    public abstract class ToolkitWindow : SettingsWindow
    {
        public ToolkitWindow(Mod mod) : base(mod)
        {

        }
    }
}
