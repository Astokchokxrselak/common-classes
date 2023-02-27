using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI
{
    [AddComponentMenu("UI/On Button/Progress Screen on Button")]
    public class NextScreenOnButton : OnButton
    {
        public NextScreenOnButtonManager manager;
        public Transform parent;
        public bool backwards;
        int CurrentScreen { get => manager.CurrentIndex; set => manager.CurrentIndex = value; }
        public override void OnButtonClick()
        {
            if (backwards)
            {
                CurrentScreen--;
            }
            else
            {
                CurrentScreen++;
            }
        }
    }
}