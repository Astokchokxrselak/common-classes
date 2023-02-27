using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    [AddComponentMenu("UI/On Button/Move Object On Button")]
    // This class is used by buttons to move objects upon click.
    public class ButtonMoveObject : OnButton // Inherits from onbutton and overrides the onbuttonclick method.
    {
        public Transform m_movedObject; // The transform of the object we are moving.
        public Vector3 firstPosition, secondPosition; // The first position, where the object starts, and the second position, where the object moves to when the button is clicked.
        Vector3 targetPosition; // The position we are moving the object towards; will be either firstPosition or secondPosition.
        public float lerpAcceleration = 0.15f; // The t value of the lerp we update every frame.
        public override void OnButtonClick() // What happens when the button is clicked.
        {
            targetPosition = targetPosition == secondPosition // If the button has been clicked...
                           ? firstPosition // reset the targetPosition back to firstPosition.
                           : secondPosition; // Else, set the targetPosition to secondPosition.
        }
        public override void OnInitialize()
        {
            targetPosition = firstPosition; // Initialize targetPosition to firstPosition.
        }
        private void Update()
        {
            if (m_movedObject is RectTransform rectTransform) // If m_movedObject is a ui element...
            {
                rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, targetPosition, lerpAcceleration); // Lerp anchoredPosition towards the current targetPosition. 
            }
            else // If m_movedObject is not a ui element...
            {
                m_movedObject.position = Vector3.Lerp(m_movedObject.position, targetPosition, lerpAcceleration); // Lerp position towards the current targetPosition.
            }
        }
        
    }
}