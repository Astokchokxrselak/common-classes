using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Common.Helpers;

namespace Common
{
    public static class ScreenEffects
    {
        public enum Type
        {
            /// <summary>
            /// Screen Shake shakes the screen.
            /// Frequency => 5 minus Frequency is the frequency at which the screen shakes.
            /// Force => 0.25 times Force is the maximum magnitude at which the screen is shaken.
            /// </summary>
            /// <returns></returns>
            ScreenShake,
            /// <summary>
            /// Pause pauses the screen for FrameDuration frames.
            /// Frequency => Does nothing.
            /// Force => Does nothing.
            /// </summary>
            /// <returns></returns>
            Pause,
            /// <summary>
            /// Vignette Zoom zooms into the screen while applying a vignette.
            /// Frequency => Frequency is the weight of the vignette (alpha channel).
            /// Force => Force is the change in orthographic size.
            /// </summary>
            /// <returns></returns>
            VignetteZoom,
            /// <summary>
            /// Screen Shake 3D shakes the screen in a sphere instead of a circle.
            /// Frequency => Look to Screen Shake Frequency
            /// Force => Look to Screen Shake Force
            /// </summary>
            ScreenShake3D,
            /// <summary>
            /// Slow Down temporarily changes the TimeScale.
            /// Frequency => Frequency is the lerp T that timeScale is reset to default value. (0 - 1, 1 - 10)
            /// Force => Determines the new TimeScale (1 - 0, 1 - 10)
            /// </summary>
            SlowDown
        }
        internal interface IScreenEffect
        {
            public abstract int FrameDuration { get; set; }
            public abstract int FrameCounter { get; set; }
            public abstract bool IsDone { get; set; }
            public abstract void Update();
            public abstract void Init();
            public void Apply()
            {
                FrameCounter++;
                if (FrameCounter == 1)
                {
                    Init();
                }
                if (FrameCounter > FrameDuration)
                {
                    IsDone = true;
                }
                Update();
            }
        }
        #region Toggle Screen Effects
        internal class Pause : IScreenEffect
        {
            public int FrameDuration { get; set; }
            public int FrameCounter { get; set; }
            public bool IsDone { get; set; }
            float oldScale;
            public void Init()
            {
                numPause++;
                oldScale = Time.timeScale;
                Time.timeScale = 0f;
            }
            static int numPause;
            public void Update() 
            {
                if (IsDone)
                {
                    Time.timeScale = oldScale;
                    OnPauseDone?.Invoke();
                    OnPauseDone = null;
                    numPause--;
                }
            } 
        }
        #endregion
        #region Measured Screen Effects
        internal abstract class MeasuredScreenEffect : IScreenEffect
        {
            public int FrameDuration { get; set; }
            public int FrameCounter { get; set; }
            public bool IsDone { get; set; }
            public float frequency;
            public float force;
            public abstract void Init();
            public abstract void Update();
        }
        internal class VignetteZoom : MeasuredScreenEffect
        {
            public override void Init()
            {
                oldOrtho = Camera.main.orthographicSize;
            }
            float deltaOrtho, oldOrtho;
            const float RatioFadeIn = 0.3f, RatioFadeOut = 0.7f;
            public override void Update()
            {
                var frameRatio = (float)FrameCounter / FrameDuration;
                if (frameRatio < RatioFadeIn)
                {
                    var ratio = frameRatio / RatioFadeIn;
                    deltaOrtho = Mathf.Lerp(deltaOrtho, force, ratio * ratio);
                }
                else if (frameRatio > RatioFadeOut)
                {
                    var ratio = (1 - frameRatio) / (1 - RatioFadeOut);
                    deltaOrtho = Mathf.Lerp(0, deltaOrtho, ratio * ratio);
                }
                Camera.main.orthographicSize = !IsDone ? oldOrtho - deltaOrtho : oldOrtho;
                /*if (oldOrtho != Camera.main.orthographicSize + deltaOrtho)
                {
                    oldOrtho += Camera.main.orthographicSize - oldOrtho;
                }*/
            }
        }
        internal class ScreenShake : MeasuredScreenEffect, IScreenEffect
        {
            Vector3 cameraOffset, origin;
            public override void Init()
            {
                origin = Camera.main.transform.position;
            }
            const float PerspectiveShiftMultiplier = 2.5f;
            public override void Update()
            {
                if (origin != Camera.main.transform.position - cameraOffset)
                {
                    origin = Camera.main.transform.position - cameraOffset;
                }
                if (FrameCounter % frequency == 0)
                {
                    cameraOffset = UnityEngine.Random.insideUnitCircle * force;
                    if (Camera.main.orthographic)
                    {
                        cameraOffset *= PerspectiveShiftMultiplier;
                    }
                } Camera.main.transform.position = origin + cameraOffset;
                if (IsDone)
                {
                    Camera.main.transform.position = origin;
                }
            }
        }
        internal class ScreenShake3D : MeasuredScreenEffect, IScreenEffect
        {
            Vector3 cameraOffset, origin;
            public override void Init()
            {
                origin = Camera.main.transform.position;
            }
            const float PerspectiveShiftMultiplier = 2.5f;
            public override void Update()
            {
                if (origin != Camera.main.transform.position - cameraOffset)
                {
                    origin = Camera.main.transform.position - cameraOffset;
                }

                if (FrameCounter % frequency == 0)
                {
                    cameraOffset = UnityEngine.Random.insideUnitSphere * force;
                    if (Camera.main.orthographic)
                    {
                        cameraOffset *= PerspectiveShiftMultiplier;
                    }
                }
                Camera.main.transform.position = origin + cameraOffset;
                if (IsDone)
                {
                    Camera.main.transform.position = origin;
                }
            }
        }
        internal class SlowDown : MeasuredScreenEffect, IScreenEffect
        {
            public override void Init()
            {
            
            }
            public override void Update()
            {
                var lerpT = Mathf.Lerp(0, 1, frequency * FrameCounter / FrameDuration);
                Time.timeScale = Mathf.Lerp(force, CommonGameManager.instance.defaultTimeScale, lerpT);
                if (IsDone)
                {
                    Time.timeScale = CommonGameManager.instance.defaultTimeScale;
                }
            }
        }
        #endregion
        static List<IScreenEffect> screenEffects = new List<IScreenEffect>();
        private static readonly Dictionary<Type, System.Type> typeMatch = new()
        {
            { Type.ScreenShake, typeof(ScreenShake) },
            { Type.ScreenShake3D, typeof(ScreenShake3D) },
            { Type.SlowDown, typeof(SlowDown) }
        };
        public static bool EffectIsOn(Type type) => screenEffects.Any(se => typeMatch[type] == se.GetType());
        public static void Clear() => screenEffects.ForEach(effect => effect.IsDone = true);
        public static void KillPause() => screenEffects.FindAll(effect => effect is Pause).ForEach(effect => effect.IsDone = true);
        public static void Activate(Type type, float duration = 1f, float magnitude = 3f, bool scaled = true) // 1 - least, 10 - most
            // 1 - barely noticeable
            // 3 - default
            // 10 - insufferable
        {
            if (!MathHelper.Between(magnitude, 0f, 10f))
            {
                throw new ArgumentException("Magnitude does not fit in the range of [0.0, 10.0]!");
            }
            switch (type)
            {
                case Type.ScreenShake:
                    float frequencyMax = 5f;
                    #if UNITY_WEBGL
                        screenEffects.Add(new ScreenShake() { force = magnitude * 0.025f, frequency = 5 - (int)magnitude});
                    #else
                        screenEffects.Add(new ScreenShake() { force = magnitude * 0.025f, frequency = (int)((10 - magnitude) / 10 * frequencyMax)});
                    #endif
                    break;
                case Type.Pause:
                    var screenEffect = screenEffects.Find((effect) => effect is Pause);
                    if (screenEffect != null)
                    {
                        screenEffect.FrameDuration += (int)(duration * 50);
                        return;
                    } screenEffects.Add(new Pause());
                    break;
                case Type.VignetteZoom:
                    magnitude /= 10f;
                    screenEffects.Add(new VignetteZoom() { force = magnitude * 5f, frequency = magnitude * 255f });
                    break;
                case Type.ScreenShake3D:
                    float frequencyMax3D = 5f;
                    #if UNITY_WEBGL
                        screenEffects.Add(new ScreenShake3D() { force = magnitude * 0.025f, frequency = 5 - (int)magnitude});
                    #else
                        screenEffects.Add(new ScreenShake3D() { force = magnitude * 0.05f * CommonGameManager.ScreenshakeMultiplier, frequency = (int)((10 - magnitude) / 10 * frequencyMax3D) + 1 });
                    #endif
                    break;
                case Type.SlowDown:
                    if (!EffectIsOn(Type.SlowDown)) 
                    {
                        screenEffects.Add(new SlowDown() { force = 1f - Mathf.Log10(magnitude), frequency = Mathf.Log10(magnitude) });
                    }
                    else
                    {
                        Debug.LogWarning("Cannot create a new Type.SlowDown ScreenEffect; a Type.Slowdown ScreenEffect is already in progress");
                    }
                    break;
            }
            screenEffects.Last().FrameDuration = (int)(50 * duration);
        }

        static Image screen;
        static Image Screen
        {
            get
            {
                if (!screen)
                {
                    screen = GameObject.Find("ScreenEffectsCanvas/Screen").GetComponent<Image>();
                } return screen;
            }
        }
        public static bool fadingScreen;
        public static event Action OnScreenFaded;
        public static event Action OnPauseDone;
        public static void ResetScreen()
        {
            fadingScreen = false;
            OnScreenFaded = null;
            OnPauseDone = null;
        }
        public static Coroutine FadeScreen(MonoBehaviour source, float time, Color beforeColor, Color afterColor, bool scaled = true)
        {
            Debug.Log(fadingScreen);
            if (!fadingScreen)
            {
                return ForceFadeScreen(source, time, beforeColor, afterColor, scaled);
            } return null;
        }
        public static Coroutine FadeScreen(MonoBehaviour source, float time, Color color, bool scaled = true)
        { 
            return FadeScreen(source, time, Screen.color, color, scaled);
        }
        public static Coroutine ForceFadeScreen(MonoBehaviour source, float time, Color beforeColor, Color afterColor, bool scaled = true)
        {
            return source.StartCoroutine(FadeScreenIEnum(time, beforeColor, afterColor, scaled));
        }
        public static void SetScreen(Color color)
        {
            Screen.color = color;
        }
        static IEnumerator FadeScreenIEnum(float t, Color bColor, Color aColor, bool scaled)
        {
            fadingScreen = true;
            yield return new WaitForEndOfFrame();
            for (float i = 0; i <= t; i += scaled ? Time.deltaTime : Time.unscaledDeltaTime)
            {
                Screen.color = Color.Lerp(bColor, aColor, i / t);
                yield return null;
            }
            Screen.color = aColor;
            fadingScreen = false;
            OnScreenFaded?.Invoke();
            OnScreenFaded = null;
        }
        public static void Update() //(UnityEngine.Object source)
        {
            /*var instanceID = source.GetInstanceID();
            if (sourceID == 0)
            {
                sourceID = instanceID;
            }
            else if (sourceID != instanceID)
            {
                throw new Exception("Only one method should be updating ScreenEffects every frame!");
            }*/
            for (int i = 0; i < screenEffects.Count; i++)
            {
                var screenEffect = screenEffects[i];
                screenEffect.Apply();
                if (screenEffect.IsDone)
                {
                    screenEffects.Remove(screenEffect);
                }
            }
        }
    }
}
