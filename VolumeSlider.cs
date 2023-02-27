using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Slider m_slider;
    public static float volume;
    // Start is called before the first frame update
    void Start()
    {
        m_slider = GetComponent<Slider>();
        m_slider.onValueChanged.AddListener(value => AudioListener.volume = volume = value);

        m_slider.value = 0.8f;
        // m_slider.onValueChanged.Invoke(0.8f);
    }
    private void Awake()
    {
        AudioListener.volume = volume;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
