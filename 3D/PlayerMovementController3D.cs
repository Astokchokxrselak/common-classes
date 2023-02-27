using System.Reflection;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common3D;
using Common;
using Common.Extensions;
using Common.Helpers;
using Common.Testing;

namespace Parkour3D
{
    public class PlayerMovementController3D : MobileEntity3D
    {
        private PlayerManager manager;
        private void Start()
        {
            manager = GetComponent<PlayerManager>();
        }
        private void Update()
        {
            GetDirectionalInput();
            Jump();
        }
        Vector3 input;
        public Vector3 GetInput()
        {
            return input;
        }
        private const float InputMagnitudeTolerance = 0.1f;
        public bool AnyDirectionalInput()
        {
            return input.sqrMagnitude >= InputMagnitudeTolerance * InputMagnitudeTolerance;
        }
        public float speed;
        void GetDirectionalInput()
        {
            input = InputHelper.Get3DInput();
        }

        private void FixedUpdate()
        {
            Move();
        }
        Vector3 oldPosition;
        private void LateUpdate()
        {
            oldPosition = transform.position;
        }
        void Move()
        {
            manager.MoveInDirection(new Vector3(input.x, 0, input.z) * speed);
            // characterController.SimpleMove(head.TransformDirection(new Vector3(input.x, 0, input.z)) * speed);
        }

        private const float HeadBobInterval = 0f, HeadbobMagnitudeDivisor = 15f, HeadLerpT = 0.05f;
        Timer headBob = HeadBobInterval;
        /*void HeadBob()
        {
            headBob.max = Velocity.magnitude / speed * HeadBobInterval;
            headBob.ClampedIncrementHit(true, true, true);
            nextCameraPosition = Vector3.Lerp(DefaultCameraLocalPosition, BobbedCameraLocalPosition, Mathf.Cos(Mathf.PI * 2 * headBob.Ratio));
            cameraPosition = Vector3.Lerp(cameraPosition, nextCameraPosition, HeadLerpT);
            Camera.main.transform.position = transform.TransformPoint(cameraPosition);
        }*/

        private const float JumpDuration = 5f, MaxJumpSpeed = 12f;
        private const float PlatformCheckLength = 3f;
        Timer jumpTimer = JumpDuration;
        bool isJumping = false;
        IEnumerator IEnumJump()
        {
            if (!IsPlatformBelow(PlatformCheckLength))
            {
                yield break;
            }
            isJumping = true;
            Rigidbody.AddForce(Vector3.up * MaxJumpSpeed, ForceMode.VelocityChange);
            while (!IsPlatformBelow(PlatformCheckLength))
            {
                yield return new WaitForFixedUpdate();
            }
            isJumping = false;
        }

        public void StartJump() => StartCoroutine(IEnumJump());
        private KeyCode JumpKey = KeyCode.Space;
        void Jump()
        {
            if (Input.GetKeyDown(JumpKey) && !isJumping)
            {
                StartJump();
            }
        }
    }
}