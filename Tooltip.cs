using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Helpers;

namespace Common
{
    public class Tooltip : MonoBehaviour
    {
        RectTransform box;
        // Start is called before the first frame update
        void Start()
        {
            box = transform.Find("TooltipBox") as RectTransform;
        }
        internal Vector3[] boxCorners = new Vector3[4];
        Vector3 BottomLeft => boxCorners[0];
        Vector3 TopLeft => boxCorners[1];
        Vector3 TopRight => boxCorners[2];
        Vector3 BottomRight => boxCorners[3];
        // Update is called once per frame
        public void UpdateBox()
        {
            box.anchoredPosition = InputHelper.CenteredMousePosition() + new Vector2(-box.sizeDelta.x, box.sizeDelta.y) / 2f;
            box.GetWorldCorners(boxCorners);
            FitWithinCamera(BottomLeft);
            FitWithinCamera(TopLeft);
            FitWithinCamera(TopRight);
            FitWithinCamera(BottomRight);
        }
        public bool Enabled
        {
            set => gameObject.SetActive(value);
        }
        void FitWithinCamera(Vector2 point)
        {
            var offscreen = CameraHelper.IsOffscreen(Camera.main, point);
            if (offscreen.x != 0)
            {
                box.anchoredPosition = new Vector2(offscreen.x * (CameraHelper.Resolution().x - box.sizeDelta.x) / 2f, box.anchoredPosition.y);
            }
            if (offscreen.y != 0)
            {
                box.anchoredPosition = new Vector2(box.anchoredPosition.x, offscreen.y * (CameraHelper.Resolution().y - box.sizeDelta.y) / 2f);
            }
        }
    }
}