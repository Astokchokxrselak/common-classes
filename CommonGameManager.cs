using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    // This script contains update methods for the MusicManager, ScreenEffects, and other static classes.
    public class CommonGameManager : MonoBehaviour
    {
        public static float ScreenshakeMultiplier = 1f;
        public static CommonGameManager instance;
        public float fadeInTime = 0;
        public float defaultTimeScale = 1f;
        public float screenshakeMultiplier = 1f;

        public static bool IsMouseOverUIObject => UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        public static Vector2 MouseDelta => Input.mousePosition - instance.oldMouse;
        public static Vector2 MouseVector => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * InvertedMouseYToggle.Multiplier) * MouseSensitivitySlider.mouseSensitivity;
        public void Awake()
        {
            instance = this;
            ScreenEffects.ResetScreen();
            ScreenEffects.FadeScreen(this, fadeInTime, Color.black, Color.clear, false);
            Time.timeScale = defaultTimeScale;
            ScreenshakeMultiplier = screenshakeMultiplier;
            MouseSensitivitySlider.mouseSensitivity = 1f;
            GameAwake();
        }
        private void Update()
        {
            ScreenEffects.Update();
            GameUpdate();
            UpdateLastKeyPress();
        }
        protected virtual void GameAwake()
        {

        }
        Vector3 oldMouse;
        private void LateUpdate()
        {
            oldMouse = Input.mousePosition;
        }
        protected virtual void GameUpdate()
        {

        }
        public const float _FarFarAway = 9e9f;
        public static void SendToTheVoid(Transform transform)
        {
            transform.position = Vector3.one * _FarFarAway;
        }
        private static IEnumerator _KillOffscreen(GameObject gObj)
        {
            while (Helpers.CameraHelper.ContainsWorldPoint(gObj.transform.position))
                // while the object is still onscreen,
            {
                yield return null; // wait for it to become offscreen.
            }
            Destroy(gObj);
        }
        // Marks a game object to be destroyed when it goes offscreen.
        public static void KillWhenOffscreen(GameObject gameObject)
        {
            instance.StartCoroutine(_KillOffscreen(gameObject));
        }
        public static void SwitchScenes(int nextScene)
        {
            SwitchScenes(nextScene, instance.fadeInTime);
        }
        public static void SwitchScenes(int nextScene, float fadeToBlackTime)
        {
            if (fadeToBlackTime != 0)
            {
                ScreenEffects.ForceFadeScreen(instance, fadeToBlackTime, Color.clear, Color.black, false);
                MusicManager.Fadeout(fadeToBlackTime);
                ScreenEffects.OnScreenFaded += () => SceneManager.LoadScene(nextScene);
            }
            else SceneManager.LoadScene(nextScene);
        }

        public static void FadeInOut(Action onFadeIn)
        {
            ScreenEffects.OnScreenFaded += onFadeIn + new Action(() => {
                ScreenEffects.FadeScreen(instance, instance.fadeInTime, Color.clear);
            });

            print(instance);
            ScreenEffects.FadeScreen(instance, instance.fadeInTime, Color.black);
        }

        private static float _durationOfLastKeyPress;
        public static bool IsKeyHeldFor(KeyCode key, float minDuration) => Input.GetKey(key) && KeyPressDuration >= minDuration;
        /// <summary>
        /// This should be used exclusively in Coroutines or FixedUpdate contexts, as input gets a little wonky there. key - the key pressed. maxDuration - the maximum duration the key has been held before it is not considered "down" but "held".
        /// </summary>
        /// <param name="key"></param>
        /// <param name="maxDuration"></param>
        /// <returns></returns>
        public static bool IsKeyDown(KeyCode key, float maxDuration) => Input.GetKey(key) && KeyPressDuration < maxDuration;
        public static float KeyPressDuration => _durationOfLastKeyPress;
        private void UpdateLastKeyPress()
        {
            if (Input.anyKey)
            {
                _durationOfLastKeyPress += Time.deltaTime;
            }
            else
            {
                _durationOfLastKeyPress = 0;
            }
        }
    }
}
