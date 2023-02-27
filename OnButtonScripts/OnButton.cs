using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    // This is the base class for all OnButton classes.
    // THis can also be used for storing event handlers.
    public class OnButton : MonoBehaviour
    {
        Button button;
        public bool ButtonEnabled
        {
            get => button.interactable;
            set => button.interactable = value;
        }
        public void Awake()
        {
            OnInitialize(); // Initializes necessary data used by the OnButton script.
            button = GetComponent<Button>();
            OnButtonClicked += OnButtonClick;
            button.onClick.AddListener(() => OnButtonClicked());
        }
        public virtual void OnButtonClick()
        {

        }
        public virtual void OnInitialize()
        {

        }
        public event Action OnButtonClicked;
        public void ClearEvents() => OnButtonClicked = null;
    }
}
