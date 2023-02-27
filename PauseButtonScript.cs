using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class PauseButtonScript : MonoBehaviour
    {
        Button m_button;
        public GameObject menu;
        public static bool isPaused;
        public static bool canBePaused;
        public static readonly KeyCode pauseKey = KeyCode.Escape;
        // Start is called before the first frame update
        void Awake()
        {
            isPaused = false;
            canBePaused = true;
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(() => PressButton());
        }
        void PressButton()
        {
            TogglePause();
            menu.SetActive(isPaused);
        }
        public void TogglePause()
        {
            if (!canBePaused && !isPaused)
            {
                return;
            }
            isPaused = !isPaused;
            Time.timeScale = !isPaused ? 1.0f : 0.0f;
        }
        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                PressButton();
            }
        }
    }
}