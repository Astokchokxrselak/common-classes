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

        public static Vector2 MouseDelta => Input.mousePosition - instance.oldMouse;
        public static Vector2 MouseVector => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * InvertedMouseYToggle.Multiplier) * MouseSensitivitySlider.mouseSensitivity;
        private void Awake()
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
    }
}
