using System;
using UnityEngine;

namespace ModConfig.Utilities
{
    public class KeybindHook : MonoBehaviour
    {
        public Action onRebind;

        public KeyCode PathToKeyCode(string path)
        {
            if (string.IsNullOrEmpty(path)) return KeyCode.None;

            string keyName = path.Contains("/") ? path.Split('/')[1] : path;

            if (Enum.TryParse(keyName, true, out KeyCode result)) return result;

            // Handle common mismatches
            return keyName.ToLower() switch
            {
                "escape" => KeyCode.Escape,
                "enter" => KeyCode.Return,
                "leftshift" => KeyCode.LeftShift,
                "rightshift" => KeyCode.RightShift,
                _ => KeyCode.None
            };
        }
    }
}
