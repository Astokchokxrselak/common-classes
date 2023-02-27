using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasVideoCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        #if UNITY_WEBGL
        gameObject.SetActive(false);
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
