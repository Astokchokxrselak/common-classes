using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using MyBox;

namespace Common.UI
{

    public class HPBarController : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject focus;
        IDamageable damageable;
        Image barImage;
        Bar bar;
        public bool update, disableOnStart = false;
        public bool coolParticlesOn;

        public Color fullColor, emptyColor;

//        [ConditionalField("coolParticlesOn")]
        public Vector3 particleSystemPositionLeft;
//        [ConditionalField("coolParticlesOn")]
        public Vector3 particleSystemPositionRight;

        void Awake()
        {
            AttachToFocus();
            if (coolParticlesOn)
            {
                CreateCoolParticleEffect();
            }
            UpdateBar(damageable);
        }
        void AttachToFocus()
        {
            barImage = GetComponentInChildren<Image>();
            if (barImage == null)
            {
                throw new System.ArgumentNullException("Bar Image is null on object " + name);
            }
            this.bar = new Bar(barImage);
            this.damageable = focus.GetComponent<IDamageable>();
            if (this.damageable == null)
            {
                throw new System.NullReferenceException("No IDamageable is attached to focus");
            }
            if (disableOnStart)
            {
                HideBar();
            }
        }

        private ParticleSystem m_coolPS;
        void CreateCoolParticleEffect()
        {
            var psGameObject = new GameObject("HPBarParticleSystem", typeof(RectTransform));
            m_coolPS = psGameObject.AddComponent<ParticleSystem>();

            var psRect = psGameObject.transform as RectTransform;
            psRect.SetParent(this.transform, false);
            psRect.anchoredPosition = Vector2.right * (-barImage.rectTransform.sizeDelta.x / 2f + barImage.rectTransform.sizeDelta.x * bar.Ratio);
            InitializeParticleSystem(); 
            Vector3 worldCenter = barImage.rectTransform.localPosition;
        }
        void InitializeParticleSystem()
        {
            InitializeEmission();
        }

        const float ShapeRadiusMultiplier = 16.5f / 30f;
        const float EmissionRate = 100f;
        void InitializeEmission()
        {
            var shape = m_coolPS.shape;
            shape.rotation = Vector3.up * 90;
            shape.radius = ShapeRadiusMultiplier * barImage.rectTransform.sizeDelta.y;

            var emission = m_coolPS.emission;
            emission.rateOverTime = EmissionRate;
        }
        void UpdateParticleSystemPosition()
        {
            var psRect = m_coolPS.transform as RectTransform;
            psRect.SetParent(this.transform, false);
            Vector3 worldCenter = barImage.rectTransform.position;
            Vector3 direction = Camera.main.ScreenToWorldPoint(worldCenter);
            print(direction);
//            psRect.anchoredPosition = Vector2.right * (-barImage.rectTransform.sizeDelta.x / 2f + barImage.rectTransform.sizeDelta.x * bar.Ratio);
        }
        private void Update()
        {
            if (update)// && damageable.Health / damageable.MaxHealth != bar.Ratio)
            UpdateBar(damageable);
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
        public void UpdateBar(float health, float maxHealth)
        {
            bar.Ratio = health / maxHealth;
        }
        public void UpdateBar(IDamageable reference)
        {
            bar.Ratio = reference.Health / reference.MaxHealth;
            bar.bar.color = Color.Lerp(emptyColor, fullColor, bar.Ratio);
        }
    }
}