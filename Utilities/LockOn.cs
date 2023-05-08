using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public static LockOn instance;
    private void Start()
    {
        instance = this;
    }
    public Transform focus;
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = focus.position + offset;
        transform.rotation = focus.rotation;
    }
}
