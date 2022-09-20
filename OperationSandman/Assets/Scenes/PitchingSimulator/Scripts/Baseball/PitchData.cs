// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Baseball
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The encapsulated data for a specific pitch type.
    /// </summary>
    [Serializable]
    public class PitchData : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The type of this pitch. i.e. Fastball, Changeup, etc.
        /// </summary>
        [SerializeField]
        public PitchType PitchType;

        /// <summary>
        /// The end point that we will hit.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public Vector3 EndPoint;

        /// <summary>
        /// The middle points along the movement.
        /// </summary>
        [SerializeField]
        public List<GameObject> InterpolationPoints;

        /// <summary>
        /// The duration (in seconds) it will take the ball to get to the <see cref="EndPoint"/>
        /// </summary>
        [SerializeField]
        [Range(0.01f, 10.0f)]
        public float Duration = 1.0f;

        /// <summary>
        /// The sprite that represents this pitch type.
        /// </summary>
        [SerializeField]
        public Sprite PitchSprite;

        /// <summary>
        /// The delta positon that moves with <see cref="EndPoint"/>
        /// </summary>
        [NonSerialized]
        protected List<Vector3> DeltaInterpolationPoint;
        #endregion Fields

        #region Properties
        #endregion Properties

        #region Methods
        /// <summary>
        /// Called on Unity Awake.
        /// </summary>
        protected void Awake()
        {
            DeltaInterpolationPoint = new List<Vector3>();
            foreach(var point in InterpolationPoints)
            {
                DeltaInterpolationPoint.Add(point.transform.position);
            }
        }

        /// <summary>
        /// Needed to generate the pitch data for the resulting EndPosition
        /// </summary>
        /// <param name="endPoint">The end point of the pitch.</param>
        public void GeneratePitchData(GameObject endPoint)
        {
            EndPoint = endPoint.transform.position;
            for(var index = 0; index < DeltaInterpolationPoint.Count; index++)
            {
                InterpolationPoints[index].transform.position = endPoint.transform.position + DeltaInterpolationPoint[index];
            }
        }
        #endregion Methods
    }
}