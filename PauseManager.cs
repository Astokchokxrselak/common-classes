using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager Instance;
    public GameObject menu;

    private static bool _isPaused;
    public static bool IsPaused => _isPaused;

    public static bool canBePaused;
    private static float timescaleAtPause = 1f;
    public static readonly KeyCode pauseKey = KeyCode.Escape;
    public static event Action OnPaused, OnUnpaused;
    // Start is called before the first frame update
    private void Start()
    {
        timescaleAtPause = Time.timeScale;
        Instance = this;

        OnPaused += PauseAllUpdated;
        OnUnpaused += UnpauseAllUpdated;

        canBePaused = true;

        SetPause(!menu.activeInHierarchy);
    }
    private static void PauseAllUpdated()
    {
        MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour mono in sceneActive)
        {
            var attribute = Attribute.GetCustomAttribute(mono.GetType(), typeof(PauseUpdateAttribute));
            if (attribute != null)
            {
                mono.enabled = false;
            }
        }
    }
    private static void UnpauseAllUpdated()
    {
        MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour mono in sceneActive)
        {
            var attribute = Attribute.GetCustomAttribute(mono.GetType(), typeof(PauseUpdateAttribute));
            if (attribute != null)
            {
                mono.enabled = true;
            }
        }
    }
    public static void TogglePause()
    {
        if (!canBePaused && !_isPaused)
        {
            return;
        }
        if (_isPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }
    public static void SetPause(bool isPaused)
    {
        if (isPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }
    public static void Pause()
    {
        PauseAllUpdated();
        _isPaused = true;
        Instance.menu.SetActive(true);
        OnPaused?.Invoke();
        timescaleAtPause = Time.timeScale;
        Time.timeScale = 0f;
    }
    public static void Unpause()
    {
        UnpauseAllUpdated();
        _isPaused = false;
        Instance.menu.SetActive(false);
        OnUnpaused?.Invoke();
        Time.timeScale = timescaleAtPause;
    }
}