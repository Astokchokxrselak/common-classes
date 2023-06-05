using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common3D
{
    public class FaceVelocity : MonoBehaviour
    {
        private Rigidbody _rb;
        private void Awake() => TryGetComponent(out _rb);
        private void FixedUpdate() => _rb.rotation = Quaternion.LookRotation(_rb.velocity);
    }
}