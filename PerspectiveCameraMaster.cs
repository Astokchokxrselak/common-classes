using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveCameraMaster : MonoBehaviour
{
    private static float targetFieldOfView = 0f;
    private static float defaultFieldOfView;
    public static void ResetFOV() => targetFieldOfView = defaultFieldOfView;
    public static void SetFOV(float nFov) => targetFieldOfView = nFov;
    private void Awake()
    {
        defaultFieldOfView = targetFieldOfView = Camera.main.fieldOfView;
    }

    private const float FieldOfViewZoom = 0.1f;
    void Update()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFieldOfView, FieldOfViewZoom);
    }
}
