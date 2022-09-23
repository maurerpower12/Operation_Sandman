// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.Controllers
{
    using System;
    using System.Collections.Generic;
    using Common.Scripts;
    using Baseball;
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Controlls the pitch sequence and generating the baseballs.
    /// </summary>
    public class PitchController : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The types of pitches this pitcher has.
        /// </summary>
        [SerializeField]
        public List<PitchData> Pitches;

        /// <summary>
        /// The baesball prefab that the pitch will throw.
        /// </summary>
        [SerializeField]
        protected GameObject BaseballPrefab;

        /// <summary>
        /// The starting position of the throw.
        /// </summary>
        [SerializeField]
        protected Transform BaseballStartingPosition;

        /// <summary>
        /// The cursor of where the player is trying to throw.
        /// </summary>
        [SerializeField]
        public CursorFollow Cursor;

        /// <summary>
        /// The animator used to control the pitcher.
        /// </summary>
        [SerializeField]
        protected Animator PitcherAnimator;

        /// <summary>
        /// Dynamic list of all of the baseballs we have in the scene.
        /// </summary>
        [NonSerialized]
        protected List<GameObject> InstantiatedBaseballs = new List<GameObject>();

        /// <summary>
        /// The string value of the pitch state in the animator.
        /// </summary>
        [NonSerialized]
        private string PitchTriggerName = "Pitch";

        /// <summary>
        /// The hash value of the pitch state in the animator.
        /// </summary>
        [NonSerialized]
        private int PitchStateHash = -1;

        /// <summary>
        /// The string value of the wave state in the animator.
        /// </summary>
        [NonSerialized]
        private string WaveTriggerName = "Wave";

        /// <summary>
        /// The hash value of the wave state in the animator.
        /// </summary>
        [NonSerialized]
        private int WaveStateHash = -1;

        /// <summary>
        /// The duration of <see cref="PitchTriggerName"/>.
        /// </summary>
        /// <remarks>
        /// There are ways to get this value dynamically based on the animation
        /// length or a custom key frame set up in the pitching animation.
        /// Using this solution of a hardcoded value for now.
        /// </remarks>
        [NonSerialized]
        private WaitForSeconds PitchStateWaitInSeconds;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The currently selected pitch index in <see cref="Pitches"/>.
        /// </summary>
        /// <remarks>Zero based index.</remarks>
        public int PitchTypeIndex { get; set; }

        /// <summary>
        /// True if the pitcher is currently throwing a pitch.
        /// </summary>
        public bool IsThrowing { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Called on Unity Awake.
        /// </summary>
        protected virtual void Awake()
        {
            PitchStateHash = Animator.StringToHash(PitchTriggerName);
            WaveStateHash = Animator.StringToHash(WaveTriggerName);
            PitchStateWaitInSeconds = new WaitForSeconds(2.0f);
        }

        /// <summary>
        /// Called on Unity Start.
        /// </summary>
        protected virtual void Start()
        {
            PitcherAnimator.SetTrigger(WaveStateHash);
        }

        /// <summary>
        /// Instantiates a baseball and throws it.
        /// </summary>
        public void ThrowPitch()
        {
            if(!IsThrowing)
            {
                // This is a design decision but only ever allow one ball in play.
                CleanUpPitches();

                StartCoroutine(PitchingSequence(Pitches[PitchTypeIndex]));
            }
        }

        /// <summary>
        /// Deletes any instantiated baseballs.
        /// </summary>
        public void CleanUpPitches()
        {
            InstantiatedBaseballs.ForEach(ball => Destroy(ball));
            InstantiatedBaseballs.Clear();
        }

        /// <summary>
        /// Perform the pitching sequence.
        /// </summary>
        /// <param name="pitchData">The data for the pitch.</param>
        /// <returns>Coroutine.</returns>
        protected IEnumerator PitchingSequence(PitchData pitchData)
        {
            IsThrowing = true;
            // First, make sure we grab the cursor data for where the player is trying to throw the ball.
            pitchData.GeneratePitchData(Cursor.gameObject);

            // Start the pitcher throwing animation
            PitcherAnimator.SetTrigger(PitchStateHash);
            yield return PitchStateWaitInSeconds;

            // Create a baseball to throw.
            var baseballObject = Instantiate(BaseballPrefab,
                                        BaseballStartingPosition.gameObject.transform.position,
                                        Quaternion.identity, this.transform);

            if(baseballObject != null)
            {
                InstantiatedBaseballs.Add(baseballObject);
                var baseballScript = baseballObject.GetComponent<Baseball>();
                if(baseballScript != null)
                {
                    baseballScript.Throw(pitchData);
                    yield return new WaitForSeconds(pitchData.Duration);
                }
                else
                {
                    Debug.LogError("PitchController: Baseball prefab does not have a Baseball script attached. Please fix.");
                }
            }
            else
            {
                Debug.LogError("PitchController: Unable to Instantiate a baseball.");
            }

            // Hold while the pitcher is still throwing the ball
            while(PitcherAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash == PitchStateHash)
            {
                yield return null;
            }

            CleanUpPitches();

            IsThrowing = false;
        }
        #endregion Methods
    }
}