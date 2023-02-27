using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    [AddComponentMenu("UI/On Button/Quit On Button")]
    public class ButtonQuitGame : OnButton
    {
        public override void OnButtonClick()
        {
            Application.Quit();
        }
    }
}