using ModConfig.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace ModConfig.UI
{
    public class ModConfigMain : MonoBehaviour
    {
        private static ModConfigMain Instance = null!;

        private static Transform graphicsPage = null!;
        private static Transform audioPage = null!;
        private static Transform modsPage = null!;
        private static Transform modsContent = null!;

        public static Transform controlsPage = null!;

        private void Awake()
        {
            Instance = this;

            graphicsPage = transform.Find("Graphics");
            audioPage = transform.Find("Audio");
            controlsPage = transform.Find("Controls");
            modsPage = transform.Find("Mods");
            modsContent = modsPage.Find("ModContent");
            UICloner.GetUIRefs(graphicsPage, audioPage);

            if (Plugin.IsBepInExModInstalled("com.sinai.unityexplorer") && !Plugin.shownErrorThisSession)
            {
                Plugin.Log.LogWarning("Unity explorer is a know incompatibility!");
            }

            UICreator.CreateMainTopBar(modsPage.gameObject, out GameObject topBar);
            ConfigGetter.InitialiseConfigs(modsPage.gameObject, topBar);
        }
    }
}
