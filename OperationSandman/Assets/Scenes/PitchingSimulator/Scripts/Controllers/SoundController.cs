// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    /// <summary>
    /// Provides an interface for playing sounds.
    /// </summary>
    public class SoundController : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// Represents all of the audio data for the game.
        /// </summary>
        [SerializeField]
        protected List<AudioClipData> AudioData = new List<AudioClipData>();
        #endregion Fields

        #region Methods
        /// <summary>
        /// Called on Unity's Awake.
        /// </summary>
        protected virtual void Awake()
        {
            Play(SoundEnum.UmpirePlayBall);
        }

        /// <summary>
        /// Plays the passed audio clip
        /// </summary>
        public void Play(SoundEnum soundEnum)
        {
            var sfx = AudioData.FirstOrDefault(item => item.SoundEnum == soundEnum);
            if(sfx != null)
            {
                sfx.AudioSource.Play();
            }
            else
            {
                Debug.LogError($"SoundController: Unable to sfx {soundEnum}");
            }
        }
        #endregion Methods
    }

    #region Enum
    /// <summary>
    /// Enum for each possible sfx.
    /// </summary>
    public enum SoundEnum
    {
        UmpirePlayBall = 0,
        UmpireBall,
        UmpireStrike,
        UmpireOut,
        CrowdCheer,
        CrowdBoo,
        PitchSFX
    }
    #endregion Enum

    #region Internal Classes
    [Serializable]
    public class AudioClipData
    {
        public SoundEnum SoundEnum;

        public AudioSource AudioSource;
    }
    #endregion Internal Classes
}