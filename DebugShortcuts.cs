using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Common.Testing
{
    public class DebugShortcuts : MonoBehaviour
    {
        private static readonly Dictionary<string, KeyCode> shortcuts = new()
        {
            { "Pause", KeyCode.Escape },
            { "LockCursor", KeyCode.Tab },
            { "UnlockCursor", KeyCode.Backspace },
            { "DefaultSpeed", KeyCode.Minus },
            { "SlowSpeed", KeyCode.Equals }
        };
        private static readonly Dictionary<string, System.Action> shortcutMethods = new()
        {
            { "Pause", Pause },
            { "LockCursor", LockCursor },
            { "UnlockCursor", UnlockCursor },
            { "DefaultSpeed", DefaultSpeed },
            { "SlowSpeed", SlowSpeed }
        };
        private void Update()
        {
            foreach (var identifier in shortcuts.Keys)
            {
                if (Input.GetKeyDown(shortcuts[identifier]))
                {
                    shortcutMethods[identifier]();
                }
            }
        }
        public static void Pause()
        {
            Debug.Break();
        }

        public static void LockCursor()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                return;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        public static void UnlockCursor()
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                return;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        public static void DefaultSpeed()
        {
            Time.timeScale = 1f;
        }

        private const float SlowSpeedFactor = 0.2f;
        public static void SlowSpeed()
        {
            Time.timeScale = SlowSpeedFactor;
        }
    }
}