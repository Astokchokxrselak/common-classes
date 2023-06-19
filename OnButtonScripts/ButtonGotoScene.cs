using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

namespace Common.UI
{
    [AddComponentMenu("UI/On Button/Go to Scene On Button")]
    public class ButtonGotoScene : OnButton
    {
        public int nextScene;
        public float fadeToBlackTime;
        public override void OnButtonClick()
        {
            if (fadeToBlackTime != 0)
            {
                ScreenEffects.ForceFadeScreen(this, fadeToBlackTime, Color.clear, Color.black, false);
                MusicManager.Fadeout(fadeToBlackTime);
                ScreenEffects.OnScreenFaded += () => SceneManager.LoadScene(nextScene);
                ButtonEnabled = false;
            }
            else SceneManager.LoadScene(nextScene);
        }
    }
}