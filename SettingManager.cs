using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Common;
using Common.UI;
using Common.Helpers;
public class SettingManager : ToggleManager
{
    static Dictionary<string, bool> settings;
    public static Dictionary<string, bool> Settings => settings == null ? (settings = new()) : settings;
    public static bool Get(string name) => Settings[name];
    public static bool Get(string name, bool fallback) => Settings.ContainsKey(name) ? Get(name) : (Settings[name] = fallback);
    // Start is called before the first frame update
    void Awake()
    {
        settings = CollectionsHelper.ListsToDict(togglesNames, toggles.Select(toggle => toggle.isOn));
    }
}
