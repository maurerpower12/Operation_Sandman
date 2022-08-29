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
        #endregion Events

        #region Methods
        /// <summary>
        /// Called when the collision occurs on this object.
        /// </summary>
        /// <param name="collision">Object involved in this collision.</param>
        public void OnCollisionEnter(Collision collision)
        {
            var data = new PitchResultData();
            // For debug purposes, draw a ray at the collision point.
            var contact = collision.GetContact(0);
            Debug.DrawRay(contact.point, contact.normal, Color.green);
            data.Point = contact.point;
            data.Normal = contact.normal;

            collision.gameObject.SetActive(false);
            StrikeEvent?.Invoke(this, data);
            StopAllCoroutines();
            StartCoroutine(DisplayLastPitch(data));
        }

        /// <summary>
        /// Displays a prefab at the last pitch location.
        /// </summary>
        /// <param name="data">Result data for the last pitch.</param>
        /// <returns>Coroutine.</returns>
        protected virtual IEnumerator DisplayLastPitch(PitchResultData data)
        {
            var baseballObject = Instantiate(BaseballDecalPrefab, data.Point,
                                       Quaternion.identity, this.transform);
            BaseballDecals.Add(baseballObject);
            yield return new WaitForSeconds(0.5f);
            BaseballDecals.ForEach(ball => Destroy(ball));
            BaseballDecals.Clear();
        }
        #endregion Methods
    }
}