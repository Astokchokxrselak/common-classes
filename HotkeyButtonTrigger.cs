using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.UI;
public class HotkeyButtonTrigger : MonoBehaviour
{
    private OnButton[] _buttons;
    public KeyCode hotkey;
    private void Awake()
    {
        _buttons = GetComponentsInChildren<OnButton>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(hotkey))
        {
            foreach (OnButton b in _buttons)
            {
                b.OnButtonClick();
            }
        }
    }
}
