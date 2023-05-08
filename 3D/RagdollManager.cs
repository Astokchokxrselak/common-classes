using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public Vector3 impulseDirection, impulsePosition;
    private Rigidbody[] rigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        Impulse();
    }

    void Impulse()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].AddForceAtPosition(impulseDirection, impulsePosition, ForceMode.Impulse);
        }
    }
}
