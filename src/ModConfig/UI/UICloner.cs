using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

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

        public static void GetUIRefs(Transform graphicsPage, Transform audioPage)
        {
            if (graphicsPage == null) return;

            titleTMP = GameObject.Instantiate(graphicsPage.Find("Title").gameObject, null);
            titleTMP.name = "MODCONFIG_TITLE_TEMPLATE";
            GameObject.DestroyImmediate(titleTMP.GetComponentInChildren<LocalizeStringEvent>());

            var _container = GameObject.Instantiate(graphicsPage.Find("Option Container"));
            _container.name = "MODCONFIG_CONTAINER_TEMPLATE";

            dropdown = GameObject.Instantiate(_container.Find("Resolution").gameObject, null);
            dropdown.name = "MODCONFIG_DROPDOWN_TEMPLATE";
            GameObject.DestroyImmediate(dropdown.GetComponentInChildren<LocalizeDropdown>());
            GameObject.DestroyImmediate(dropdown.GetComponentInChildren<LocalizeStringEvent>());
            dropdown.GetComponent<DropdownOption>().localized = null;

            button = GameObject.Instantiate(graphicsPage.parent.Find("General/Tutorial").GetChild(0).gameObject, null);
            button.name = "MODCONFIG_BUTTON_TEMPLATE";
            GameObject.DestroyImmediate(button.GetComponentInChildren<LocalizeStringEvent>());

            slider = GameObject.Instantiate(audioPage.Find("Volume Options/Master Volume").gameObject);
            slider.name = "MODCONFIG_SLIDER_TEMPLATE";
            GameObject.DestroyImmediate(slider.GetComponentInChildren<LocalizeStringEvent>());

            while (_container.transform.childCount > 0) UnityEngine.Object.DestroyImmediate(_container.transform.GetChild(0).gameObject);
            container = _container.gameObject;
        }
    }
}
