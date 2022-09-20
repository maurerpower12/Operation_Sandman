// Copyright (c) 2022

namespace Assets.Scenes.Common.Scripts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Implements serveral types of bezier curves movements for GameObjects.
    /// </summary>
    public class MoveObjectToTargetWithArc : MonoBehaviour
    {
        #region Events
        /// <summary>
        /// Event for when the move is donme.
        /// </summary>
        private event EventHandler MoveCompleteEvent;

        /// <summary>
        /// A safe add method for <see cref="MoveCompleteEvent"/>.
        /// </summary>
        public event EventHandler MoveCompelte
        {
            add
            {
                MoveCompleteEvent -= value;
                MoveCompleteEvent += value;
            }
            remove
            {
                MoveCompleteEvent -= value;
            }
        }
        #endregion Events

        #region Public Methods
        /// <summary>
        /// Moves an object from a source to a target with an arc.
        /// </summary>
        /// <param name="objectToMove">The object to move across the screen.</param>
        /// <param name="startPoint">The start point of the movement.</param>
        /// <param name="endPoint">The end point of the mvoement.</param>
        /// <param name="interpolationPoints">The middle interpolation points.</param>
        /// <param name="Duration">Duration of the movement.</param>
        /// <returns>Coroutine.</returns>
        public virtual IEnumerator Move(GameObject objectToMove,
            Vector3 startPoint, Vector3 endPoint,
            List<Vector3> interpolationPoints, float Duration)
        {
            var deltaTime = 0.0f;
            var elapsed = 0.0f;
            var newPosition = new Vector3();

            while(elapsed < Duration)
            {
                var currentTime = elapsed / Duration;

                // 0 < t < 1
                currentTime = Mathf.Clamp(currentTime, 0.0f, 1.0f);

                // See http://www.theappguruz.com/blog/bezier-curve-in-games
                if(interpolationPoints.Count == 0)
                {
                    // Linear Interpolation
                    // P = P0 + t(P1 – P0)
                    newPosition = startPoint + currentTime *
                        (endPoint - startPoint);
                }
                else if(interpolationPoints.Count == 1)
                {
                    // Quadratic Bezier curves
                    // B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2
                    newPosition = Mathf.Pow(1 - currentTime, 2) * startPoint + 2 *
                        (1 - currentTime) * currentTime * interpolationPoints[0] +
                        Mathf.Pow(currentTime, 2) * endPoint;
                }
                else if(interpolationPoints.Count == 2)
                {
                    // Cubic Bezier Cruves
                    // B(t) = (1-t)3P0 + 3(1-t)2tP1 + 3(1-t)t2P2 + t3P3
                    newPosition = Mathf.Pow(1 - currentTime, 2) * startPoint + 2 *
                        (1 - currentTime) * currentTime * interpolationPoints[0] + 3 * (1 - currentTime) *
                        Mathf.Pow(currentTime, 2) * interpolationPoints[1] +
                        Mathf.Pow(currentTime, 2) * endPoint;
                }
                else
                {
                    Debug.LogError("Currently only supports Linear (2), Quadratic (3), and Cubic (4) curves.");
                }

                objectToMove.transform.position = newPosition;

                elapsed += deltaTime;
                deltaTime = Time.deltaTime;
                yield return null;
            }
            objectToMove.transform.position = endPoint;

            MoveCompleteEvent?.Invoke(this, null);
        }
        #endregion Public Methods
    }
}