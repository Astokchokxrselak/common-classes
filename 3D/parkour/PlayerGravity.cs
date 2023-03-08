using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common3D;
public class PlayerGravity : MobileEntity3D
{
    public float magnitude;
    public Vector3 direction = Vector3.down;
    private void FixedUpdate()
    {
        Rigidbody.AddForce(magnitude * direction);
    }
}
