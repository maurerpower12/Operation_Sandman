// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System;
    using System.Collections.Generic;
    using Common.Scripts;
    using Baseball;
    using UnityEngine;

    /// <summary>
    /// Controlls the pitch sequence and generating the baseballs.
    /// </summary>
    public class PitchController : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The types of pitches this pitcher has.
        /// </summary>
        [SerializeField]
        public List<PitchData> Pitches;

        /// <summary>
        /// The baesball prefab that the pitch will throw.
        /// </summary>
        [SerializeField]
        protected GameObject BaseballPrefab;

        /// <summary>
        /// The starting position of the throw.
        /// </summary>
        [SerializeField]
        protected Transform BaseballStartingPosition;

        /// <summary>
        /// The cursor of where the player is trying to throw.
        /// </summary>
        [SerializeField]
        protected CursorFollow Cursor;

        /// <summary>
        /// Dynamic list of all of the baseballs we have in the scene.
        /// </summary>
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
                    pitchData.GeneratePitchData(Cursor.gameObject);
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
        public void CleanUpPitches()
        {
            InstantiatedBaseballs.ForEach(ball => Destroy(ball));
            InstantiatedBaseballs.Clear();
        }
        #endregion Methods
    }
}