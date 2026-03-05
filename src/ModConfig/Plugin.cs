using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using ModConfig.Core;
using ModConfig.UI;
using ModConfig.Utilities;
using System;
using UnityEngine;
using static FullScreenMessage;

namespace ModConfig
{
    [BepInAutoPlugin]
    public partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; } = null!;
        public static Harmony harmony = null!;

        public static bool shownErrorThisSession = false;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo($"Plugin {Name} is loaded!");

            harmony = new Harmony(Id);
            harmony.PatchAll();
        }

        public static bool IsBepInExModInstalled(string guid)
        {
            return Chainloader.PluginInfos.ContainsKey(guid);
        }
    }

    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MenuTabs), nameof(MenuTabs.Awake))]
        public static void HookIntoGUI(MenuTabs __instance)
        {
            ModConfigManager.Inject(__instance);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(FullScreenMessage), nameof(FullScreenMessage.ShowInternal))]
        public static void Keybind_Localization(ref string message, string header, bool isError, bool requiresMouse, params ButtonEntry[] buttonEntries)
        {
            if (message.Contains("UI/SETTINGS_Keybinds_ModConfigMap_"))
            {
                var name = message.Replace("UI/SETTINGS_Keybinds_ModConfigMap_", "");

                message = name;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActionRebindOption), nameof(ActionRebindOption.RefreshBindingDisplay), new Type[] { })]
        public static void KeybindFinish(ControlsRebind __instance)
        {
            var hook = __instance.GetComponent<KeybindHook>();
            if (hook == null) return;

            hook.onRebind?.Invoke();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InputIconsSet), nameof(InputIconsSet.GetIcon))]
        public static bool getIconFix(InputIconsSet __instance, string binding, ref Sprite __result)
        {
            if (binding == "<Keyboard>/none")
            {
                __result = UICreator.getEmptyKeyIcon();
                return false;
            }
            return true;
        }
    }
}