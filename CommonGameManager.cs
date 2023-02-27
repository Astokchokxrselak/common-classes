using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        private void Awake()
        {
            instance = this;
            ScreenEffects.ResetScreen();
            ScreenEffects.FadeScreen(this, fadeInTime, Color.black, Color.clear, false);
            Time.timeScale = defaultTimeScale;
            ScreenshakeMultiplier = screenshakeMultiplier;
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
    }
}
