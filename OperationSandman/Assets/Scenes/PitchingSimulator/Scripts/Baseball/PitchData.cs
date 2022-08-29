// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Baseball
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PitchData
    {
        [SerializeField]
        public PitchType PitchType;

        [SerializeField]
        protected Vector3 Speed;

        [SerializeField]
        protected Vector3 Torque;

        public Vector3 GetForceVector()
        {
            return Speed;
        }

        public Vector3 GetTorqueVector()
        {
            return Torque;
        }
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
        Forkball,
        Screwball,
        Knuckleball,
        Eephus
    }
}