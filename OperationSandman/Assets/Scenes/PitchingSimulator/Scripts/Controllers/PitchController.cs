// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System;
    using System.Collections.Generic;
    using Common.Scripts;
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

        [SerializeField]
        protected CursorFollow Cursor;

        [NonSerialized]
        protected List<GameObject> InstantiatedBaseballs = new List<GameObject>();
        #endregion Fields

        #region Properties
        /// <summary>
        /// The currently selected pitch index in <see cref="Pitches"/>.
        /// </summary>
        /// <remarks>Zero based index.</remarks>
        public int PitchTypeIndex { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Instantates a baseball and throws it.
        /// </summary>
        public virtual void ThrowPitch()
        {
            // This is a design decision but only ever allow one ball in play.
            CleanUpPitches();
            var finalStartingPosition = new Vector3(
                        Cursor.gameObject.transform.position.x,
                        Cursor.gameObject.transform.position.y,
                        BaseballStartingPosition.gameObject.transform.position.z);

            var baseballObject = Instantiate(BaseballPrefab,
                                       finalStartingPosition,
                                       Quaternion.identity, this.transform);
            var pitchData = Pitches[PitchTypeIndex];

            if(baseballObject != null)
            {
                InstantiatedBaseballs.Add(baseballObject);
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

        /// <summary>
        /// Deletes any instantiated baseballs.
        /// </summary>
        public virtual void CleanUpPitches()
        {
            InstantiatedBaseballs.ForEach(ball => Destroy(ball));
            InstantiatedBaseballs.Clear();
        }
        #endregion Methods
    }
}