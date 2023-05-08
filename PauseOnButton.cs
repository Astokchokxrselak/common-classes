using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class PauseOnButton : OnButton
    {
        private Button m_button;
        public override void OnButtonClick()
        {
            PauseManager.TogglePause();
        }
    }
}