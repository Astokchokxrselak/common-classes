using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertedMouseYToggle : MonoBehaviour
{
    private Toggle _toggle;
    private static bool _on;

    public bool _initialValue = true;
    public static int Multiplier => _on ? -1 : 1;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _toggle);
        _toggle.onValueChanged.AddListener(b =>
        {
            _on = b;
        });
        _toggle.isOn = _initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
