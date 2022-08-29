// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System;
    using UnityEngine;

    public class BackstopController : MonoBehaviour
    {
        #region Fields
        #endregion Fields

        #region Events
        /// <summary>
        /// Event for when the strike has occured.
        /// </summary>
        private event EventHandler BallEvent;

        /// <summary>
        /// A safe add method for <see cref="BallEvent"/>.
        /// </summary>
        public event EventHandler Ball
        {
            add
            {
                BallEvent -= value;
                BallEvent += value;
            }
            remove
            {
                BallEvent -= value;
            }
        }
        #endregion Events

        #region Methods
        /// <summary>
        /// Called when the collision occurs on this object.
        /// </summary>
        /// <param name="collision">Object involved in this collision.</param>
        public void OnCollisionEnter(Collision collision)
        {
            // For debug purposes, draw a ray at the collision point.
            foreach(ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.red);
            }

            BallEvent?.Invoke(this, EventArgs.Empty);
        }
        #endregion Methods
    }
}