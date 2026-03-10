using BepInEx.Bootstrap;
using BepInEx.Configuration;
using ModConfig.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ModConfig.Core
{
    public static class ModConfigManager
    {
        private const string TAB_NAME = "Mods";
        private static readonly Color BG_COLOR = new Color(1f, 0.8024f, 0.8584f, 1f);
        private static readonly float SPACING = -45f;
        private static readonly Vector3 BUTTON_SCALE = new Vector3(0.8f, 0.8f, 0.8f);
        private static readonly float PADDING_X = 190f;

        public static Dictionary<string, List<ConfigEntryBase>> LoadAllModConfigs()
        {
            var modConfigs = new Dictionary<string, List<ConfigEntryBase>>();

            foreach (var plugin in Chainloader.PluginInfos.Values.OrderBy(p => p.Metadata.Name))
            {
                if (plugin.Instance == null || plugin.Instance.Config == null) continue;

                var entries = plugin.Instance.Config
                    .Where(e => !e.Value.Description.Tags.Contains("Hidden"))
                    .Select(e => e.Value)
                    .ToList();

                if (entries.Count > 0)
                {
                    modConfigs[plugin.Metadata.Name] = entries;
                    Debug.Log($"Found {entries.Count} config entries for mod: {plugin.Metadata.Name}");
                }
            }

            return modConfigs;
        }

        public static void Inject(MenuTabs menuTabs)
        {
            if (menuTabs == null || menuTabs.transform.Find("Menu/Options Tab/Mods") != null) return;

            Transform optionsTab = menuTabs.transform.Find("Menu/Options Tab");
            Transform controlsButton = optionsTab.Find("Controls");
            GameObject controlsPage = menuTabs.tabs.FirstOrDefault(t => t.name.Contains("Controls"));

            if (controlsButton == null || controlsPage == null) return;

            optionsTab.GetComponent<HorizontalLayoutGroup>().spacing = SPACING;
            ChangeButtons(menuTabs);

            GameObject modsButtonObj = Object.Instantiate(controlsButton.gameObject, optionsTab);
            modsButtonObj.name = "Mods";
            modsButtonObj.GetComponent<Image>().color = BG_COLOR;
            modsButtonObj.transform.Find("Highlight/Background").GetComponent<Image>().color = BG_COLOR;
            modsButtonObj.transform.Find("Highlight/Background/Stripes").GetComponent<Image>().color = BG_COLOR - new Color(0.05f, 0.05f, 0.05f, 0f);
            SetupButtonText(modsButtonObj);

            GameObject modsPage = Object.Instantiate(controlsPage, controlsPage.transform.parent);
            modsPage.name = "Mods";
            
            while (modsPage.transform.childCount > 0) Object.DestroyImmediate(modsPage.transform.GetChild(0).gameObject);

            CleanContentPage(modsPage);
            var modConfig = modsPage.transform.parent.gameObject.AddComponent<ModConfigMain>();
            UpdateMenuTabs(menuTabs, modsButtonObj, modsPage);
        }

        private static void UpdateMenuTabs(MenuTabs menuTabs, GameObject buttonObj, GameObject page)
        {
            var buttons = menuTabs.tabButtons.ToList();
            var tabs = menuTabs.tabs.ToList();
            buttons.Add(buttonObj.GetComponent<Button>());
            tabs.Add(page);

            menuTabs.tabButtons = buttons.ToArray();
            menuTabs.tabs = tabs.ToArray();

            menuTabs.textColors = menuTabs.textColors.Append(menuTabs.textColors.Last()).ToArray();

            int index = menuTabs.tabButtons.Length - 1;
            buttonObj.GetComponent<Button>().onClick.RemoveAllListeners();
            buttonObj.GetComponent<Button>().onClick.AddListener(() => menuTabs.SelectTab(index));
        }

        private static void SetupButtonText(GameObject button)
        {
            Object.DestroyImmediate(button.GetComponentInChildren<UnityEngine.Localization.Components.LocalizeStringEvent>());
            foreach (var text in button.GetComponentsInChildren<TMP_Text>()) text.text = TAB_NAME;
        }

        private static void CleanContentPage(GameObject page)
        {
            var valid = new[] { "RectTransform", "CanvasRenderer", "Canvas", "Image", "ScrollRect", "Mask" };
            foreach (var comp in page.GetComponents<Component>())
                if (comp is MonoBehaviour && !valid.Contains(comp.GetType().Name)) Object.DestroyImmediate(comp);
        }

        private static void ChangeButtons(MenuTabs menu)
        {
            var buttons = menu.tabButtons.ToList();

            foreach (var button in buttons)
            {
                button.transform.localScale = BUTTON_SCALE;
            }
        }
    }
}