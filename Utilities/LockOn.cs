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
    public float t = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, focus.position + offset, t);
        transform.rotation = Quaternion.Lerp(transform.rotation, focus.rotation, t);
    }
}
