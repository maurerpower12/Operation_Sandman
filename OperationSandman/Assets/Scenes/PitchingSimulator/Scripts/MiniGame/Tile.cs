// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.MiniGame
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tile : MonoBehaviour
    {
        #region Fields
        public SpriteRenderer TileSpriteRenderer;
        #endregion Fields

        #region Events
        #endregion Events

        #region Properties
        #endregion Properties

        #region Methods
        /// <summary>
        /// Called of Unity Awake.
        /// </summary>
        protected void Awake()
        {
            if(TileSpriteRenderer == null)
            {
                Debug.LogError("Tile script requires a SpriteRenderer");
            }
        }

        /// <summary>
        /// Called of Unity Start.
        /// </summary>
        protected void Start()
        {
        }
        #endregion Methods
    }
}