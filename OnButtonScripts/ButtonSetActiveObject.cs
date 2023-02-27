using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Common.UI
{
    public class ButtonSetActiveObject : OnButton
    {
        public bool setActive;
        public GameObject disabledOnClick;
        public override void OnButtonClick()
        {
            disabledOnClick.SetActive(setActive);
        }
    }
}