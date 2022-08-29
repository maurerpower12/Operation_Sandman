// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Baseball
{
    using System;
    using UnityEngine;

    /// <summary>
    /// The encapsulated data for a specific pitch type.
    /// </summary>
    [Serializable]
    public class PitchData
    {
        #region Fields
        [SerializeField]
        public PitchType PitchType;

        [SerializeField]
        protected Vector3 Speed;

        [SerializeField]
        protected ForceMode SpeedMode = ForceMode.Impulse;

        [SerializeField]
        protected Vector3 Torque;

        [SerializeField]
        protected ForceMode TorqueMode = ForceMode.Impulse;
        #endregion Fields

        #region Properties
        public Vector3 ForceVector { get { return Speed; } }

        public Vector3 TorqueVector { get { return Torque; } }

        public ForceMode SpeedForceMode { get { return SpeedMode; } }

        public ForceMode TorqueForceMode { get { return TorqueMode; } }
        #endregion Properties
    }

    public enum PitchType
    {
        FourSeamFastball = 0,
        TwoSeamFastball,
        Sinker,
        Cutter,
        Changeup,
        Curveball,
        KnuckleCurve,
        Slider,
        Slurve,
        Spiltter,
        Forkball,
        Screwball,
        Knuckleball,
        Eephus
    }
}