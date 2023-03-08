using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common3D;

namespace Parkour3D 
{
    public class PlayerManager : MobileEntity3D
    {
        internal Transform head;
        internal PlayerMovementController3D motion;
        internal PlayerDashSlideController dash;
        new internal PlayerCameraController camera;
        internal PlayerWeaponController weapon;
        internal PlayerGravity gravity;
        public override void OnAwake()
        {
            motion = GetComponent<PlayerMovementController3D>();
            dash = GetComponent<PlayerDashSlideController>();
            camera = GetComponent<PlayerCameraController>();
            weapon = GetComponent<PlayerWeaponController>();
            gravity = GetComponent<PlayerGravity>();
            head = transform.Find("PlayerHead");
        }
        private const float DefaultDistanceFromGround = 2f;
        public bool IsGrounded()
        {
            return IsPlatformBelow(DefaultDistanceFromGround);
        }
        public bool IsGrounded(float distance)
        {
            return IsPlatformBelow(distance);
        }
        public void MoveInDirection(Vector3 localVector)
        {
            if (localVector != Vector3.zero)
            VelocityXZ = head.TransformDirection(localVector);
        }
        public void MoveInWorldDirection(Vector3 worldVector)
        {
            if (worldVector != Vector3.zero)
                VelocityXZ = worldVector;
        }
        public void SetCameraRotation(Vector3 lookRotation)
        {
            Camera.main.transform.rotation = Quaternion.LookRotation(lookRotation);
        }
        public void SetCameraPosition(Vector3 point)
        {
            Camera.main.transform.position = transform.TransformPoint(point);
        }
    }
}