using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public readonly struct Bar
    {
        readonly Transform parent;
        public readonly Image bar;
        public Bar(Image bar)
        {
            this.parent = bar.transform.parent;
            this.bar = bar;
        }
        public void Hide()
        {
            parent.gameObject.SetActive(false);
        }
        public void Show()
        {
            parent.gameObject.SetActive(true);
        }

        public float Ratio
        {
            get => bar.fillAmount;
            set
            {
                bar.fillAmount = value;
            }
        }
    }

    public class BarController : MonoBehaviour
    {
        protected Bar bar;
        public Image barImage;
        public Color fullColor, emptyColor;
        // Start is called before the first frame update
        void Awake()
        {
            bar = new Bar(barImage);
        }

        public void ShowBar()
        {
            bar.Show();
        }
        public void ShowBarFancy(float time)
        {
            ShowBar();
            if (!isLoadingBar)
            StartCoroutine(LoadBar(time));
        }

        bool isLoadingBar = false;
        private IEnumerator LoadBar(float time)
        {
            isLoadingBar = true;
            for (float i = 0; i < time; i += Time.deltaTime)
            {
                bar.Ratio = i / time;
                yield return null;
            }
            isLoadingBar = false;
        }
        public void HideBar()
        {
            bar.Hide();
        }

        // Update is called once per frame
        public void UpdateBar(float current, float max)
        {
            bar.Ratio = current / max;
            bar.bar.color = Color.Lerp(emptyColor, fullColor, bar.Ratio);
        }
        public void UpdateBar(IDamageable reference)
        {
            bar.Ratio = reference.Health / reference.MaxHealth;
            bar.bar.color = Color.Lerp(emptyColor, fullColor, bar.Ratio);
        }
    }
}