// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Baseball
{
    using System.Linq;
    using Common.Scripts;
    using UnityEngine;

    public class Baseball : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The rigidboday that is attached to the baseball.
        /// </summary>
        [SerializeField]
        protected Rigidbody Rigidbody;

        /// <summary>
        /// A reference to the script that will move the baseball.
        /// </summary>
        [SerializeField]
        protected MoveObjectToTargetWithArc MoveObjectToTargetWithArc;
        #endregion Fields

        #region Public Methods
        /// <summary>
        /// Throws the ball toward the desired location.
        /// </summary>
        /// <param name="pitchData">The data needed to throw the pitch.</param>
        public void Throw(PitchData pitchData)
        {
            Rigidbody.useGravity = false;
            if(pitchData != null)
            {
                if(MoveObjectToTargetWithArc)
                {
                    StartCoroutine(MoveObjectToTargetWithArc.Move(gameObject,
                        gameObject.transform.position, pitchData.EndPoint,
                        pitchData.InterpolationPoints.Select(point => point.transform.position).ToList(),
                        pitchData.Duration));
                }
                else
                {
                    Debug.LogError("Unable to get move object");
                }
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