﻿using HarmonyLib;
using JumpKing;
using JumpKing.API;
using JumpKing.Mods;
using JumpKing.PauseMenu.BT;
using JumpKing.XnaWrappers;
using JumpKingSaveStates.Models;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JumpKing.PauseMenu;

namespace JumpKingSaveStates
{
    [JumpKingMod(IDENTIFIER)]
    public class JumpKingSaveStates
    {
        const string IDENTIFIER = "Phoenixx19.SaveStates";
        const string HARMONY_IDENTIFIER = "Phoenixx19.SaveStates.Harmony";
        const string SETTINGS_FILE = "Phoenixx19.SaveStates.Settings.xml";

        private static string AssemblyPath { get; set; }
        public static Preferences Preferences { get; private set; }
        public static CustomPadInstance PadInstance { get; private set; }

        internal static JKSound DeleteSave { get; private set; }

        #region Menu Items
        [PauseMenuItemSetting]
        [MainMenuItemSetting]
        public static TextButton BindSettings(object factory, GuiFormat format)
        {
            return new TextButton("Bind Keys", MenuOptions.CreateSaveStatesBindControls(factory));
        }

        [PauseMenuItemSetting]
        public static TextButton TeleportMenu(object factory, GuiFormat format)
        {
            return new TextButton("Teleport to...", MenuOptions.CreateTeleportList(format));
        }
        #endregion

        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
#if DEBUG
            Debugger.Launch();
            Harmony.DEBUG = true;
#endif

            // set path for dll
            AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // try reading config file
            try
            {
                var prefs = XmlSerializerHelper.Deserialize<Preferences>($@"{AssemblyPath}\{SETTINGS_FILE}");
                if (prefs.KeyBindings.Count != Enum.GetNames(typeof(EBinding)).Length)
                {
                    throw new Exception("Missing keybindings!");
                }
                Preferences = prefs;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
                Preferences = new Preferences();
            }

            // add save on property changed
            Preferences.PropertyChanged += SaveSettingsOnFile;

            // setup harmony
            var harmony = new Harmony(HARMONY_IDENTIFIER);

            // patching on each class (is better than attributes)
            new GameLoopRun(harmony);
            PadInstance = new CustomPadInstance(harmony);
        }

        private static void SaveSettingsOnFile(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            try
            {
                XmlSerializerHelper.Serialize($@"{AssemblyPath}\{SETTINGS_FILE}", Preferences);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
            }
        }

        [OnLevelStart]
        public static void OnLevelStart()
        {
            // load savestate delete sfx
            DeleteSave = new JKSound(
                Game1.instance.contentManager.Load<SoundEffect>($@"{AssemblyPath}\Content\delete_savestate"),
                SoundType.SFX);
        }
    }
}
