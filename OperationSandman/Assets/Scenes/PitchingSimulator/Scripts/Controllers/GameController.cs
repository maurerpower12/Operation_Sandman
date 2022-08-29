// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System.Collections.Generic;
    using Baseball;
    using TMPro;
    using UnityEngine;

    public class GameController : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        protected PitchController PitchController;

        [SerializeField]
        protected StrikeZoneController StrikeZoneController;

        [SerializeField]
        protected BackstopController BackstopController;

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
        /// Called when <see cref="PitchTypeDropdown"/> value is changed.
        /// </summary>
        /// <param name="dropDown">Reference to the dropdown that changed.</param>
        public void OnDropDownChanged(TMP_Dropdown dropDown)
        {
            Debug.Log($"New Pitch Type Selected -> {(PitchType)dropDown.value}");
            PitchController.PitchTypeIndex = dropDown.value;
        }

        /// <summary>
        /// Called on Unity Start.
        /// </summary>
        protected virtual void Start()
        {
            PopulatePitchTypeDropdown();
            StrikeZoneController.Strike += OnStrikeCall;
            BackstopController.Ball += OnBallCall;
            FormatCount();
        }

        /// <summary>
        /// Called on Unity Update.
        /// </summary>
        protected virtual void Update()
        {
            if(Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                PitchController.ThrowPitch();
            }
        }

        /// <summary>
        /// Populates the dropdown options for <see cref="PitchTypeDropdown"/>.
        /// </summary>
        protected virtual void PopulatePitchTypeDropdown()
        {
            // Collect all of the pitch names that are configured on the controller.
            var options = new List<string>();
            foreach(var pitch in PitchController.Pitches)
            {
                options.Add(pitch.PitchType.ToString("G"));
            }

            // Clear the old options of the Dropdown menu
            PitchTypeDropdown.ClearOptions();

            // Add the options created in the List above
            PitchTypeDropdown.AddOptions(options);
        }

        /// <summary>
        /// Formats the current AB count.
        /// </summary>
        protected virtual void FormatCount()
        {
            CountText.text = $"{NumberOfBallsInCurrentCount}-{NumberOfStrikesInCurrentCount}";
        }

        /// <summary>
        /// Processes the strike/ball call and formats it on the HUD.
        /// </summary>
        /// <param name="strike">If true, pitch was a strick.</param>
        protected virtual void ProcessCall(bool strike)
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
                NumberOfStrikesInCurrentCount = 0;
                NumberOfBallsInCurrentCount = 0;
            }
            else if(NumberOfStrikesInCurrentCount >= 3)
            {
                // strikeout
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
            Debug.Log("Strike!");
            ProcessCall(true);
        }

        /// <summary>
        /// Called when a ball call occurs.
        /// </summary>
        /// <param name="sender">The Backstop Controller (aka. the ump)</param>
        /// <param name="args">Empty.</param>
        private void OnBallCall(object sender, System.EventArgs data)
        {
            Debug.Log("Ball");
            ProcessCall(false);
        }
        #endregion Event Handlers
    }
}