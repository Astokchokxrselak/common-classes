using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Extensions;
using Common3D;

namespace Parkour3D
{
    public class PlayerDashSlideController : MobileEntity3D
    {
        private PlayerManager manager;
        public override void OnAwake()
        {
            manager = GetComponent<PlayerManager>();
        }

        private bool isMoving;
        private bool isSliding;
        private KeyCode dashKey = KeyCode.Q;
        public bool IsActive => isMoving || isSliding;
        public bool IsSliding => isSliding;
        public bool IsMoving => isMoving;
        
        private void Update()
        {
            if (!isMoving)
            {
                CheckForInput();
            }
        }

        void EnableDashSlide()
        {
            if (!manager.motion.AnyDirectionalInput())
            {
                motionDirection = manager.head.forward.XZComponents();
            }
            else
            {
                motionDirection = VelocityXZ.normalized;
            }
            if (manager.IsGrounded())
            {
                isSliding = true;
            }
            else
            {
                isSliding = false;
                dashTimer = new(0f, DashDuration);
            }
        }
        void CheckForInput()
        {
            isMoving = Input.GetKeyDown(dashKey);
            if (isMoving)
            {
                EnableDashSlide();
            }
        }

        private void FixedUpdate()
        {
            //if (manager.IsGrounded())
            //{
            //    WhenSliding();
            //}
            //else
            //{
            if (isSliding)
            {
                WhenSliding();
            }
            else
            {
                WhenDashing();
            }//}
        }
        void WhenSliding()
        {
            if (!Input.GetKey(dashKey))
            {
                DisableSlide();
                EnableManagerControlsSlide();
                return;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                DisableSlide();
                EnableManagerControlsSlide();
                JumpWhileSliding();
                return;
            }
            DisableManagerControlsSlide();
            SlideCameraUpdate();
            if (manager.IsGrounded())
            {
                SlideDetectTurn();
                SlideMotion();
            }
        }
        void JumpWhileSliding()
        {
            manager.motion.StartJump();
        }

        private const float DashDuration = .25f;
        MultiPhaseTimer dashTimer;
        private const int Dashing = 0, PostDash = 1;
        void WhenDashing()
        {
            if (!isMoving)
            {
                EnableManagerControlsDash();
                return;
            }
            DisableManagerControlsDash();
            if (dashTimer.IsPhase(Dashing))
                DashMotion();
            else if (dashTimer.IsPhase(PostDash))
                PostDashMotion();
        }
        private const float DashSpeed = 45f;
        void DashMotion()
        {
            manager.gravity.enabled = false;
            if (!dashTimer.IncrementHit(true, true, true))
            {
                manager.MoveInWorldDirection(motionDirection * DashSpeed);
                VelocityY = 0f;
            }
            else 
            {
                manager.gravity.enabled = true;
                dashTimer.NextPhase(true, PostDashDuration);
            }
        }

        private const float PostDashDuration = 0.9f, PostDashSpeed = 10f;
        void PostDashMotion()
        {
            manager.MoveInWorldDirection(motionDirection * PostDashSpeed);
            if (manager.IsGrounded())
            {
                isMoving = false;
                dashTimer.SetPhase(0, true, DashDuration);
                // if (Input.GetKey(dashKey))
                // {
                //    EnableDashSlide();
                // }
            }
            if (dashTimer.IncrementHit(true, true, true))
            {
                isMoving = false;
                dashTimer.SetPhase(0, true, DashDuration);
            }
        }
        void EnableManagerControlsDash()
        {
            manager.motion.enabled = true;
            manager.camera.Enable();
        }
        void DisableManagerControlsDash()
        {
            manager.motion.enabled = false;
            // manager.camera.setRotation = false;
        }
        void EnableManagerControlsSlide()
        {
            manager.motion.enabled = true;
            manager.camera.ResetAngleExtrema();
            manager.camera.Enable();
        }
        void DisableSlide()
        {
            isSliding = false;
            isMoving = false;
        }
        void DisableManagerControlsSlide()
        {
            manager.motion.enabled = false;
            manager.camera.setPosition = false;
        }

        private const float SlideSpeed = 12f, ShakeMagnitude = 4f, FrameDuration = 1f;
        private static readonly Vector2 CameraLocalPosition = new(0, 0.1f);
        Vector3 motionDirection;
        void SlideCameraUpdate()
        {
            manager.SetCameraPosition(CameraLocalPosition);
        }
        void SlideMotion()
        {
            manager.MoveInWorldDirection(manager.head.forward * SlideSpeed);
            ScreenEffects.Activate(ScreenEffects.Type.ScreenShake3D, Time.fixedDeltaTime * FrameDuration, ShakeMagnitude);
        }

        private const float TurnForce = 2;
        void SlideDetectTurn()
        {
            var turnDirection = Input.GetAxis("Horizontal") * TurnForce;
            var lookRotation = Quaternion.Euler(0, turnDirection, 0);
            motionDirection = lookRotation * motionDirection;
            manager.camera.Rotate(turnDirection * transform.up);
        }
    }
}