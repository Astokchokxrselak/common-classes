using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Common.UI
{
    public class RestartSceneOnButton : OnButton
    {
        int index;
        public override void OnInitialize()
        {
            index = SceneManager.GetActiveScene().buildIndex;
        }
        public float fadeToBlackTime;
        public override void OnButtonClick()
        {
            if (fadeToBlackTime != 0)
            {
                ScreenEffects.ForceFadeScreen(this, fadeToBlackTime, Color.clear, Color.black, false);
                MusicManager.Fadeout(fadeToBlackTime);
                ScreenEffects.OnScreenFaded += () => SceneManager.LoadScene(index);
                ButtonEnabled = false;
            }
            else SceneManager.LoadScene(index);
        }
    }
}