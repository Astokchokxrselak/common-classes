using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common3D
{
    public class MobileEntity3D : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody Rigidbody;
        #region Rigidbody Related Functions
        public Vector3 Position
        {
            get => Rigidbody == null ? transform.position : Rigidbody.position;
            set
            {
                if (Rigidbody == null)
                {
                    transform.position = value;
                }
                else
                {
                    Rigidbody.position = value;
                }
            }
        }
        public Vector3 Velocity
        {
            get => Rigidbody.velocity;
            set => Rigidbody.velocity = value;
        }
        public float VelocityX
        {
            get => Rigidbody.velocity.x;
            set => Rigidbody.velocity = new Vector3(value, VelocityY, VelocityZ);
        }
        public float VelocityY
        {
            get => Rigidbody.velocity.y;
            set => Rigidbody.velocity = new Vector3(VelocityX, value, VelocityZ);
        }
        public float VelocityZ
        {
            get => Rigidbody.velocity.z;
            set => Rigidbody.velocity = new Vector3(VelocityX, VelocityY, value);
        }
        public Vector3 VelocityXZ
        {
            get => new(VelocityX, 0, VelocityZ);
            set
            {
                VelocityX = value.x;
                VelocityZ = value.z;
            }
        }
        public Vector3 Rotation
        {
            get => RotationQuaternion.eulerAngles;
            set => RotationQuaternion = Quaternion.Euler(value);
        }
        public Quaternion RotationQuaternion
        {
            get => Rigidbody == null ? transform.rotation : Rigidbody.rotation;
            set
            {
                if (Rigidbody != null)
                {
                    Rigidbody.rotation = value;
                }
                else
                {
                    transform.rotation = value;
                }
            }
        }
        #endregion
        [HideInInspector]
        public Collider Collider;
        #region Collider Related Data
        #endregion
        #region Bounds Related Data
        private Bounds? NColliderBounds => Collider == null ? null : Collider.bounds;
        public Bounds ColliderBounds => NColliderBounds.Value;
        [HideInInspector]
        public Bounds Bounds
        {
            get
            {
                return (Bounds)(NColliderBounds);
            }
        }
        #endregion
        [HideInInspector]
        public MeshRenderer Renderer;
        #region Transform Related Data
        public Quaternion QuaternionRotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
        #endregion
        public virtual void OnAwake()
        {
        }
        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>(); // Rigidbody should be attached to the mobile entity itself
            Collider = GetComponentInChildren<Collider>(); // Colliders may be children
            Renderer = GetComponent<MeshRenderer>();
            MiscellaneousInit();
            OnAwake();
        }
        void MiscellaneousInit()
        {
            platformRay = new Ray(transform.position, Vector3.down);
        }

        private Ray platformRay;
        public bool IsPlatformBelow(float distance)
        {
            return PlatformBelow(distance) != null;
        }
        public bool IsPlatformInDirection(float distance, Vector3 direction)
        {
            return PlatformInDirection(distance, direction) != null;
        }

        const float PlatformCheckYOffset = 0.1f;
        public Collider PlatformInDirection(float distance, Vector3 direction)
        {
            platformRay.direction = direction;
            platformRay.origin = Bounds.center;
            var cast = Physics.Raycast(platformRay, out RaycastHit hit, distance);
            if (Common.MobileEntity.DebugMode)
            {
                Debug.DrawLine(platformRay.origin, platformRay.origin + platformRay.direction * distance, Color.red, 0.05f);
                /*if (cast)
                {
                    Debug.Break();
                }*/
            }
            return hit.collider;
        }
        public Collider PlatformBelow(float distance)
        {
            return PlatformInDirection(distance, Vector3.down);
        }
        public Collider PlatformBelowBoxcast(float distance, Vector2 size)
        {
            platformRay.origin = transform.position;
            var cast = Physics.BoxCast(platformRay.origin, size / 2f, platformRay.direction, out RaycastHit hit, RotationQuaternion, distance);
            print("Boxcast: " + cast);
            if (Common.MobileEntity.DebugMode)
            {
                Debug.DrawLine(platformRay.origin, platformRay.origin + platformRay.direction * distance, Color.yellow, 0.05f);
            }
            return hit.collider;
        }
        public Vector3 DirectionTo(Component other)
        {
            return DirectionTo(other.transform.position);
        }
        public Vector3 DirectionTo(Vector3 point)
        {
            return (point - Position).normalized;
        }
        public float SquareDistanceXYZ(Component other)
        {
            return SquareDistanceXYZ(other.transform.position);
        }
        public float SquareDistanceXYZ(Vector3 point)
        {
            return Vector3.Distance(Position, point);
        }
        public float SquareDistanceXZ(Component other)
        {
            return SquareDistanceXZ(other.transform.position);
        }
        public float SquareDistanceXZ(Vector3 point)
        {
            Vector2 xz1 = new Vector2(point.x, point.z), xz2 = new Vector2(Position.x, Position.z);
            return Vector2.Distance(xz1, xz2);
        }
    }
}