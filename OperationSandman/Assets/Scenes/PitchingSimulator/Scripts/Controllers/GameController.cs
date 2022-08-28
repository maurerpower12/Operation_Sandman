// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class GameController : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        protected PitchController PitchController;

        /// <summary>
        /// This is the dropdown of pitch types.
        /// </summary>
        [SerializeField]
        protected TMP_Dropdown PitchTypeDropdown;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Called when <see cref="PitchTypeDropdown"/> value is changed.
        /// </summary>
        /// <param name="dropDown">Reference to the dropdown that changed.</param>
        public void OnDropDownChanged(TMP_Dropdown dropDown)
        {
            Debug.Log($"New Pitch Type Selected -> {(Baseball.PitchType)dropDown.value}");
            PitchController.PitchTypeIndex = dropDown.value;
        }

        protected virtual void Start()
        {
            PopulatePitchTypeDropdown();
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
        #endregion Methods
    }
}