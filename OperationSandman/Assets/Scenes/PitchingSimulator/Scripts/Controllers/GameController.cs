// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Baseball;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The main game controller with various responsibilities including:
    /// - Controlling the HUD
    /// - Processing the ball and strikes (umpire logic)
    /// </summary>
    public class GameController : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// Reference to the pitch controller for the game.
        /// </summary>
        [SerializeField]
        protected PitchController PitchController;

        /// <summary>
        /// Reference to the strike zone controller for the game.
        /// </summary>
        [SerializeField]
        protected StrikeZoneController StrikeZoneController;

        /// <summary>
        /// Reference to the back stop controller for the game.
        /// </summary>
        [SerializeField]
        protected BackstopController BackstopController;

        /// <summary>
        /// Reference to the sound controller for the game.
        /// </summary>
        [SerializeField]
        protected SoundController SoundController;

        /// <summary>
        /// This is the dropdown of pitch types.
        /// </summary>
        [SerializeField]
        protected TMP_Dropdown PitchTypeDropdown;

        /// <summary>
        /// GameObject to show when they throw a strike.
        /// </summary>
        [SerializeField]
        protected GameObject StrikeTextGameObject;

        /// <summary>
        /// GameObject to show when they throw a ball.
        /// </summary>
        [SerializeField]
        protected GameObject BallTextGameObject;

        /// <summary>
        /// This is the dropdown of pitch types.
        /// </summary>
        [SerializeField]
        protected TMP_Text CountText;

        /// <summary>
        /// This is the ptich breakdown displayed on the HUD.
        /// </summary>
        [SerializeField]
        protected Image PitchBreakdown;

        /// <summary>
        /// The number of balls thrown in the current count.
        /// </summary>
        [SerializeField]
        private int NumberOfBallsInCurrentCount = 0;

        /// <summary>
        /// The number of strikes thrown in the current count.
        /// </summary>
        [SerializeField]
        private int NumberOfStrikesInCurrentCount = 0;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Called on Unity Start.
        /// </summary>
        protected virtual void Start()
        {
            PopulatePitchTypeDropdown();
            StrikeZoneController.Strike += OnStrikeCall;
            BackstopController.Ball += OnBallCall;
            FormatCount();

            // Set the pitch to the default.
            PitchBreakdown.sprite = PitchController.Pitches.FirstOrDefault()?.PitchSprite;
        }

        /// <summary>
        /// Called on Unity Update.
        /// </summary>
        protected virtual void Update()
        {
            if(Input.GetMouseButtonDown(0) &&
                PitchController.Cursor.IsCursorWithinBounds())
            {
                PitchController.ThrowPitch();
            }
        }

        /// <summary>
        /// Populates the dropdown options for <see cref="PitchTypeDropdown"/>.
        /// </summary>
        protected void PopulatePitchTypeDropdown()
        {
            // Collect all of the pitch names that are configured on the controller.
            var options = new List<string>();
            foreach(var pitch in PitchController.Pitches)
            {
                if(pitch != null)
                {
                    options.Add(pitch.PitchType.ToString("G"));
                }
                else
                {
                    Debug.LogWarning("Unable to populate a null pitch option from the Pitch Controller");
                }
            }

            // Clear the old options of the Dropdown menu
            PitchTypeDropdown.ClearOptions();

            // Add the options created in the List above
            PitchTypeDropdown.AddOptions(options);
        }

        /// <summary>
        /// Formats the current AB count.
        /// </summary>
        protected void FormatCount()
        {
            CountText.text = $"{NumberOfBallsInCurrentCount}-{NumberOfStrikesInCurrentCount}";
        }

        /// <summary>
        /// Processes the strike/ball call and formats it on the HUD.
        /// </summary>
        /// <param name="strike">If true, pitch was a strick.</param>
        protected void ProcessCall(bool strike)
        {
            if(strike)
            {
                NumberOfStrikesInCurrentCount++;
            }
            else
            {
                NumberOfBallsInCurrentCount++;
            }

            if(NumberOfBallsInCurrentCount >= 4)
            {
                //walk
                SoundController.Play(SoundEnum.CrowdBoo);
                NumberOfStrikesInCurrentCount = 0;
                NumberOfBallsInCurrentCount = 0;
            }
            else if(NumberOfStrikesInCurrentCount >= 3)
            {
                // strikeout
                SoundController.Play(SoundEnum.CrowdCheer);
                SoundController.Play(SoundEnum.UmpireOut);
                NumberOfStrikesInCurrentCount = 0;
                NumberOfBallsInCurrentCount = 0;
            }

            // just updated the count
            FormatCount();
        }
        #endregion Methods

        #region Event Handlers
        /// <summary>
        /// Called when a strike call occurs.
        /// </summary>
        /// <param name="sender">The Strike Zone Controller (aka. the ump)</param>
        /// <param name="args">The resulting pitch data for display.</param>
        private void OnStrikeCall(object sender, PitchResultData data)
        {
            SoundController.Play(SoundEnum.UmpireStrike);
            ProcessCall(true);
        }

        /// <summary>
        /// Called when a ball call occurs.
        /// </summary>
        /// <param name="sender">The Backstop Controller (aka. the ump)</param>
        /// <param name="args">Empty.</param>
        private void OnBallCall(object sender, System.EventArgs data)
        {
            SoundController.Play(SoundEnum.UmpireBall);
            ProcessCall(false);
        }

        /// <summary>
        /// Called when <see cref="PitchTypeDropdown"/> value is changed.
        /// </summary>
        /// <param name="dropDown">Reference to the dropdown that changed.</param>
        public void OnDropDownChanged(TMP_Dropdown dropDown)
        {
            PitchController.PitchTypeIndex = dropDown.value;
            PitchBreakdown.sprite = PitchController.Pitches[dropDown.value].PitchSprite;
        }
        #endregion Event Handlers
    }
}