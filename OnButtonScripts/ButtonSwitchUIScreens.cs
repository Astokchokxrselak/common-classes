using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    [AddComponentMenu("UI/On Button/Switch Screens On Button")]
    public class ButtonSwitchUIScreens : OnButton
    {
        public Transform parent;
        public int targetScreen, defaultScreen = -1;
        bool triggered;

        private NextScreenOnButtonManager _manager;
        public override void OnInitialize()
        {
            parent.TryGetComponent(out _manager);
        }
        public override void OnButtonClick()
        {
            if (_manager)
            {
                _Manager_OnClick();
            }
            else
            {
                _NoManager_OnClick();
            }
        }
        private void _NoManager_OnClick()
        {
            triggered = !triggered;
            if (triggered)
            {
                OnEnabled();
            }
            else
            {
                OnDisabled();
            }
        }

        private void _Manager_OnClick()
        {
            triggered = !triggered;
            if (triggered)
            {
                _manager.CurrentIndex = targetScreen;
            }
            else if (defaultScreen != -1)
            {
                _manager.CurrentIndex = defaultScreen;
            }
        }

        private void OnEnable()
        {
            triggered = false;
        }
        public void EnableScreen(int screen)
        {
            bool killMe = false;
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (i != screen)
                {
                    if (child != this.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else killMe = true;
                }
                else child.gameObject.SetActive(true);
            }
            if (killMe)
            {
                gameObject.SetActive(false);
            }
        }
        void OnEnabled()
        {
            EnableScreen(targetScreen);
        }
        void OnDisabled()
        {
            if (defaultScreen != -1)
            {
                EnableScreen(defaultScreen);
            }
        }
    }
}