using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Helpers;
using Common.Extensions;
namespace Slide3D
{
    public class HitIndicator : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private void Awake()
        {
            TryGetComponent(out _rigidbody);
        }

        private const float _ArcAngle = 30f;
        public float initialLaunchForce;
        private void OnEnable()
        {
            var angle = RandomHelper.RandomSigned() * _ArcAngle;
            _rigidbody.AddForce(Quaternion.Euler(0, 0, angle) * Vector3.up * initialLaunchForce, ForceMode.VelocityChange);
        }
    }

}