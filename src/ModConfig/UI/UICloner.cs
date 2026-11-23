using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace ModConfig.UI
{
    public class UICloner
    {
        public static GameObject titleTMP;
        public static GameObject container;
        public static GameObject dropdown;
        public static GameObject slider;
        public static GameObject button;

        public static Sprite emptyKeyIcon;

        public static TMP_FontAsset baseFont;

        public static void GetUIRefs(Transform graphicsPage, Transform audioPage)
        {
            if (graphicsPage == null) return;

            titleTMP = GameObject.Instantiate(graphicsPage.Find("Title").gameObject, null);
            titleTMP.name = "MODCONFIG_TITLE_TEMPLATE";
            GameObject.DestroyImmediate(titleTMP.GetComponentInChildren<LocalizeStringEvent>());
            baseFont = titleTMP.GetComponent<TextMeshProUGUI>().font;

            var _container = GameObject.Instantiate(graphicsPage.Find("Option Container"));
            _container.name = "MODCONFIG_CONTAINER_TEMPLATE";

            dropdown = GameObject.Instantiate(_container.Find("Resolution").gameObject, null);
            dropdown.name = "MODCONFIG_DROPDOWN_TEMPLATE";
            GameObject.DestroyImmediate(dropdown.GetComponentInChildren<LocalizeDropdown>());
            GameObject.DestroyImmediate(dropdown.GetComponentInChildren<LocalizeStringEvent>());
            dropdown.GetComponent<DropdownOption>().localized = null;

            GameObject buttonSource = FindButtonTemplate(graphicsPage);
            if (buttonSource != null)
            {
                button = GameObject.Instantiate(buttonSource, null);
                button.name = "MODCONFIG_BUTTON_TEMPLATE";
                GameObject.DestroyImmediate(button.GetComponentInChildren<LocalizeStringEvent>());
            }

            slider = GameObject.Instantiate(audioPage.Find("Volume Options/Master Volume").gameObject);
            slider.name = "MODCONFIG_SLIDER_TEMPLATE";
            GameObject.DestroyImmediate(slider.GetComponentInChildren<LocalizeStringEvent>());

            while (_container.transform.childCount > 0) UnityEngine.Object.DestroyImmediate(_container.transform.GetChild(0).gameObject);
            container = _container.gameObject;
        }

        private static GameObject FindButtonTemplate(Transform graphicsPage)
        {
            Transform generalPage = graphicsPage.parent.Find("General");
            if (generalPage == null) return null;

            // v3 path: General/Tutorial/<button>
            Transform tutorial = generalPage.Find("Tutorial");
            if (tutorial != null && tutorial.childCount > 0)
                return tutorial.GetChild(0).gameObject;

            // v4+: find any Button component in the General page hierarchy
            var btn = generalPage.GetComponentInChildren<Button>(true);
            if (btn != null) return btn.gameObject;

            return null;
        }
    }
}
