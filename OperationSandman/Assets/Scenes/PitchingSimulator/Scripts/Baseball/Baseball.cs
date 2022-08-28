// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Baseball
{
    using System;
    using UnityEngine;

    public class Baseball : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        protected bool Pitch = false;

        [SerializeField]
        protected Rigidbody Rigidbody;
        #endregion Fields

        #region Public Methods
        /// <summary>
        /// Throws the ball toward the desired location.
        /// </summary>
        public virtual void Throw(PitchData pitchData)
        {
            Rigidbody.useGravity = true;
            if(pitchData != null)
            {
                Rigidbody.AddForce(pitchData.GetForceVector(), ForceMode.Impulse);
                Rigidbody.AddTorque(pitchData.GetTorqueVector(), ForceMode.Impulse);
            }
        }

        /// <summary>
        /// This is the "get the sign" state where the ball is inactive.
        /// </summary>
        public virtual void Hold()
        {
            Rigidbody.useGravity = false;
        }
        #endregion Public Methods

        #region Protected Methods
        /// <summary>
        /// Called on Unity Awake.
        /// </summary>
        protected virtual void Awake()
        {
            Hold();
        }
        #endregion Protected Methods
    }
}