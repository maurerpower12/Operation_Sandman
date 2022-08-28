// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System;
    using System.Collections.Generic;
    using Baseball;
    using UnityEngine;

    public class PitchController : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        public List<PitchData> Pitches;

        [SerializeField]
        protected GameObject BaseballPrefab;

        [SerializeField]
        protected Transform BaseballStartingPosition;
        #endregion Fields


        #region Properties
        /// <summary>
        /// The currently selected pitch index in <see cref="Pitches"/>.
        /// </summary>
        /// <remarks>Zero based index.</remarks>
        public int PitchTypeIndex { get; set; }
        #endregion Properties

        #region Events
        /// <summary>
        /// Event for when the pitch sequence has finished.
        /// </summary>
        private event EventHandler PitchCompleteEvent;

        /// <summary>
        /// A safe add method for <see cref="PitchCompleteEvent"/>.
        /// </summary>
        public event EventHandler PitchComplete
        {
            add
            {
                PitchCompleteEvent -= value;
                PitchCompleteEvent += value;
            }
            remove
            {
                PitchCompleteEvent -= value;
            }
        }
        #endregion Events

        #region Methods
        public virtual void ThrowPitch()
        {
            var baseballObject = Instantiate(BaseballPrefab,
                                       BaseballStartingPosition.position,
                                       Quaternion.identity, this.transform);
            var pitchData = Pitches[0];

            if(baseballObject != null)
            {
                var baseballScript = baseballObject.GetComponent<Baseball>();
                if(baseballScript != null)
                {
                    baseballScript.Throw(pitchData);
                }
                else
                {
                    Debug.LogError("PitchController: Baseball prefab does not have a Baseball script attached. Please fix.");
                }
            }
            else
            {
                Debug.LogError("PitchController: Unable to Instantiate a baseball.");
            }
        }
        #endregion Methods
    }
}