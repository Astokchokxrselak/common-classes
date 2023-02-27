using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;

namespace Common.UI 
{
    public class NextScreenOnButtonManager : MonoBehaviour
    {
        public bool loop, resetOnDisable;
        public int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
                if (loop)
                {
                    currentIndex %= transform.childCount;
                }
                else
                {
                    currentIndex = System.Math.Clamp(currentIndex, 0, transform.childCount - 1);
                }

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (i != currentIndex)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                transform.GetChild(currentIndex).gameObject.SetActive(true);
            }
        }
        NextScreenOnButton[] buttons;
        private void Awake()
        {
            buttons = GetComponentsInChildren<NextScreenOnButton>();
            foreach (var button in buttons)
            {
                button.manager = this;
            }
        }
        private void OnDisable()
        {
            if (resetOnDisable)
            {
                CurrentIndex = 0;
            }
        }
    } 
}
