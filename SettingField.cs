using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingField : MonoBehaviour
{
    public string nameOn, nameOff;
    public string settingName;
    SettingManager manager;
    // Start is called before the first frame update
    void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        Text text = toggle.GetComponentInChildren<Text>();
        toggle.onValueChanged.AddListener(value =>
        {
            print("culled");
            text.text = toggle.isOn ? nameOn : nameOff;
            SettingManager.Settings[settingName] = value;
        });


        manager = GetComponentInParent<SettingManager>();
        SettingManager.Settings.TryAdd(settingName, toggle.isOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
