using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Common;
public class MouseSensitivitySlider : MonoBehaviour
{
    Slider m_slider;
    public static float mouseSensitivity;
    public float maxSensitivity = 2;
    // Start is called before the first frame update
    void Start()
    {
        m_slider = GetComponent<Slider>();
        m_slider.onValueChanged.AddListener(value => mouseSensitivity = value * maxSensitivity);

        m_slider.value = 1f / maxSensitivity;
        // m_slider.onValueChanged.Invoke(0.8f);
    }
}
