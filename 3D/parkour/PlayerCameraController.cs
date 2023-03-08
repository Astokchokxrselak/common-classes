using Parkour3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Helpers;
public class PlayerCameraController : MonoBehaviour
{
    private PlayerManager manager;
    public bool setPosition, setRotation;
    public void Enable() => setPosition = setRotation = true;
    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        ResetAngleExtrema();
    }

    private void Update()
    {
        Turn();
        SetCameraPosition();
        SetCameraRotation();
    }

    private static readonly Vector3 DefaultCameraLocalPosition = new(0f, 0.9f), BobbedCameraLocalPosition = new(0f, 0f);
    Vector3 nextCameraPosition;
    Vector3 cameraPosition;
    void SetCameraPosition()
    {
        if (setPosition)
        manager.SetCameraPosition(DefaultCameraLocalPosition);
    }
    void SetCameraRotation()
    {
        if (setRotation)
        manager.SetCameraRotation(manager.head.forward);
    }

    private const float MouseSensitivity = 900f;

    Vector3 rotation;
    float mouseYSign;

    private const float MaxRotationX = 89.99f;
    public float minAngleX = -MaxRotationX, maxAngleX = MaxRotationX;
    public void ResetMinAngleX() => minAngleX = -MaxRotationX;
    public void ResetMaxAngleX() => maxAngleX = MaxRotationX;
    public void ResetAngleExtrema()
    {
        ResetMinAngleX();
        print("Neg: "+ -MaxRotationX);
        ResetMaxAngleX();
    }
    public void Rotate(Vector3 deltaRot)
    {
        rotation += deltaRot;
        if (!MathHelper.Between(rotation.x + 90f, 0, 180))
        {
            rotation.x %= 180f;
        }
        rotation.x = Mathf.Clamp(rotation.x, minAngleX, maxAngleX);
        manager.head.rotation = Quaternion.Euler(rotation);
    }
    void Turn()
    {
        Rotate(MouseSensitivity * Time.deltaTime * new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")));
    }
}
