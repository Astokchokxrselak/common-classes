using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Helpers;
public class CameraWobble : MonoBehaviour
{
    public float radius = 1f, speedDivisor = 1f;
    private Vector3 CameraOffset;
    private void Start()
    {
        _origin = Camera.main.transform.localPosition;
    }

    public void Update()
    {
        Wobble();
        AngleWobble();
    }

    Vector3 _origin;
    void Wobble()
    {
        CameraOffset = (Vector3)MathHelper.UnitCircle(Time.time / speedDivisor) * radius;        
        transform.localPosition = _origin + CameraOffset; // Rotate around origin
    }

    public float maxAngle = 1f;
    void AngleWobble()
    {
        var angle = Mathf.Sin(Mathf.PI / 4 + 2 * Time.time / speedDivisor) * maxAngle;
        transform.localEulerAngles = Vector3.forward * angle;
    }
}
