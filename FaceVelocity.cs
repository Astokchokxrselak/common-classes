using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class FaceVelocity : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private void Awake()
        {
            _SleepMagnitude = Physics2D.linearSleepTolerance;
            TryGetComponent(out _rb);
        }
        private static float _SleepMagnitude;
        private void FixedUpdate() 
        {
            if (_rb.velocity.sqrMagnitude > _SleepMagnitude * _SleepMagnitude)
            {
                _rb.rotation = Helpers.MathHelper.VectorAngle(_rb.velocity);
            }
        }
    }
}