// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Baseball
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Represents the resulting data from the last pitch.
    /// </summary>
    [Serializable]
    public class PitchResultData
    {
        [SerializeField]
        public PitchType PitchType;

        [SerializeField]
        public Vector3 Point;

        [SerializeField]
        public Vector3 Normal;
    }
}