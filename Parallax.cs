using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Helpers;
using Common.Extensions;

namespace Common 
{
    public class Parallax : MonoBehaviour
    {
        private enum ParallaxMode
        {
            Follow,
            Stop,
            PingPong,
            MixedFollow
        }
        public Transform focus;
        public Vector3[] points;
        int pointsIndex = 0;
        public bool stopped;
        [SerializeField]
        private ParallaxMode mode;
        private void Start()
        {
            Vector3[] nPoints = new Vector3[points.Length + 1];
            nPoints[0] = transform.position;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].y *= -1;
                nPoints[i + 1] = points[i];
            }
            points = nPoints;
        }

        Timer easingTime = new Timer(0, EasingTime);
        const float EasingTime = 5f;
        private bool isPlaying;
        public void Play()
        {
            isPlaying = true;
        }
        public void Stop()
        {
            isPlaying = false;
        }
        private float scrollSpeed;
        public void SetScrollSpeed(float speed)
        {
            scrollSpeed = speed;
        }
        Vector3 scrollVelocity;
        const float ScrollSpeedMultiplierX = 5f, ScrollSpeedMultiplierY = 3f;
        const float XMultiplier = 4f;
        void FixedUpdate()
        {
            switch (mode)
            {
                case ParallaxMode.Stop:
                    StopMode();
                    break;
                case ParallaxMode.PingPong:
                    PingPongMode();
                    break;
                case ParallaxMode.Follow:
                    FollowMode();
                    break;
            }
        }

        bool HitTarget => transform.position == points[pointsIndex];
        bool ApproachNextPoint(bool increaseIndex)
        {
            var oldPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, points[pointsIndex], easingTime.Ratio * ScrollSpeedMultiplierX * XMultiplier * Time.fixedDeltaTime);
            if (transform.position - oldPosition != Vector3.zero) // if we have not reached our destination yet
            {
                scrollVelocity = (transform.position - oldPosition).normalized; // set our velocity to the direction betweeen our position and our target
            }
            else
            {
                if (increaseIndex) // if we should be going to the next point when we hit out target
                {
                    if (pointsIndex + 1 == points.Length) // if this is the last point
                    {
                        return true;
                    }
                    else
                    {
                        pointsIndex++; // otherwise goto next point
                    }
                }
                else
                {
                    if (pointsIndex - 1 == -1) // if this is the first point
                    {
                        return true;
                    }
                    else
                    {
                        pointsIndex--;  // otherwise goto last point
                    }
                }
            } return false;
        }
        #region Stop Mode Logic
        void StopMode()
        {
            if (!stopped)
            {
                StopModeNormal();
            }
            else
            {
                StopModeLast();
            }
        }
        void StopModeNormal()
        {
            EaseTimer();
            stopped = ApproachNextPoint(true);
        }
        void StopModeLast()
        {
            SlowTime(); // reduce the velocity multilpier/easingtime ratio
            transform.position += Vector3.Lerp(Vector2.zero, scrollVelocity, easingTime.Ratio); // as easingtime ratio decreases from 1, scrollVelocity approaches zero
        }
        #endregion
        #region Ping Pong Mode Logic
        void PingPongMode()
        {
            if (!stopped)
            {
                PingPongModeForward();
            }
            else
            {
                PingPongModeBackward();
            }
        }
        void PingPongModeForward()
        {
            EaseTimer();
            bool done = ApproachNextPoint(true);
            if (done) // if we hit the last point
            {
                pointsIndex--; // set target to the second to last point
                stopped = true;
            }
        }
        void PingPongModeBackward()
        {
            EaseTimer();
            bool done = ApproachNextPoint(false);
            if (done) // if we hit the first point again
            {
                pointsIndex++; // set target to the second to last point
                stopped = false;
            }
        }
        #endregion
        #region Follow Mode Logic
        void FollowMode()
        {
            FollowSetScrollVelocity();
            transform.position += scrollVelocity * Time.fixedDeltaTime;
        }
        void FollowSetScrollVelocity()
        {
            EaseTimer();
            var displacement = -CameraHelper.DisplacementFromCamera(focus.position).x;
            // dispalcement is negative
            displacement = Mathf.Clamp(displacement, -CameraHelper.WorldWidth() / 2f, CameraHelper.WorldWidth() / 2f) / (CameraHelper.WorldWidth() / 2f);
            scrollVelocity = new Vector3(displacement * ScrollSpeedMultiplierX, ScrollSpeedMultiplierY * -easingTime.Ratio);
        }
        #endregion
        /*#region Mixed Follow Mode Logic
        void MixedFollowMode()
        {
            MixedFollowAdjustPosition();
            StopMode(); // Mix of stop mode and follow mode
            // Parallax shifts perpendicular to direction of motion
        }
        void MixedFollowAdjustPosition()
        {
            var displacement = -CameraHelper.DisplacementFromCamera(focus.position).x;
            // dispalcement is negative
            displacement = Mathf.Clamp(displacement, -CameraHelper.WorldWidth() / 2f, CameraHelper.WorldWidth() / 2f) / (CameraHelper.WorldWidth() / 2f);
            transform.position += (Vector3)Vector2.Perpendicular(scrollVelocity).normalized * displacement * ScrollSpeedMultiplierX * Time.fixedDeltaTime;
        }
        #endregion*/
        void EaseTimer()
        {
            easingTime.ClampedIncrement();
        }
        void SlowTime()
        {
            easingTime.ClampedDecrement();
        }
    }
}