using BepInEx.Bootstrap;
using BepInEx.Configuration;
using DG.Tweening.Plugins.Core;
using ModConfig.UI;
using ModConfig.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using static PlayerCosmeticsMetadata;

namespace ModConfig.Core
{
    public class ConfigGetter
    {
        public static void InitialiseConfigs(GameObject modsPage, GameObject topBar)
        {
            bool firstPlugin = true;
            foreach (var plugin in Chainloader.PluginInfos)
            {
                string modName = plugin.Value.Metadata.Name;
                ConfigFile configFile = plugin.Value.Instance.Config;

                UICreator.CreateModPage(modsPage, modName, out GameObject page);
                UICreator.CreateModPageNavigationButton(topBar, modsPage, page, modName);

                if (firstPlugin) { page.SetActive(true); firstPlugin = false; }

                var groupedEntries = configFile.GroupBy(entry => entry.Value.Definition.Section);

                UICreator.CreateTitle(page, modName);

                foreach (var sectionGroup in groupedEntries)
                {
                    string categoryName = sectionGroup.Key;

                    UICreator.CreateSubHeading(page, categoryName, out GameObject titleObj);
                    UICreator.CreateNewSection(page, categoryName, out GameObject container);

                    foreach (var entry in sectionGroup)
                    {
                        ConfigEntryBase entryBase = entry.Value;
                        Type settingType = entryBase.SettingType;

                        if (settingType == typeof(bool))
                        {
                            UICreator.CreateDropdown(container, entryBase.Definition.Key, new List<string> { "True", "False" }, out DropdownOption dropdownOption);

                            bool currentVal = (bool)entryBase.BoxedValue;
                            dropdownOption.SetValue(currentVal ? 0 : 1);

                            dropdownOption.onChanged += () => {
                                entryBase.BoxedValue = (dropdownOption.value == 0);
                                Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {entryBase.BoxedValue}");
                            };
                        }
                        else if (settingType == typeof(float))
                        {
                            var range = entryBase.Description.AcceptableValues as AcceptableValueRange<float>;

                            if (range != null)
                            {
                                UICreator.CreateFloatSlider(
                                    container,
                                    (float)entryBase.BoxedValue,
                                    range.MinValue,
                                    range.MaxValue,
                                    entryBase.Definition.Key,
                                    out SliderOption sliderOption
                                );

                                sliderOption.onChanged += () => {
                                    entryBase.BoxedValue = sliderOption.value;
                                    sliderOption.SetValueText(entryBase.BoxedValue.ToString());
                                    Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {entryBase.BoxedValue}");
                                };
                            }
                            else
                            {
                                UICreator.CreateFloatSlider(
                                    container,
                                    (float)entryBase.BoxedValue,
                                    0,
                                    999,
                                    entryBase.Definition.Key,
                                    out SliderOption sliderOption
                                );

                                sliderOption.onChanged += () => {
                                    entryBase.BoxedValue = sliderOption.value;
                                    sliderOption.SetValueText(entryBase.BoxedValue.ToString());
                                    Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {entryBase.BoxedValue}");
                                };
                            }
                        }
                        else if (settingType == typeof(int))
                        {
                            var range = entryBase.Description.AcceptableValues as AcceptableValueRange<int>;

                            if (range != null)
                            {
                                UICreator.CreateIntSlider(
                                    container,
                                    (int)entryBase.BoxedValue,
                                    range.MinValue,
                                    range.MaxValue,
                                    entryBase.Definition.Key,
                                    out SliderOption sliderOption
                                );

                                sliderOption.onChanged += () => {
                                    entryBase.BoxedValue = sliderOption.value;
                                    sliderOption.SetValueText(entryBase.BoxedValue.ToString());
                                    Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {entryBase.BoxedValue}");
                                };
                            }
                            else
                            {
                                UICreator.CreateIntSlider(
                                    container,
                                    (int)entryBase.BoxedValue,
                                    0,
                                    999,
                                    entryBase.Definition.Key,
                                    out SliderOption sliderOption
                                );

                                sliderOption.onChanged += () => {
                                    entryBase.BoxedValue = sliderOption.value;
                                    sliderOption.SetValueText(entryBase.BoxedValue.ToString());
                                    Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {entryBase.BoxedValue}");
                                };
                            }
                        }
                        else if (settingType == typeof(string))
                        {
                            var listConstraint = entryBase.Description.AcceptableValues as AcceptableValueList<string>;

                            if (listConstraint != null)
                            {
                                List<string> options = listConstraint.AcceptableValues.ToList();

                                UICreator.CreateDropdown(container, entryBase.Definition.Key, options, out DropdownOption dropdownOption);

                                string currentVal = (string)entryBase.BoxedValue;
                                dropdownOption.SetValue(options.IndexOf(currentVal));

                                dropdownOption.onChanged += () => {
                                    entryBase.BoxedValue = options[dropdownOption.value];
                                    Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {entryBase.BoxedValue}");
                                };
                            }
                            else
                            {
                                UICreator.CreateStringInput(container, entryBase.Definition.Key, (newVal) => { entryBase.BoxedValue = newVal; Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {entryBase.BoxedValue}"); }, (string)entryBase.BoxedValue);
                            }
                        }
                        else if (settingType == typeof(Color))
                        {
                            Color currentColor = (Color)entryBase.BoxedValue;

                            string initialHex = ColorUtility.ToHtmlStringRGBA(currentColor);

                            UICreator.CreateStringInput(
                                container,
                                entryBase.Definition.Key,
                                (newText) => {
                                    if (ColorUtility.TryParseHtmlString(newText.StartsWith("#") ? newText : "#" + newText, out Color newCol))
                                    {
                                        entryBase.BoxedValue = newCol;
                                        Plugin.Log.LogInfo($"Config {entryBase.Definition.Key} updated to {newText}");
                                    }
                                },
                                initialHex
                            );
                        }
                        else if (settingType == typeof(KeyCode))
                        {
                            var configEntry = (ConfigEntry<KeyCode>)entryBase;

                            UICreator.CreateKeybinding(
                                container,
                                configEntry.Definition.Key,
                                configEntry.Value,
                                out ActionRebindOption actionRebindOption,
                                out KeybindHook actionHook
                            );

                            actionHook.onRebind += () => {
                                string newPath = actionRebindOption.action.bindings[actionRebindOption.keyboardBindingIndex].effectivePath;

                                KeyCode newKey = actionHook.PathToKeyCode(newPath);

                                configEntry.BoxedValue = newKey;

                                Plugin.Log.LogInfo($"Config {configEntry.Definition.Key} updated to {newKey}");
                            };
                        }
                        else if (settingType == typeof(KeyboardShortcut))
                        {
                            var configEntry = (ConfigEntry<KeyboardShortcut>)entryBase;

                            UICreator.CreateKeybinding(
                                container,
                                configEntry.Definition.Key,
                                configEntry.Value.MainKey,
                                out ActionRebindOption actionRebindOption,
                                out KeybindHook actionHook
                            );

                            actionHook.onRebind += () => {
                                string newPath = actionRebindOption.action.bindings[actionRebindOption.keyboardBindingIndex].effectivePath;
                                KeyCode newKey = actionHook.PathToKeyCode(newPath);

                                configEntry.Value = new KeyboardShortcut(newKey);

                                Plugin.Log.LogInfo($"Config {configEntry.Definition.Key} updated to {newKey}");
                            };
                        }
                        else if (settingType.IsEnum)
                        {
                            List<string> enumNames = Enum.GetNames(settingType).ToList();

                            UICreator.CreateDropdown(
                                container,
                                entryBase.Definition.Key,
                                enumNames,
                                out DropdownOption dropdownOption
                            );

                            string currentEnumName = entryBase.BoxedValue.ToString();
                            int initialIndex = enumNames.IndexOf(currentEnumName);
                            dropdownOption.SetValue(initialIndex);

                            dropdownOption.onChanged += () => {
                                string selectedName = enumNames[dropdownOption.value];
                                object enumValue = Enum.Parse(settingType, selectedName);

                                entryBase.BoxedValue = enumValue;
                            };
                        }
                        else
                        {
                            string currentValAsString = entryBase.BoxedValue?.ToString() ?? "";

                            UICreator.CreateStringInput(
                                container,
                                $"[?] {entryBase.Definition.Key}",
                                (newText) => {
                                    try
                                    {
                                        entryBase.BoxedValue = newText;
                                    }
                                    catch (Exception ex)
                                    {
                                        Plugin.Log.LogWarning($"[ModConfig] Failed to set unrecognised type {settingType}: {ex.Message}");
                                    }
                                },
                                currentValAsString
                            );
                        }
                    }
                }
            }
        }

        // Currently unused, I added this whilst debugging and we might aswell keep it.
        public static void SaveAllConfigs()
        {
            foreach (var plugin in Chainloader.PluginInfos)
            {
                try
                {
                    plugin.Value.Instance.Config.Save();
                    Plugin.Log.LogInfo($"Saved config for: {plugin.Value.Metadata.Name}");
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Failed to save config for {plugin.Value.Metadata.Name}: {ex.Message}");
                }
            }
        }
    }
}
