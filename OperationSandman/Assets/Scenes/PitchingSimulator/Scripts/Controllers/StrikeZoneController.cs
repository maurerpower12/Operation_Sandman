// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Baseball;
    using UnityEngine;

    public class StrikeZoneController : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The small baseball to display in the strick zone after a pitch was thrown.
        /// </summary>
        [SerializeField]
        protected GameObject BaseballDecalPrefab;

        /// <summary>
        /// A collection of baseball decals shown for a strike.
        /// </summary>
        private List<GameObject> BaseballDecals = new List<GameObject>();

        /// <summary>
        /// The collider that defines the strike zone.
        /// </summary>
        /// <remarks>No collider events are used.</remarks>
        [SerializeField]
        protected Collider Collider;
        #endregion Fields

        #region Events
        /// <summary>
        /// Event for when the strike has occured.
        /// </summary>
        private event EventHandler<PitchResultData> StrikeEvent;

        /// <summary>
        /// A safe add method for <see cref="StrikeEvent"/>.
        /// </summary>
        public event EventHandler<PitchResultData> Strike
        {
            add
            {
                StrikeEvent -= value;
                StrikeEvent += value;
            }
            remove
            {
                StrikeEvent -= value;
            }
        }

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
        public void OnPitchFinished(Vector3 contactPoint)
        {
            var data = new PitchResultData
            {
                Point = contactPoint
            };
            var pitchCall = IsPitchWithinStrikeZone(contactPoint);
            if(pitchCall)
            {
                StrikeEvent?.Invoke(this, data);
            }
            else
            {
                BallEvent?.Invoke(this, EventArgs.Empty);
            }

            StopAllCoroutines();
            StartCoroutine(DisplayLastPitch(data));
        }

        /// <summary>
        /// Displays a prefab at the last pitch location.
        /// </summary>
        /// <param name="data">Result data for the last pitch.</param>
        /// <returns>Coroutine.</returns>
        protected IEnumerator DisplayLastPitch(PitchResultData data)
        {
            var baseballObject = Instantiate(BaseballDecalPrefab, data.Point,
                                       Quaternion.identity, this.transform);
            BaseballDecals.Add(baseballObject);
            yield return new WaitForSeconds(0.5f);
            BaseballDecals.ForEach(ball => Destroy(ball));
            BaseballDecals.Clear();
        }

        /// <summary>
        /// Determines if the pitch is pointing a valid direction.
        /// </summary>
        /// <returns>True if it is within bounds. Else false.</returns>
        private bool IsPitchWithinStrikeZone(Vector3 contactPoint)
        {
            return Collider.bounds.Contains(contactPoint);
        }
        #endregion Methods
    }
}