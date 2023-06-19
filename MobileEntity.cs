using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

using Common.Extensions;
using Common.Helpers;

namespace Common
{
    [DisallowMultipleComponent]
    public class MobileEntity : MonoBehaviour
    {
        // These methods are here because I find myself having to use them
        // Very often
        #region Static Methods shared by mobile entities
        public static bool TimePaused => Time.timeScale == 0f;
        #endregion
        private Rigidbody2D m_rigidbody2D;
        private Collider2D m_collider2D; 
        private SpriteRenderer m_spriteRenderer;
        private Animator m_animator;
        private SFXData m_sfx;

        public Rigidbody2D Rigidbody2D => m_rigidbody2D;
        public Collider2D Collider => m_collider2D;
        public SpriteRenderer SpriteRenderer => m_spriteRenderer;
        public Animator Animator => m_animator;
        public SFXData SFX => m_sfx;

        // Start is called before the first frame update
        public virtual void OnAwake()
        {

        }
        static RaycastHit2D[] HitsArray;
        public void Awake()
        {
            realScale = ScaleVector;
            if (HitsArray == null)
            {
                HitsArray = new RaycastHit2D[2];
            }
            TryGetComponent(out m_rigidbody2D);
            TryGetComponent(out m_collider2D);
            TryGetComponent(out m_spriteRenderer);
            TryGetComponent(out m_animator);
            TryGetComponent(out m_sfx);
            OnAwake();
        }
        public void SetActive(bool active) => gameObject.SetActive(active);
        public Vector2 BoundsSize
        {
            get => Bounds.size;
        }
        public Vector2 HalfBoundsSizeX // For when we need to offset something from the center horizontally
        {
            get => BoundsSize.x / 2f * Vector2.right;
        }
        public Vector2 HalfBoundsSizeY // For when we need to offset something from the center vertically
        {
            get => BoundsSize.x / 2f * Vector2.up;
        }
        public Bounds Bounds
        {
            get => (NullableSpriteBounds ?? NullableColliderBounds).GetValueOrDefault();
        }
        public Transform Parent
        {
            get => transform.parent;
            set => transform.parent = value;
        }
        public Transform GetChild(int n) => transform.GetChild(n);
        public Transform GetChild(string s) => transform.Find(s);
        public Vector3 TransPosition
        {
            get => transform.position;
            set => transform.position = value;
        }
        public Vector3 Position
        {
            get => Rigidbody2D ? Rigidbody2D.position : TransPosition;
            set
            {
                if (Rigidbody2D)
                {
                    Rigidbody2D.MovePosition(value);
                }
                else
                {
                    TransPosition = value;
                }
            }
        }
        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
        public Vector2 DirectionTo(Vector2 other)
        {
            return ((Vector3)other - Position).normalized;
        }
        public float PositionX
        {
            get => Position.x;
            set => Position = new Vector3(value, PositionY);
        }
        public float PositionY
        {
            get => Position.y;
            set => Position = new Vector3(PositionX, value);
        }
        public float TransPositionX
        {
            get => TransPosition.x;
            set => TransPosition = new Vector3(value, TransPositionY);
        }
        public float TransPositionY
        {
            get => TransPosition.y;
            set => TransPosition = new Vector3(TransPositionX, value);
        }

        public float LocalPositionX
        {
            get => LocalPosition.x;
            set => LocalPosition = new Vector2(value, LocalPosition.y);
        }

        public float LocalPositionY
        {
            get => LocalPosition.y;
            set => LocalPosition = new Vector2(LocalPosition.x, value);
        }

        public float LocalPositionZ
        {
            get => LocalPosition.z;
            set => LocalPosition = new Vector3(LocalPosition.x, LocalPosition.y, value);
        }

        public Quaternion QuaternionRotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public Vector3 EulerAngles
        {
            get => transform.eulerAngles;
            set => transform.eulerAngles = value;
        }

        Vector2 realScale;
        public Vector2 ScaleVector
        {
            get => transform.localScale;
            set => transform.localScale = realScale = value;
        }
        public float ScaleAsMultiplier => ScaleVector.magnitude;
        public void SetScale(float value) 
        { 
            transform.localScale = realScale * value;
        }

        /*
         * Helper properties relating to rigidbody2D fields
         */
        public Vector2 Velocity
        {
            get => Rigidbody2D.velocity;
            set => Rigidbody2D.velocity = value;
        }
        public float VelocityX
        {
            get => Velocity.x;
            set => Velocity = new Vector2(value, VelocityY);
        }
        public float VelocityY
        {
            get => Velocity.y;
            set => Velocity = new Vector2(VelocityX, value);
        }
        public float Rotation
        {
            get => Rigidbody2D.rotation;
            set => Rigidbody2D.rotation = value;
        }
        public float AngularVelocity
        {
            get => Rigidbody2D.angularVelocity;
            set => Rigidbody2D.angularVelocity = value;
        }

        public void Float() => GravityScale = 0f;
        public void Unfloat() => GravityScale = 1f;
        public float GravityScale
        {
            get => Rigidbody2D.gravityScale;
            set => Rigidbody2D.gravityScale = value;
        }

        public bool Kinematic
        {
            get => Rigidbody2D.isKinematic;
            set => Rigidbody2D.isKinematic = value;
        }

        /* 
         * Helper methods relating to Rigidbody2D and Collider2D
         */
        public void StopMoving()
        {
            Rigidbody2D.velocity = Vector2.zero;
        }
        /*
        Properties denoting specific points along the collider
        */
        public Vector3 Center
        // TODO: centroid of collider/rigidbody locations
        {
            get => Position;
        }
        public Vector3 Right
        {
            get => Position + Bounds.size.x / 2f * Vector3.right;
        }
        public Vector3 Left
        {
            get => Position + Bounds.size.x / 2f * Vector3.left;
        }
        public Vector3 Top
        {
            get => Position + Bounds.size.y / 2f * Vector3.up;
        }
        public Vector3 Bottom
        {
            get => Position + Bounds.size.y / 2f * Vector3.down;
        }
        Bounds? NullableColliderBounds
        {
            get => !Collider ? null : Collider.bounds;
        }
        public Bounds ColliderBounds
        {
            get => NullableColliderBounds.GetValueOrDefault();
        }
        public Vector3 PositionNearTo(Vector3 point) => Collider.bounds.ClosestPoint(point);
        /*
         * Methods common among different subclasses of MobileEntity
         */
        public const bool DebugMode = false;
        public bool IsObjectBelow(float objectMinDistance, params string[] tags) => IsObjectInDirectionFrom(Bottom, objectMinDistance, Vector2.down, tags);
        public bool IsPlatformBelow(float platformMinDistance) => IsObjectBelow(platformMinDistance, "Platform");
        public bool IsPlatformBelowRelative(float platformMinDistance) => IsObjectInDirectionFrom(Bottom, platformMinDistance, -transform.up, "Platform");
        public bool IsObjectInDirection(float objectMinDistance, Vector2 direction, params string[] tags)
        {
            var origin = PositionNearTo(Position + (Vector3)direction * 10000);
            return IsObjectInDirectionFrom(origin, objectMinDistance, direction, tags);
        }
        public bool IsObjectInDirectionFrom(Vector2 origin, float objectMinDistance, Vector2 direction, params string[] tags)
        {
            HitsArray[0] = HitsArray[1] = default;
            Physics2D.RaycastNonAlloc(origin - direction * 0.01f, direction, HitsArray, objectMinDistance);
            if (DebugMode)
            {
                if (HitsArray[1])
                {
                    print("Hit1: " + HitsArray[0].collider.name + ", Hit2: " + HitsArray[1].collider.name);
                }
                Debug.DrawLine(origin, origin + direction * objectMinDistance, Color.red, Time.fixedDeltaTime);
            }
            return HitsArray[0] && HitsArray[1] && (tags.Length == 0 || tags.Any((tag) => HitsArray[0].collider.CompareTag(tag) || HitsArray[1].collider.CompareTag(tag)));
        }
        public RaycastHit2D GetObjectBelow(float objectMinDirection, params string[] tags) => GetObjectInDirectionFrom(Bottom, objectMinDirection, Vector2.down, tags);
        public RaycastHit2D GetObjectInDirectionFrom(Vector2 origin, float objectMinDistance, Vector2 direction, params string[] tags)
        {
            HitsArray[0] = HitsArray[1] = default;
            Physics2D.RaycastNonAlloc(origin - direction * 0.01f, direction, HitsArray, objectMinDistance);
            if (DebugMode)
            {
                if (HitsArray[1])
                {
                    print("Hit1: " + HitsArray[0].collider.name + ", Hit2: " + HitsArray[1].collider.name);
                }
                Debug.DrawLine(origin, origin + direction * objectMinDistance, Color.blue, Time.fixedDeltaTime);
            }
            if ((tags.Length == 0 || tags.Any((tag) => HitsArray[0].collider.CompareTag(tag) || HitsArray[1].collider.CompareTag(tag))))
            {
                return HitsArray[0].transform != transform ? HitsArray[0] : HitsArray[1];
            }
            return default;
        }
        public bool CanSee(Rigidbody2D other)
        {
            return CanSeeInDirection(other, other.position - (Vector2)Position);
        }

        public bool CanSeeInDirection(Rigidbody2D other, Vector2 direction, bool debug = false)
        {
            HitsArray[0] = HitsArray[1] = default;
            var n = Collider.Raycast(direction, HitsArray);
            /*if (DebugMode) */Debug.DrawLine(Position, other.position + direction, Color.red, 0.05f);

            bool hitMyself = HitsArray[1].rigidbody == Rigidbody2D || HitsArray[0].rigidbody == Rigidbody2D;
            if (hitMyself)
            {
                return HitsArray[0].rigidbody == other || HitsArray[1].rigidbody == other;
            }
            else
            {
                var closest = HitsArray[0].CompareTo(HitsArray[1]) > 0 // HitsArray[0] > HitsArray[1]
                            ? HitsArray[1]
                            : HitsArray[0];
                if (debug)
                {
                    for (int i = 0; i < HitsArray.Length; i++)
                    {
                        if (!HitsArray[i].transform)
                        {
                            continue;
                        }
                        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        go.transform.position = HitsArray[i].point;
                        go.name = HitsArray[i].transform.name;
                        if (HitsArray[i].collider == closest.collider)
                        {
                            go.name += " (Closest in direction " + direction + ")";
                        }
                        Destroy(go, Time.fixedDeltaTime);
                    }
                    // Debug.Break();
                }
                return closest.rigidbody == other;
            }
        }

        public float SquareDistance(Component other)
        {
            return SquareDistance(other.transform.position);
        }
        public float SquareDistance(Vector3 point)
        {
            return Vector3.Distance(Position, point);
        }

        /*
         * Members relating to sprites and the sprite renderer
         */
        public Sprite Sprite {
            get => SpriteRenderer.sprite;
            set => SpriteRenderer.sprite = value;
        }
        public Color Color
        {
            get => SpriteRenderer.color;
            set => SpriteRenderer.color = value;
        }
        public float Direction
        {
            get => SpriteRenderer.flipX ? -1 : 1;
            set => SpriteRenderer.flipX = value < 0; 
        }
        public int SortingOrder
        {
            get => SpriteRenderer.sortingOrder;
            set => SpriteRenderer.sortingOrder = value;
        }

        Bounds? NullableSpriteBounds
        {
            get => !SpriteRenderer ? null : SpriteRenderer.bounds;
        }
        public Bounds SpriteBounds
        {
            get => NullableSpriteBounds.GetValueOrDefault();
        }
        public Material Material
        {
            get => SpriteRenderer.material;
            set => SpriteRenderer.material = value;
        }

        /*
         * Members relating to Animator
         */
        public float AnimatorNormalizedTime
        {
            get => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        public void PlayAnimation(string name)//, float normalizedTime = -1f)
        {
            Animator.Play(name);
        }
        public void PlayAnimation(string name, float normalizedTime = -1f)
        {
            Animator.Play(name, 0, normalizedTime);
        }
        public void SetAnimationNormalizedTime(float normalizedTime)
        {
            Animator.Play(0, 0, normalizedTime);
        }
        public bool AnimatorIsName(string name)
        {
            var animatorArr = Animator.GetCurrentAnimatorClipInfo(0);
            return animatorArr.Length != 0 && animatorArr[0].clip.name == name;
        }
    }
}