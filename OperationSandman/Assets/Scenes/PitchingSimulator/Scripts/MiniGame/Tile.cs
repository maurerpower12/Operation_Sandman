// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.MiniGame
{
    using UnityEngine;

    public class Tile : MonoBehaviour
    {
        #region Fields
        public SpriteRenderer TileSpriteRenderer;

        /// <summary>
        /// The collider that defines the strike zone.
        /// </summary>
        /// <remarks>No collider events are used.</remarks>
        [SerializeField]
        protected Collider Collider;
        #endregion Fields

        #region Events
        #endregion Events

        #region Properties
        public Vector2 BoardPosition { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Color TileColor { get; private set; }
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

        public void SetColor(Color color)
        {
            TileColor = color;
            TileSpriteRenderer.material.color = color;
        }

        /// <summary>
        /// Determines if the pitch is pointing a valid direction.
        /// </summary>
        /// <returns>True if it is within bounds. Else false.</returns>
        public bool IsTouchingTile(Vector3 contactPoint)
        {
            return Collider.bounds.Contains(contactPoint);
        }
        #endregion Methods
    }
}