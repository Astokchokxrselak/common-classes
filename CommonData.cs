using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Device;

namespace Common
{
    /// <summary>
    /// This script contains all of the helper classes shared between scripts in order to make it
    /// easier to do stuff
    /// 
    /// Timer - use for pausing events such as a dash, dodge, or roll
    /// </summary>

    #region Common Classes
    [System.Serializable]
    public struct WeightedFloatRandom
    {
        [SerializeField]
        private List<float> weights;
        private readonly float sum;
        public int WeightCount => weights.Count;
        public WeightedFloatRandom(int count, float expectedSum)
        {
            weights = new List<float>(count);
            sum = expectedSum;
            for (int i = 0; i < count; i++)
            {
                weights[i] = sum / count;
            }
        }
        public int GetIndex(float @in)
        {
            float sum = weights[0];
            for (int i = 0; i < weights.Count; sum += weights[++i])
            {
                if (sum > @in)
                {
                    return i;
                }
            } throw new System.ArgumentOutOfRangeException("Parameter @in is greater than the expected sum of list weights");
        }
    }
    [System.Serializable]
    public class Timer
    {
        public float timer;
        public float max;
        public float Max
        {
            get => max - offset;
        }
        public Timer(float timer, float max)
        {
            this.timer = timer;
            this.max = max;
        }
        public static implicit operator bool(Timer timer) => timer.timer >= timer.Max;
        public static implicit operator Timer(float flt) => new Timer(0, flt);
        public bool Hit(bool max) => max ? timer >= this.Max : timer <= 0;
        public float Increment(bool scaled = true) => timer += scaled ? Time.deltaTime : Time.unscaledDeltaTime;
        public float Increment(float d) => timer += d;
        public bool IncrementHit(bool max, bool scaled = true)
        {
            Increment(scaled);
            return Hit(max);
        }
        public bool IncrementHit(bool max, bool scaled, bool reset)
        {
            Increment(scaled);
            var hit = Hit(max);
            if (hit && reset)
            {
                SetZero();
            }
            return hit;
        }
        public bool IncrementHit(bool max, float d)
        {
            Increment(d);
            return Hit(max);
        }
        public float Decrement() => timer -= Time.unscaledDeltaTime;
        public float Decrement(float d) => timer -= d;
        public bool DecrementHit(bool max)
        {
            Decrement();
            return Hit(max);
        }
        public bool DecrementHit(bool max, float d)
        {
            Decrement(d);
            return Hit(max);
        }
        public bool DecrementHit(bool max, bool scaled, bool reset)
        {
            Decrement();
            var hit = Hit(max);
            if (hit && reset)
            {
                SetMax();
            } 
            return hit;
        }
        public float ClampedDecrement()
        {
            return ClampedDecrement(Time.unscaledDeltaTime);
        }
        public float ClampedDecrement(float d) // keeps within the range [0, 1]
        {
            Decrement(d);
            timer = Mathf.Clamp(timer, 0, Max);
            return timer;
        }
        public float ClampedIncrement()
        {
            return ClampedIncrement(Time.unscaledDeltaTime);
        }
        public float ClampedIncrement(bool scaled)
        {
            return ClampedIncrement(scaled ? Time.deltaTime : Time.unscaledDeltaTime);
        }
        public float ClampedIncrement(float d) // keeps within the range [0, 1]
        {
            Increment(d);
            timer = Mathf.Clamp(timer, 0, Max);
            return timer;
        }
        public bool ClampedIncrementHit(bool max, bool scaled, bool reset)
        {
            ClampedIncrement(scaled ?  Time.deltaTime : Time.unscaledDeltaTime);
            var hit = Hit(max);
            if (hit && reset)
            {
                SetZero();
            }
            return hit;
        }
        public float SetMax() => timer = Max;
        public float SetZero() => timer = 0;
        public float ClampedRatio => Mathf.Clamp01(Ratio);
        public float Ratio => Max == 0 ? 0f : timer / max;
        public static explicit operator float(Timer timer) => timer.timer;
        public static Timer operator -(Timer timer, float flt) => new Timer(timer.timer - flt, timer.Max);
        public static Timer operator +(Timer timer, float flt) => new Timer(timer.timer - flt, timer.Max);
        internal float offset;
        public bool HasOffset => offset != 0;
        public void ResetOffset() => offset = 0;
        public override string ToString()
        {
            return "Timer(Current: " + timer + ", Max: " + max + ")";
        }
    }
    [System.Serializable]
    public class MultiPhaseTimer : Timer
    {
        int phase;
        public int Phase => phase;
        public bool IsPhase(int phase) => this.phase == phase;
        public void NextPhase(bool set, float max, bool setMax = false)
        {
            SetPhase(phase + 1, set, max, setMax);
        }
        public void NextPhase(bool set, bool setMax = false)
        {
            SetPhase(phase + 1, set, setMax);
        }
        public void LastPhase(bool set, bool setMax = false)
        {
            SetPhase(phase - 1, set, setMax);
        }
        public void SetPhase(int nPhase, bool set, float max, bool setMax = false)
        {
            SetPhase(nPhase, set, setMax);
            this.max = max;
        }
        public void SetPhase(int nPhase, bool set, bool setMax = false)
        {
            phase = nPhase;
            if (set)
            {
                if (setMax) SetMax();
                else SetZero();
            }
        }
        public void Reset()
        {
            ResetOffset();
            SetPhase(0, true);
        }
        public MultiPhaseTimer(float timer, float max) : base(timer, max)
        {

        }
    }

    [System.Serializable]
    public class RangedTimer
    {
        public float timer;
        public float min, max;
        public RangedTimer(float timer, float min, float max)
        {
            this.timer = timer;
            this.min = min;
            this.max = max;
        }
        public RangedTimer(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public static implicit operator bool(RangedTimer timer) => timer.timer >= timer.max;
        public bool Hit(bool max) => max ? timer >= this.max : timer <= this.min;
        public float Increment(bool scaled = true) => timer += scaled ? Time.deltaTime : Time.unscaledDeltaTime;
        public float Increment(float d) => timer += d;
        public bool IncrementHit(bool max, bool scaled = true)
        {
            Increment(scaled);
            return Hit(max);
        }
        public bool IncrementHit(bool max, bool scaled, bool reset)
        {
            Increment(scaled);
            var hit = Hit(max);
            if (hit && reset)
            {
                SetMin();
            }
            return hit;
        }
        public bool IncrementHit(bool max, float d)
        {
            Increment(d);
            return Hit(max);
        }
        public float Decrement() => timer -= Time.unscaledDeltaTime;
        public float Decrement(float d) => timer -= d;
        public bool DecrementHit(bool max)
        {
            Decrement();
            return Hit(max);
        }
        public bool DecrementHit(bool max, float d)
        {
            Decrement(d);
            return Hit(max);
        }
        public bool DecrementHit(bool max, bool scaled, bool reset)
        {
            Decrement();
            var hit = Hit(max);
            if (hit && reset)
            {
                SetMax();
            }
            return hit;
        }
        public float ClampedDecrement()
        {
            return ClampedDecrement(Time.unscaledDeltaTime);
        }
        public float ClampedDecrement(float d) // keeps within the range [0, 1]
        {
            Decrement(d);
            timer = Mathf.Clamp(timer, min, max);
            return timer;
        }
        public float ClampedIncrement()
        {
            return ClampedIncrement(Time.unscaledDeltaTime);
        }
        public float ClampedIncrement(float d) // keeps within the range [0, 1]
        {
            Increment(d);
            timer = Mathf.Clamp(timer, min, max);
            return timer;
        }
        public float SetMax() => timer = max;
        public float SetMin() => timer = min;
        public float Ratio => timer / max;
        public static explicit operator float(RangedTimer timer) => timer.timer;
        public static RangedTimer operator -(RangedTimer timer, float flt) => new RangedTimer(timer.timer - flt, timer.min, timer.max);
        public static RangedTimer operator +(RangedTimer timer, float flt) => new RangedTimer(timer.timer - flt, timer.min, timer.max);
        internal float offset;
        public bool HasOffset => offset != 0;
        public void ResetOffset() => offset = 0;
    }
    #endregion

    namespace Helpers
    {
        public static class MathHelper
        {
            public static Vector3 VectorRotatedByVector(Vector3 inVec, Vector3 rotVec)
            {
                return Quaternion.LookRotation(rotVec) * inVec;
            }
            public static float SignOrZero(float n) => n == 0 ? 0 : Mathf.Sign(n);
            public static bool Between(float x, float a, float b) => Between(x, (a, b));
            public static bool Between(float x, (float, float) between) => x >= between.Item1 && x <= between.Item2;
            public static bool Vector2Between(Vector2 v, Vector2 a, Vector2 b) => Between(v.x, a.x, b.x) && Between(v.y, a.y, b.y);
            public static float AngleTo(Vector2 pos1, Vector2 pos2)
            {
                var diff = pos1 - pos2;
                var angle = Mathf.Atan2(diff.y, diff.x);
                return Mathf.Rad2Deg * angle;
            }
            public static float VectorAngle(Vector2 dir)
            {
                var angle = Mathf.Atan2(dir.y, dir.x);
                return Mathf.Rad2Deg * angle;
            }
            public static Vector2 AbsoluteVector2(Vector2 input) => new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
            public static Vector2Int AbsoluteVector2(Vector2Int input) => Vector2Int.FloorToInt(AbsoluteVector2((Vector2)input));
            public static bool XOR(params bool[] bools)
            {
                int c = 0;
                for (int i = 0; i < bools.Length; i++)
                {
                    if (bools[i])
                        if (c++ == 1)
                            return false;
                }
                return c == 1; // In XOR, only exactly one input will be true
            }
            public static float Ratio(Vector2 vector2) => vector2.x / vector2.y;
            public static bool WithinPM(float a, float b, float range) => Between(a, (b - range, b + range));
            public static int SecondsToFrames(float seconds) => (int)(50f * seconds);
            public static float Float01ToNegRange(float n) => n * 2 - 1;
            public static Vector2 DirectionTo(Vector2 one, Vector2 two) => (two - one).normalized;
            public static Vector2 UnitCircle(float t) => new(Mathf.Cos(t), Mathf.Sin(t));
        }
        public static class CameraHelper
        {
            public static Vector2 Resolution(Camera camera) => new Vector2(camera.scaledPixelWidth, camera.scaledPixelHeight);
            public static Vector2 Resolution() => Resolution(Camera.main);
            public static Vector2 WorldResolution(Camera camera) => camera.ScreenToWorldPoint(Resolution(camera));
            public static Vector2 WorldResolution() => WorldResolution(Camera.main);
            public static float WorldWidth() => (WorldResolution() - (Vector2)Camera.main.transform.position).x * 2;
            public static float WorldLength() => (WorldResolution() - (Vector2)Camera.main.transform.position).y * 2;
            public static Vector2 WorldDimensions() => new Vector2(WorldWidth(), WorldLength());
            public static Vector2 SignedPoint(Camera camera, Vector2 point) // assuming the point is already for an unsigned resolution, i.e. within the bounds of 0 to 840
            {
                var res = Resolution(camera);
                return point - res / 2f;
            }
            public static Vector2 SignedPoint(Vector2 point) => SignedPoint(Camera.main, point);
            public static Vector2 UnsignedPoint(Camera camera, Vector2 point) // assuming the point is already for a signed resolution, i.e. within the bounds of -840 to 840
            {
                var res = Resolution(camera);
                return point + res / 2f;
            }
            public static Vector2 UnsignedPoint(Vector2 point) => UnsignedPoint(Camera.main, point);
            public static bool ContainsScreenPoint(Vector2 point, bool isUnsigned = false) => ContainsScreenPoint(Camera.main, point, isUnsigned);
            public static bool ContainsScreenPoint(Camera camera, Vector2 point, bool isUnsigned = false)
            {
                if (isUnsigned)
                {
                    point = SignedPoint(camera, point);
                }
                var res = Resolution(camera);
                return MathHelper.Vector2Between(point, -res / 2, res / 2);
            }
            public static bool ContainsWorldPoint(Camera camera, Vector2 point) => ContainsScreenPoint(camera, Camera.main.WorldToScreenPoint(point), true);
            public static bool ContainsWorldPoint(Vector2 point) => ContainsWorldPoint(Camera.main, point);
            public static int2 IsOffscreenWorld(Camera camera, Vector2 point)
            {
                var isOffscreen = IsOffscreenWorldExtents(camera, point);
                return new int2()
                {
                    x = (int)MathHelper.SignOrZero(isOffscreen.x),
                    y = (int)MathHelper.SignOrZero(isOffscreen.y),
                };
            }
            public static int2 IsOffscreen(Vector2 point) => IsOffscreen(Camera.main, point);
            public static int2 IsOffscreen(Camera camera, Vector2 point) => IsOffscreenWorld(camera, camera.ScreenToWorldPoint(point));
            public static int2 IsOffscreenWorld(Vector2 point) => IsOffscreenWorld(Camera.main, point);
            public static Vector2 IsOffscreenWorldExtents(Camera camera, Vector2 point)
            {
                var res = WorldResolution(camera);
                var dim = WorldDimensions();
                var offs = res - point; // - WorldDimensions() / 2f;
                for (int coord = 0; coord < 2; coord++)
                {
                    if (!MathHelper.Between(offs[coord], 0, dim[coord]))
                    {
                        if (offs[coord] > dim[coord])
                        {
                            offs[coord] -= dim[coord];
                        }
                    }
                    else offs[coord] = 0;
                }
                return -offs;
            }
            public static Vector2 IsOffscreenWorldExtents(Vector2 point) => IsOffscreenWorldExtents(Camera.main, point);
            public static Vector2 DisplacementFromCamera(Vector3 point) => point - Camera.main.transform.position;
            public static Vector2 DisplacementFromCamera(Vector3 point, Camera camera) => point - camera.transform.position;
        }
        public static class InputHelper
        {
            public static Vector2 GetInput()
            {
                return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
            public static Vector2Int GetRawInput()
            {
                return Vector2Int.CeilToInt(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            }
            public static Vector3 Get3DInput()
            {
                return new Vector3(GetInput().x, 0, GetInput().y);
            }
            public static Vector3 GetRaw3DInput()
            {
                return new Vector3(GetRawInput().x, 0, GetRawInput().y);
            }
            public static Vector2 CenteredMousePosition(Camera camera)
            {
                return (Vector2)Input.mousePosition - CameraHelper.Resolution(camera) / 2f;
            }
            public static Vector2 CenteredMousePosition() => CenteredMousePosition(Camera.main);
            public static Vector2 WorldMousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
            public static Vector3 WorldMousePosition3D(float z) => Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * z);
        }
        public static class SpriteHelper
        {
            public static Sprite[] GetTextureSprites(string path) => Resources.LoadAll<Sprite>(path);
        }
        public static class StringHelper
        {
            public static string ToTime(double input)
            {
                TimeSpan t = TimeSpan.FromSeconds(input);

                string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
                return answer;
            }
        }

        public static class UIHelper
        {
            public static void SetTexts(TMPro.TMP_Text[] texts, object[] values)
            {
                if (texts.Length != values.Length)
                {
                    throw new ArgumentException("Length of texts is not equal to length of values");
                }
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].text = values[i].ToString();
                }
            }
        }
        public static class DebugHelper
        {
            static int MaxLoopsBeforeTermination = 5000;
            [Obsolete("SafeWhile should be primarily used for testing. Remember to replace it with a normal while loop.")]
            public static void SafeWhile(Func<bool> condition, Action func)
            {
                for (int loops = 0; condition(); loops++)
                {
                    func();
                    if (loops > MaxLoopsBeforeTermination)
                    {
                        Debug.Break();
                        throw new Exception("Reached max number of loops before pausing loop");
                    }
                }
            }
        }
        public static class PhysicsHelper
        {
            public static ContactPoint2D[] ContactPoint = new ContactPoint2D[1];
            public static T Nearest<T>(Vector2 position, float lookRadius, string tag = null) where T : class
            {
                var nearest = Physics2D.OverlapCircleAll(position, lookRadius).Where(col => tag == null || col.CompareTag(tag)).OrderByDescending(col => ((Vector2)col.transform.position - position).sqrMagnitude);
                return nearest.ElementAt(0).GetComponent<T>();
            }
            public static T Nearest<T>(Vector2 position, float lookRadius, GameObject sender, string tag = null) where T : class
            {
                var nearest = Physics2D.OverlapCircleAll(position, lookRadius).Where(col => col.gameObject != sender && (tag == null || col.CompareTag(tag))).OrderByDescending(col => ((Vector2)col.transform.position - position).sqrMagnitude);
                return nearest.ElementAt(0).GetComponent<T>();
            }
            public static T[] Near<T>(Vector2 position, float lookRadius, string tag = null) where T : class
            {
                var nearest = Physics2D.OverlapCircleAll(position, lookRadius).Where(col => tag == null || col.CompareTag(tag)).Select(obj => obj.GetComponent<T>());
                return nearest.ToArray();
            }
            public static T[] Near<T>(Vector2 position, float lookRadius, GameObject sender, string tag = null) where T : class
            {
                var nearest = Physics2D.OverlapCircleAll(position, lookRadius).Where(col => col.gameObject != sender && (tag == null || col.CompareTag(tag))).Select(obj => obj.GetComponent<T>());
                return nearest.ToArray();
            }
        }
        public static class CollectionsHelper
        {
            public static Dictionary<T1, T2> ListsToDict<T1, T2>(IEnumerable<T1> list1, IEnumerable<T2> list2)
            {
                return list1.Zip(list2, (k, v) => new { k, v }).ToDictionary(seq => seq.k, seq => seq.v);
            }
        }
        public static class RandomHelper
        {
            public static void ResetSeed() => UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
            public static Color RandomColor() => UnityEngine.Random.ColorHSV(0, 1, 0, 1);
            public static Color RandomColor(int seed)
            {
                UnityEngine.Random.InitState(seed);
                var color = RandomColor();
                ResetSeed();
                return color;
            }
            public static int RandomNumber(int min, int max) => UnityEngine.Random.Range(min, max + 1);
            public static int RandomNumber(int min, int max, int seed)
            {
                UnityEngine.Random.InitState(seed);
                var num = RandomNumber(min ,max);
                ResetSeed();
                return num;
            }
            public static float RandomNumber(float min, float max) => UnityEngine.Random.Range(min, max);
            public static float RandomNumber(float min, float max, int seed)
            {
                UnityEngine.Random.InitState(seed);
                var num = RandomNumber(min, max);
                ResetSeed();
                return num;
            }
            public static bool RandomBool(float bias) => UnityEngine.Random.value < bias;
            public static float RandomSigned() => MathHelper.Float01ToNegRange(UnityEngine.Random.value);
        }
    }
    namespace Extensions
    {
        public static class MathExtensions
        {
            public static Vector2 RotatedBy(this Vector2 vector2, float angle)
            {
                return Quaternion.Euler(0, 0, angle) * vector2;
            }
            public static Vector2 RotatedByRand(this Vector2 vector2, float angle1, float angle2)
            {
                var angle = UnityEngine.Random.Range(angle1, angle2);
                return vector2.RotatedBy(angle);
            }
            public static Vector2 RotatedByRandArc(this Vector2 vector2, float angle)
            {
                return vector2.RotatedByRand(-angle, angle);
            }
            public static Vector3 XZComponents(this Vector3 vector3) => new(vector3.x, 0, vector3.z);
            public static Vector3 Transform(this Vector3 vec3, int3 from, int3 to)
            {
                // Transform(Vector3(1, 5, -2), int3(1, 0, 2), int3(2, 2, 2))
                // IntOut: Vector3(-2, -2, -2)

                // targ = 1
                // targ = 0
                // targ = 2
                // for i in int3:
                Vector3 vector3 = vec3;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (to[j] == from[i])
                        {
                            vector3[j] = vec3[i];
                        }
                    }
                }
                return vector3;
            }
        }
        public static class ComponentExtensions
        {
            public static T[] GetComponentsInNextGen<T>(this Component obj) where T : Component
            {
                var components = new List<T>();
                foreach (Transform transform in obj.transform)
                {
                    if (transform.TryGetComponent(out T component))
                    {
                        components.Add(component);
                    }
                }
                return components.ToArray();
            }
            public static T[] GetComponentsInNextGen<T>(this MonoBehaviour obj) where T : MonoBehaviour
            {
                var components = new List<T>();
                foreach (Transform transform in obj.transform)
                {
                    if (transform.TryGetComponent(out T component))
                    {
                        components.Add(component);
                    }
                }
                return components.ToArray();
            }
            
            public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
            {
                if (!gameObject.TryGetComponent(out T component))
                {
                    return gameObject.AddComponent<T>();
                } return component;
            }
            public static T GetOrAddComponent<T>(this Component obj) where T : Component
            {
                if (!obj.TryGetComponent(out T component))
                {
                    return obj.gameObject.AddComponent<T>();
                }
                return component;
            }
            public static T GetInterfaceComponent<T>(this GameObject gameObject) where T : class
            {
                if (!typeof(T).IsInterface)
                {
                    throw new ArgumentException("Used GetInterfaceComponent<T> where T is not an interface");
                }
                var components = gameObject.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is T interf)
                    {
                        return interf;
                    }
                }
                return null;
            }
            public static T GetInterfaceComponent<T>(this Component obj) where T : class
            {
                if (!typeof(T).IsInterface)
                {
                    throw new ArgumentException("Used GetInterfaceComponent<T> where T is not an interface");
                }
                var components = obj.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is T interf)
                    {
                        return interf;
                    }
                } return null;
            }

            public static Vector3 CenterOfMass(this Transform transform)
            {
                Vector3 sum = Vector3.zero;
                foreach (Transform t in transform)
                {
                    sum += t.position;
                }
                return sum / transform.childCount;
            }
        }
        public static class SequenceExtensions
        {
            public static T Choice<T>(this IEnumerable<T> seq)
            {
                var choice = UnityEngine.Random.Range(0, seq.Count());
                return seq.ElementAt(choice);
            }
            public static T Choice<T>(this IEnumerable<T> seq, Func<T, bool> condition)
            {
                var iEnum = seq.Where(condition);
                return iEnum.Choice();
            }
            public static T ChoiceOrDefault<T>(this IEnumerable<T> seq)
            {
                try
                {
                    return seq.Choice();
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return default;
                }
            }
            public static T ChoiceOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> condition)
            {
                try
                {
                    return seq.Choice(condition);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return default;
                }
            }
            public static string ArrToString<T>(this IEnumerable<T> seq)
            {
                string @out = "{ ";
                foreach (var m in seq)
                {
                    @out += m.ToString() + ", ";
                }
                return @out + " }";
            }
        }
        public static class AnimatorExtensions
        {
            public static float NormalizedTime(this Animator animator) => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        public static class TransformExtensions
        {
            public static void IsolateChild(this Transform transform, int index)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    if (i != index)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else child.gameObject.SetActive(true);
                }
            }
        }
        public static class OtherExtensions
        {
            public static float Aspect(this Sprite sprite) => Helpers.MathHelper.Ratio(sprite.rect.size);
            public static Vector2 Size(this Texture2D tex) => new(tex.width, tex.height);
            public static Color RGB(this Color color) => new(color.r, color.g, color.b, 1);
        }
    }
    namespace Yields
    {
        public abstract class WaitAnd : CustomYieldInstruction
        {
            protected Action andMethod;
            protected abstract bool Condition();
            public override bool keepWaiting
            {
                get
                {
                    andMethod();
                    return Condition();
                }
            }
        }
        public class WaitForSecondsAnd : WaitAnd
        {
            private readonly float stopTime;
            public WaitForSecondsAnd(float seconds, Action action)
            {
                this.stopTime = Time.time + seconds;
                this.andMethod = action;
            }
            protected override bool Condition()
            {
                return Time.time < stopTime; // if we have not reached stopTime, keep waiting
            }
        }
        public class WaitUntilAnd : WaitAnd
        {
            private Func<bool> condition;
            public WaitUntilAnd(Func<bool> condition, Action action)
            {
                this.condition = condition;
                this.andMethod = action;
            }
            protected override bool Condition()
            {
                return !condition(); // if condition is not yet met, keep waiting
            }
        }

        public class WaitForSecondsOrUntil : CustomYieldInstruction
        {
            private readonly float stopTime;
            private Func<bool> condition;
            public WaitForSecondsOrUntil(float seconds, Func<bool> condition)
            {
                this.stopTime = Time.time + seconds;
                this.condition = condition;
            }
            public override bool keepWaiting
            {
                get
                {
                    Debug.Log(stopTime - Time.time);
                    return stopTime > Time.time && !condition();
                }
            }
        }
    }
}
/*public class SingletonException : System.Exception
{
    public SingletonException(string message)
    {
        this.Message = message;
    }
}*/