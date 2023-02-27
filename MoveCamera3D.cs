using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Helpers;
using Common.Extensions;

public class MoveCamera3D : MonoBehaviour
{
    public float sensitivity;
    // Update is called once per frame
    void Update()
    {
        ListenForArrowKeys();
        PanCameraMouse();
        UpdateMouseDelta();
    }
    private void FixedUpdate()
    {
        TryMove();
    }
    Vector3 moveRay;
    void ListenForArrowKeys()
    {
        moveRay = transform.rotation * InputHelper.Get3DInput().normalized * sensitivity;
        print(moveRay);
    }

    public float wallOffset;
    void TryMove()
    {
        if (Physics.Raycast(transform.position, moveRay, out var cast, sensitivity))
        {
            transform.position = cast.point - wallOffset * moveRay.normalized;
            print("!");
        }
        else
        {
            transform.position += moveRay;
        }
    }

    public bool invertedMousePan;
    public float mouseSensitivity;
    Vector2 oldMouse;
    Vector3 deltaMouse;
    void PanCameraMouse()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        if (Input.GetMouseButton(0))
        {
            if (invertedMousePan)
            {
                eulerAngles -= deltaMouse * mouseSensitivity;
            }
            else
            {
                eulerAngles += deltaMouse * mouseSensitivity;
            }
        }
        transform.eulerAngles = eulerAngles;
    }
    void UpdateMouseDelta()
    {
        deltaMouse = Input.mousePosition - (Vector3)oldMouse;
        deltaMouse.Set(-deltaMouse.y, deltaMouse.x, 0);
        oldMouse = Input.mousePosition;
        print("Delta mouse: " + deltaMouse);
    }
}
