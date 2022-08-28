// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Baseball
{
    using System;
    using UnityEngine;

    public class Baseball : MonoBehaviour
    {
        [SerializeField]
        protected bool Pitch = false;

        [SerializeField]
        protected Rigidbody Rigidbody;

        [SerializeField]
        protected float Thrust = 20f;

        protected virtual void Update()
        {
            if (Pitch)
            {
                Rigidbody.AddForce(transform.up * Thrust);
                Pitch = false;
            }
        }
    }
}