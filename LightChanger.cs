using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.Universal;

public class ChristmasLights : MonoBehaviour
{
    Light2D m_Light2D;
    private void Awake()
    {
        m_Light2D = GetComponent<Light2D>();
        lightIndex = frameCounter = 0;
    }

    [System.Serializable]
    public struct LightState
    {
        public float intensity;
        public Color color;
        public void Set(Light2D light)
        {
            light.intensity = intensity;
            light.color = color;
        }
    }
    public enum LightMode
    {
        Random,
        Linear,
        PingPong
    }
    public LightMode lightMode;
    public int frameDurationPerState = 120;
    public LightState[] lightStates;
    int lightIndex = 0, frameCounter;

    bool pingPonged;
    void FixedUpdate()
    {
        if (frameCounter++ >= frameDurationPerState)
        {
            frameCounter = 0;
            lightStates[lightIndex].Set(m_Light2D);
            switch (lightMode)
            {
                case LightMode.Random:
                    lightIndex = Random.Range(0, lightStates.Length);
                    break;
                case LightMode.Linear:
                    lightIndex++;
                    if (lightIndex >= lightStates.Length)
                    {
                        lightIndex = 0;
                    }
                    break;
                case LightMode.PingPong:
                    if (!pingPonged)
                    {
                        lightIndex++;
                        if (lightIndex >= lightStates.Length)
                        {
                            pingPonged = true;
                            lightIndex--;
                        }
                    }
                    else
                    {
                        lightIndex--;
                        if (lightIndex < 0)
                        {
                            pingPonged = false;
                            lightIndex++;
                        }
                    }
                    break;
            }
        }
    }
}
