using ModConfig.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace ModConfig.UI
{
    public class UICreator
    {
        public static void CreateMainTopBar(GameObject modsPage, out GameObject topBarContent)
        {
            var topBarObj = new GameObject("MainTopBar", typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            topBarObj.transform.SetParent(modsPage.transform, false);

            var rect = topBarObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0.92f);
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;

            topBarObj.GetComponent<Image>().color = new Color(0, 0, 0, 0f);

            GameObject viewport = new GameObject("Viewport", typeof(RectTransform), typeof(RectMask2D));
            viewport.transform.SetParent(topBarObj.transform, false);
            var viewRect = viewport.GetComponent<RectTransform>();
            viewRect.anchorMin = Vector2.zero; viewRect.anchorMax = Vector2.one;
            viewRect.sizeDelta = Vector2.zero;

            topBarContent = new GameObject("Content", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(ContentSizeFitter));
            topBarContent.transform.SetParent(viewport.transform, false);

            var contentRect = topBarContent.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 0);
            contentRect.anchorMax = new Vector2(0, 1);
            contentRect.pivot = new Vector2(0, 0.5f);

            var layout = topBarContent.GetComponent<HorizontalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childForceExpandWidth = false;
            layout.spacing = 20f;
            layout.padding = new RectOffset(30, 30, 5, 5);

            topBarContent.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            topBarContent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var scroll = topBarObj.GetComponent<ScrollRect>();
            scroll.content = contentRect;
            scroll.viewport = viewRect;
            scroll.horizontal = true;
            scroll.vertical = false;
        }

        public static void CreateModPage(GameObject modsPage, string modName, out GameObject page)
        {
            page = new GameObject(
                $"PAGE_{modName}",
                typeof(RectTransform),
                typeof(Image),
                typeof(ScrollRect)
            );

            page.transform.SetParent(modsPage.transform, false);

            var pageRect = page.GetComponent<RectTransform>();
            pageRect.anchorMin = Vector2.zero;
            pageRect.anchorMax = new Vector2(1, 0.92f);
            pageRect.offsetMin = Vector2.zero;
            pageRect.offsetMax = Vector2.zero;

            page.GetComponent<Image>().color = Color.clear;

            GameObject viewport = new GameObject(
                "Viewport",
                typeof(RectTransform),
                typeof(RectMask2D)
            );

            viewport.transform.SetParent(page.transform, false);

            var viewportRect = viewport.GetComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;

            GameObject content = new GameObject(
                "Content",
                typeof(RectTransform),
                typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter)
            );

            content.transform.SetParent(viewport.transform, false);

            var contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = Vector2.zero;

            var layout = content.GetComponent<VerticalLayoutGroup>();
            layout.spacing = 10f;
            layout.padding = new RectOffset(190, 190, 20, 20);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            content.GetComponent<ContentSizeFitter>().verticalFit =
                ContentSizeFitter.FitMode.PreferredSize;

            GameObject scrollbarObj = new GameObject(
                "Scrollbar",
                typeof(RectTransform),
                typeof(Image),
                typeof(Scrollbar)
            );

            scrollbarObj.transform.SetParent(page.transform, false);

            var scrollbarRect = scrollbarObj.GetComponent<RectTransform>();
            scrollbarRect.anchorMin = new Vector2(1, 0);
            scrollbarRect.anchorMax = new Vector2(1, 1);
            scrollbarRect.pivot = new Vector2(0.5f, 0.5f);
            scrollbarRect.sizeDelta = new Vector2(12, -40);
            scrollbarRect.anchoredPosition = new Vector2(-15, 4);

            var scrollbarImage = scrollbarObj.GetComponent<Image>();
            scrollbarImage.color = new Color(1f, 1f, 1f, 0f);

            GameObject track = new GameObject("Track", typeof(RectTransform), typeof(Image));
            track.transform.SetParent(scrollbarObj.transform, false);

            var trackRect = track.GetComponent<RectTransform>();
            trackRect.anchorMin = new Vector2(0.5f, 0);
            trackRect.anchorMax = new Vector2(0.5f, 1);
            trackRect.pivot = new Vector2(0.5f, 0.5f);
            trackRect.sizeDelta = new Vector2(12, -25);

            var trackImage = track.GetComponent<Image>();
            trackImage.color = new Color(0.87f, 0.89f, 0.84f, 1f);

            GameObject handle = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            handle.transform.SetParent(scrollbarObj.transform, false);

            var handleRect = handle.GetComponent<RectTransform>();
            handleRect.anchorMin = new Vector2(0.5f, 0);
            handleRect.anchorMax = new Vector2(0.5f, 1);
            handleRect.pivot = new Vector2(0.5f, 0.5f);
            handleRect.sizeDelta = new Vector2(12, 0);

            var handleImage = handle.GetComponent<Image>();
            handleImage.sprite =
                UICloner.slider
                    .transform
                    .Find("Option Contents/Slider/Handle Slide Area/Handle/Graphic")
                    .GetComponent<Image>()
                    .sprite;
            handleImage.color = Color.white;
            handleImage.type = Image.Type.Sliced;

            var scrollbar = scrollbarObj.GetComponent<Scrollbar>();
            scrollbar.handleRect = handleRect;
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            scrollbar.targetGraphic = handleImage;
            scrollbar.size = 0.1f;

            var scroll = page.GetComponent<ScrollRect>();
            scroll.content = contentRect;
            scroll.viewport = viewportRect;
            scroll.verticalScrollbar = scrollbar;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 30f;
            scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

            page.SetActive(false);
        }

        public static void CreateNewSection(GameObject modPage, string name, out GameObject container)
        {
            Transform content = modPage.transform.Find("Viewport/Content");
            if (content == null)
            {
                if (modPage.name == "Content") { content = modPage.transform; }
            }
            
            container = GameObject.Instantiate(UICloner.container, content);
            container.name = $"CONTAINER_{name}";
        }

        private static void HideAllOtherModPages(GameObject modsPage, GameObject targetPage)
        {
            foreach (Transform child in modsPage.transform)
            {
                if (child.name == "MainTopBar") continue;
                if (child == targetPage.transform)
                {
                    child.gameObject.SetActive(true);
                    continue;
                }
                child.gameObject.SetActive(false);
            }
        }

        public static void CreateModPageNavigationButton(GameObject topBarContent, GameObject modsPage, GameObject modPage, string name)
        {
            CreateNewSection(topBarContent, name, out GameObject container);

            CreateButton(container, 
                name, 
                () => { HideAllOtherModPages(modsPage, modPage); }
            );
        }

        public static void CreateTitle(GameObject modPage, string name)
        {
            Transform content = modPage.transform.Find("Viewport/Content");
            var newTitle = GameObject.Instantiate(UICloner.titleTMP, content);
            newTitle.name = $"TITLE_{name}";
            newTitle.GetComponent<TextMeshProUGUI>().text = name;
        }

        public static void CreateSubHeading(GameObject modPage, string name, out GameObject titleObj)
        {
            Transform content = modPage.transform.Find("Viewport/Content");
            titleObj = GameObject.Instantiate(UICloner.titleTMP, content);
            titleObj.name = $"SUBHEADING_{name}";

            var component = titleObj.GetComponent<TextMeshProUGUI>();
            component.text = name;
            component.fontSize -= 4;
            component.fontSizeMin -= 4;
            component.fontSizeMax -= 4;
        }

        public static void CreateDropdown(GameObject container, string configName, List<string> options, out DropdownOption dropdownOption)
        {
            var newDropdown = GameObject.Instantiate(UICloner.dropdown, container.transform);
            newDropdown.name = $"DROPDOWN_{configName}";
            newDropdown.transform.Find("Label Text").GetComponent<TextMeshProUGUI>().text = configName;

            dropdownOption = newDropdown.GetComponent<DropdownOption>();
            dropdownOption.localized = null;
            dropdownOption.SetOptions(options);

            Plugin.Log.LogInfo($"Created dropdown for config: {configName} in section: {container.name} with options: {string.Join(", ", options)})");
        }

        public static void CreateFloatSlider(GameObject container, float value, float minValue, float maxValue, string configName, out SliderOption sliderOption)
        {
            var newSlider = GameObject.Instantiate(UICloner.slider, container.transform);
            newSlider.name = $"SLIDER_{configName}";

            var text = newSlider.transform.Find("Label Text");
            var textComp = text.GetComponent<TextMeshProUGUI>();
            textComp.text = configName;

            var labelValueText = newSlider.transform.Find("Option Contents/Slider Value");
            var valueTextComponent = labelValueText.GetComponent<TextMeshProUGUI>();
            var valueRectTransform = labelValueText.GetComponent<RectTransform>();
            valueTextComponent.fontSize -= 6;
            valueTextComponent.fontSizeMin -= 6;
            valueTextComponent.fontSizeMax -= 6;

            sliderOption = newSlider.GetComponent<SliderOption>();
            sliderOption.SetLimits(minValue, maxValue);
            sliderOption.SetValue(value);

            string _formatValue = value % 1 == 0 ? value.ToString("F0") : value.ToString("0.###");

            sliderOption.SetValueText($"{value}");

            var localSliderOption = sliderOption;

            var inputTextView = new GameObject("Input Text View");
            inputTextView.transform.SetParent(labelValueText.parent, false);

            var inputRectTransform = inputTextView.AddComponent<RectTransform>();
            inputRectTransform.anchorMin = valueRectTransform.anchorMin;
            inputRectTransform.anchorMax = valueRectTransform.anchorMax;
            inputRectTransform.pivot = valueRectTransform.pivot;
            inputRectTransform.offsetMin = valueRectTransform.offsetMin;
            inputRectTransform.offsetMax = valueRectTransform.offsetMax;
            inputRectTransform.sizeDelta = valueRectTransform.sizeDelta;
            inputRectTransform.anchoredPosition = valueRectTransform.anchoredPosition;

            inputTextView.transform.SetSiblingIndex(labelValueText.GetSiblingIndex() + 1);

            var inputTextComponent = inputTextView.AddComponent<TextMeshProUGUI>();
            inputTextComponent.font = valueTextComponent.font;
            inputTextComponent.fontSize = valueTextComponent.fontSize;
            inputTextComponent.fontStyle = valueTextComponent.fontStyle;
            inputTextComponent.alignment = valueTextComponent.alignment;
            inputTextComponent.color = valueTextComponent.color;
            inputTextComponent.text = valueTextComponent.text;
            inputTextComponent.raycastTarget = true;

            var inputField = inputTextView.AddComponent<TMP_InputField>();
            inputField.textComponent = inputTextComponent;
            inputField.textViewport = labelValueText.parent.GetComponent<RectTransform>();
            inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
            inputField.lineType = TMP_InputField.LineType.SingleLine;
            inputField.interactable = true;

            inputField.targetGraphic = inputTextComponent;

            inputTextView.SetActive(false);

            var trigger = labelValueText.gameObject.AddComponent<EventTrigger>();
            var clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((data) => {
                labelValueText.gameObject.SetActive(false);
                inputField.text = localSliderOption.value.ToString();
                inputTextView.SetActive(true);
                inputField.ActivateInputField();
                inputField.Select();
            });
            trigger.triggers.Add(clickEntry);

            inputField.onEndEdit.AddListener((newValue) => {
                if (float.TryParse(newValue, out float result))
                {
                    result = Mathf.Clamp(result, minValue, maxValue);
                    localSliderOption.SetValue(result);
                    string formatValue = result % 1 == 0 ? result.ToString("F0") : result.ToString("0.###");
                    localSliderOption.SetValueText(formatValue);
                    valueTextComponent.text = formatValue;
                    inputTextComponent.text = formatValue;
                    localSliderOption.onChanged?.Invoke();
                }

                inputTextView.SetActive(false);
                labelValueText.gameObject.SetActive(true);
            });
        }

        public static void CreateIntSlider(GameObject container, int value, int minValue, int maxValue, string configName, out SliderOption sliderOption)
        {
            var newSlider = GameObject.Instantiate(UICloner.slider, container.transform);
            newSlider.name = $"SLIDER_{configName}";

            var text = newSlider.transform.Find("Label Text");
            var textComp = text.GetComponent<TextMeshProUGUI>();
            textComp.text = configName;

            var labelValueText = newSlider.transform.Find("Option Contents/Slider Value");
            var valueTextComponent = labelValueText.GetComponent<TextMeshProUGUI>();
            var valueRectTransform = labelValueText.GetComponent<RectTransform>();
            valueTextComponent.fontSize -= 6;
            valueTextComponent.fontSizeMin -= 6;
            valueTextComponent.fontSizeMax -= 6;

            sliderOption = newSlider.GetComponent<SliderOption>();
            sliderOption.slider.wholeNumbers = true;
            sliderOption.SetLimits(minValue, maxValue);
            sliderOption.SetValue(value);
            sliderOption.SetValueText(value.ToString("F0"));

            var localSliderOption = sliderOption;

            var inputTextView = new GameObject("Input Text View");
            inputTextView.transform.SetParent(labelValueText.parent, false);

            var inputRectTransform = inputTextView.AddComponent<RectTransform>();
            inputRectTransform.anchorMin = valueRectTransform.anchorMin;
            inputRectTransform.anchorMax = valueRectTransform.anchorMax;
            inputRectTransform.pivot = valueRectTransform.pivot;
            inputRectTransform.offsetMin = valueRectTransform.offsetMin;
            inputRectTransform.offsetMax = valueRectTransform.offsetMax;
            inputRectTransform.sizeDelta = valueRectTransform.sizeDelta;
            inputRectTransform.anchoredPosition = valueRectTransform.anchoredPosition;

            inputTextView.transform.SetSiblingIndex(labelValueText.GetSiblingIndex() + 1);

            var inputTextComponent = inputTextView.AddComponent<TextMeshProUGUI>();
            inputTextComponent.font = valueTextComponent.font;
            inputTextComponent.fontSize = valueTextComponent.fontSize;
            inputTextComponent.fontStyle = valueTextComponent.fontStyle;
            inputTextComponent.alignment = valueTextComponent.alignment;
            inputTextComponent.color = valueTextComponent.color;
            inputTextComponent.text = valueTextComponent.text;
            inputTextComponent.raycastTarget = true;

            var inputField = inputTextView.AddComponent<TMP_InputField>();
            inputField.textComponent = inputTextComponent;
            inputField.textViewport = labelValueText.parent.GetComponent<RectTransform>();
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputField.lineType = TMP_InputField.LineType.SingleLine;
            inputField.interactable = true;
            inputField.characterValidation = TMP_InputField.CharacterValidation.Integer;

            inputField.targetGraphic = inputTextComponent;

            inputTextView.SetActive(false);

            var trigger = labelValueText.gameObject.AddComponent<EventTrigger>();
            var clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((data) => {
                labelValueText.gameObject.SetActive(false);
                inputField.text = localSliderOption.value.ToString();
                inputTextView.SetActive(true);
                inputField.ActivateInputField();
                inputField.Select();
            });
            trigger.triggers.Add(clickEntry);

            inputField.onEndEdit.AddListener((newValue) => {
                if (int.TryParse(newValue, out int result))
                {
                    result = Mathf.Clamp(result, minValue, maxValue);
                    localSliderOption.SetValue(result);
                    string formatValue = result.ToString();
                    localSliderOption.SetValueText(formatValue);
                    valueTextComponent.text = formatValue;
                    inputTextComponent.text = formatValue;
                    localSliderOption.onChanged?.Invoke();
                }
                else
                {
                    inputTextComponent.text = valueTextComponent.text;
                }

                inputTextView.SetActive(false);
                labelValueText.gameObject.SetActive(true);
            });
        }

        public static void CreateStringInput(GameObject container, string name, Action<string> onChange, string defaultValue = "")
        {
            var stringInput = GameObject.Instantiate(UICloner.slider, container.transform);
            stringInput.name = $"INPUT_{name}";
            stringInput.transform.Find("Label Text").GetComponent<TextMeshProUGUI>().text = name;

            var contents = stringInput.transform.Find("Option Contents");
            var contentsRect = contents.GetComponent<RectTransform>();

            if (contents.GetComponent<RectMask2D>() == null) contents.gameObject.AddComponent<RectMask2D>();

            GameObject.DestroyImmediate(stringInput.GetComponent<SliderOption>());
            var sliderObj = contents.Find("Slider");
            if (sliderObj != null) GameObject.DestroyImmediate(sliderObj.gameObject);
            var caretObj = contents.Find("Caret");
            if (caretObj != null) GameObject.DestroyImmediate(caretObj.gameObject);

            var displayTextObj = contents.Find("Slider Value").gameObject;
            displayTextObj.name = "Display Text";
            var displayTextComponent = displayTextObj.GetComponent<TextMeshProUGUI>();
            var displayRectTransform = displayTextObj.GetComponent<RectTransform>();

            displayRectTransform.anchorMin = new Vector2(0, 0);
            displayRectTransform.anchorMax = new Vector2(1, 1);
            displayRectTransform.offsetMin = new Vector2(10, 0);
            displayRectTransform.offsetMax = new Vector2(-10, 0);

            displayTextComponent.textWrappingMode = TextWrappingModes.NoWrap;
            displayTextComponent.text = defaultValue;
            displayTextComponent.alignment = TextAlignmentOptions.Left;
            displayTextComponent.overflowMode = TextOverflowModes.Masking;

            var inputTextView = new GameObject("Input Text View");
            inputTextView.transform.SetParent(contents, false);

            var inputRectTransform = inputTextView.AddComponent<RectTransform>();
            inputRectTransform.anchorMin = displayRectTransform.anchorMin;
            inputRectTransform.anchorMax = displayRectTransform.anchorMax;
            inputRectTransform.offsetMin = displayRectTransform.offsetMin;
            inputRectTransform.offsetMax = displayRectTransform.offsetMax;

            var inputTextComponent = inputTextView.AddComponent<TextMeshProUGUI>();
            inputTextComponent.font = displayTextComponent.font;
            inputTextComponent.fontSize = displayTextComponent.fontSize;
            inputTextComponent.alignment = TextAlignmentOptions.Left;
            inputTextComponent.color = displayTextComponent.color;
            inputTextComponent.raycastTarget = true;

            inputTextComponent.textWrappingMode = TextWrappingModes.NoWrap;

            var inputField = inputTextView.AddComponent<TMP_InputField>();
            inputField.textViewport = contentsRect;
            inputField.textComponent = inputTextComponent;
            inputField.contentType = TMP_InputField.ContentType.Standard;
            inputField.lineType = TMP_InputField.LineType.SingleLine;

            inputTextView.SetActive(false);

            var trigger = displayTextObj.AddComponent<EventTrigger>();
            var clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            clickEntry.callback.AddListener((data) => {
                displayTextObj.SetActive(false);
                inputTextView.SetActive(true);
                inputField.text = displayTextComponent.text;
                inputField.ActivateInputField();
            });
            trigger.triggers.Add(clickEntry);

            inputField.onEndEdit.AddListener((newValue) => {
                displayTextComponent.text = newValue;
                onChange?.Invoke(newValue);
                inputTextView.SetActive(false);
                displayTextObj.SetActive(true);
            });
        }

        public static void CreateButton(GameObject container, string name, Action action)
        {
            var buttonObj = GameObject.Instantiate(UICloner.button, container.transform);
            buttonObj.name = $"BUTTON_{name}";
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = name;

            buttonObj.GetComponent<Button>().onClick.RemoveAllListeners();
            buttonObj.GetComponent<Button>().onClick.AddListener(() => action.Invoke());

            buttonObj.GetComponent<Transform>().localPosition = Vector3.zero;
        }

        private static string KeyCodeToPath(KeyCode key)
        {
            return $"<Keyboard>/{key.ToString().ToLower()}";
        }

        public static void CreateKeybinding(GameObject container, string configName, KeyCode defaultValue, out ActionRebindOption actionRebindOption, out KeybindHook actionHook)
        {
            var map = new InputActionMap("ModConfigMap");
            var action = map.AddAction(configName);
            int kbdIndex = action.AddBinding("<Keyboard>/none").binding.GetHashCode();
            kbdIndex = 0;

            action.ApplyBindingOverride(kbdIndex, KeyCodeToPath(defaultValue));
            action.Enable();

            var newKeybind = GameObject.Instantiate(UICloner.dropdown, container.transform);
            newKeybind.name = $"MC_KEYBIND_{configName}";
            newKeybind.transform.Find("Label Text").GetComponent<TextMeshProUGUI>().text = configName;

            Transform optionsContent = newKeybind.transform.Find("Option Contents");
            if (optionsContent.Find("Dropdown"))
                GameObject.DestroyImmediate(optionsContent.Find("Dropdown").gameObject);

            var layout = optionsContent.gameObject.AddOrGetComponent<HorizontalLayoutGroup>();
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = false;
            layout.spacing = 10;
            layout.childAlignment = TextAnchor.MiddleRight;

            var kbdObj = new GameObject("KbdButton", typeof(RectTransform), typeof(Image), typeof(Button));
            kbdObj.transform.SetParent(optionsContent, false);

            float squareSize = 40f;
            var kbdRect = kbdObj.GetComponent<RectTransform>();
            kbdRect.sizeDelta = new Vector2(squareSize, squareSize);

            var kbdImage = kbdObj.GetComponent<Image>();
            kbdImage.preserveAspect = true;

            var textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObj.transform.SetParent(kbdObj.transform, false);
            var tmp = textObj.GetComponent<TextMeshProUGUI>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 18;
            tmp.color = Color.white;
            tmp.raycastTarget = false;

            var textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var resetObj = new GameObject("ResetButton", typeof(RectTransform), typeof(Button));
            resetObj.transform.SetParent(optionsContent, false);
            resetObj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

            actionRebindOption = newKeybind.AddComponent<ActionRebindOption>();

            actionRebindOption.keyboardRebindButton = new ActionRebindOption.RebindButton
            {
                parentObject = kbdObj,
                button = kbdObj.GetComponent<Button>(),
                bindingIcon = kbdImage,
                tmpBinding = tmp,
                resetButton = resetObj.GetComponent<Button>()
            };

            GameObject dummy = new GameObject("Dummy_Gamepad");
            dummy.transform.SetParent(newKeybind.transform);
            dummy.SetActive(false);
            actionRebindOption.gamepadRebindButton = new ActionRebindOption.RebindButton
            {
                parentObject = dummy,
                button = dummy.AddComponent<Button>(),
                resetButton = dummy.AddComponent<Button>(),
                bindingIcon = dummy.AddComponent<Image>(),
                tmpBinding = dummy.AddComponent<TextMeshProUGUI>()
            };

            var rebindManager = ModConfigMain.controlsPage.GetComponent<ControlsRebind>();

            GameObject hookHost = (rebindManager != null) ? rebindManager.gameObject : container;
            actionHook = newKeybind.AddComponent<KeybindHook>();

            if (defaultValue == KeyCode.None)
            {
                kbdImage.sprite = getEmptyKeyIcon();
                kbdImage.enabled = true;
            }

            try
            {
                actionRebindOption.Initialize(rebindManager, action, kbdIndex, -1);
            }
            catch (Exception ex)
            {
                // I always error due to localization, it doesnt actually break anything so catch the error and discard.
            }
        }

        public static Sprite getEmptyKeyIcon()
        {
            if (UICloner.emptyKeyIcon != null) return UICloner.emptyKeyIcon;

            var resource = "ModConfig.UI.empty-key.png";
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    Plugin.Log.LogError($"Resource not found: {resource}");
                    return null;
                }

                byte[] ba = new byte[stream.Length];
                stream.Read(ba, 0, ba.Length);

                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (texture.LoadImage(ba))
                {
                    Sprite sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100f
                    );

                    UICloner.emptyKeyIcon = sprite;
                    return sprite;
                }
            }

            return null;
        }
    }
}
