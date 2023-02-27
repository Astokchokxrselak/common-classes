using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Helpers;

public class LerpDisplacement : MonoBehaviour
{
    float minPositionXBeforeLerp;
    public Transform focus;
    public float viewportDisplacementFromCamera = 0.2f;
   
    float? offset = null;
    public bool Active => offset != null;
    // Update is called once per frame
    void Update()
    {
        var pos = Camera.main.ViewportToWorldPoint(viewportDisplacementFromCamera * Vector2.right);
        if (focus.position.x > pos.x)
        {
            if (offset == null)
            {
                offset = transform.position.x - focus.position.x;
            }
            transform.position = focus.position + new Vector3(offset.Value, 0, -10);
        }
        else
        {
            offset = null;
        }
    }
}
