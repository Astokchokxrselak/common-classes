using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceTrigger : MonoBehaviour
{
    public static bool voicesEnabled = true;
    Toggle m_toggle;
    // Start is called before the first frame update
    void Awake()
    {
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener(enabled => voicesEnabled = enabled);
    }
}
