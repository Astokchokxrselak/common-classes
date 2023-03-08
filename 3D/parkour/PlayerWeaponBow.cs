using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Pools;

namespace Parkour3D
{
    public class PlayerWeaponBow : PlayerWeapon
    {
        private const float AngleWhileSliding = 105f, DefaultAngle = 90f;
        private static readonly Vector3 WhileSlidingLocalPosition = new(0.4f, -1f, 2.5f), ShootingWhileSlidingLocalPosition = new(0f, -0.8f, 1.25f);
        private static readonly Vector3 DefaultLocalPosition = new(0.6f, 0f, 2f), ShootingLocalPosition = new(0f, 0f, 1.25f);
        public override Vector3 WeaponLocalPosition 
        {
            get
            {
                if (PlayerManager.dash.IsSliding)
                {
                    if (bow && bow.GetInteger("BowState") > 0)
                    {
                        return ShootingWhileSlidingLocalPosition;
                    }
                    return WhileSlidingLocalPosition;
                }
                else
                {
                    if (bow && bow.GetInteger("BowState") > 0)
                    {
                        return ShootingLocalPosition;
                    }
                    return DefaultLocalPosition;
                }
            }
        }
        Animator bow;
        // Start is called before the first frame update
        void Start()
        {
            transform.localPosition = WeaponLocalPosition;
            barrel = transform.Find("Barrel");
            TryGetComponent(out bow);
        }

        private Transform barrel;
        int frameCounter;
        // Update is called once per frame
        void Update()
        {
            AnimationManager();
        }

        private const float FOVWhenShooting = 60;
        void AnimationManager()
        {

            bow.SetInteger("FrameCounter", frameCounter);
            if (Input.GetMouseButtonDown(0))
            {
                bow.SetInteger("BowState", 1);
                PerspectiveCameraMaster.SetFOV(FOVWhenShooting);
                frameCounter = 0;
            }
            else
            {
                frameCounter++;
                if (Input.GetMouseButtonUp(0))
                {
                    if (frameCounter > 20)
                    {
                        bow.SetInteger("BowState", 2);
                        fireArrow = true;
                        PerspectiveCameraMaster.ResetFOV();
                    }
                    else
                    {
                        bow.SetInteger("BowState", 0);
                    }
                }
            }
        }
        bool fireArrow;
        private void FixedUpdate()
        {
            FixAngleBasedOnSlideState();
            CheckFireArrow();
        }
        void FixAngleBasedOnSlideState()
        {
            if (!PlayerManager.dash.IsActive)
            {
                transform.localEulerAngles = Vector3.forward * DefaultAngle;
            }
            else
            {
                transform.localEulerAngles = Vector3.forward * AngleWhileSliding;
            }
        }
        void CheckFireArrow()
        {
            if (fireArrow)
            {
                FireArrow();
                fireArrow = false;
                bow.SetInteger("BowState", 0);
            }
        }

        public PhysicsPool3D arrowPool;
        private const float ArrowFireVelocity = 100f;
        void FireArrow()
        {
            var arrow = arrowPool.GetObject(barrel.position, null);
            arrow.GetComponent<DamageOnContact>().targetTag = "Enemy";
            arrow.Transform.forward = transform.forward;
            arrow.rigidbody.velocity = transform.forward * ArrowFireVelocity;
        }
    }
}