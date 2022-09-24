// Copyright (c) 2022

namespace Assets.Scenes.Common.Scripts
{
    using UnityEngine;

    /// <summary>
    /// Follows your cursor around the screen.
    /// </summary>
    public class CursorFollow : MonoBehaviour
    {
        /// <summary>
        /// The sprite renderer for the cursor.
        /// </summary>
        [SerializeField]
        protected SpriteRenderer Renderer;

        /// <summary>
        /// Distance the point is at in the scene.
        /// </summary>
        [SerializeField]
        protected float Distance = 10.0f;

        /// <summary>
        /// The min bounds that the cursor should follow in.
        /// </summary>
        [SerializeField]
        protected Vector3 MinBounds;

        /// <summary>
        /// The max bounds that the cursor should follow in.
        /// </summary>
        [SerializeField]
        protected Vector3 MaxBounds;

        /// <summary>
        /// True if the cursor able to be displayed.
        /// </summary>
        public bool Displayable { get; set; }

        /// <summary>
        /// If true, the cursor's position will not updated.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Determines if the cursor is pointing a valid direction.
        /// </summary>
        /// <returns>True if cursor is within bounds. Else false.</returns>
        public bool IsCursorWithinBounds()
        {
            var mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            var pointOnScreen = mousePosition.GetPoint(Distance);
            return pointOnScreen.x <= MaxBounds.x &&
                   pointOnScreen.y <= MaxBounds.y &&
                   pointOnScreen.x >= MinBounds.x &&
                   pointOnScreen.y >= MinBounds.y;
        }

        /// <summary>
        /// Called on Unity Fixed Updated.
        /// </summary>
        protected void FixedUpdate()
        {
            Renderer.enabled  = Displayable;
            if(!Locked)
            {
                var mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
                var pointOnScreen = mousePosition.GetPoint(Distance);
                // Clamp the position of the cursor to be within bounds.
                // Intentionally leaving z alone since that should be the same as Distance.
                transform.position = new Vector3(
                    Mathf.Clamp(pointOnScreen.x, MinBounds.x, MaxBounds.x),
                    Mathf.Clamp(pointOnScreen.y, MinBounds.y, MaxBounds.y),
                    pointOnScreen.z);
            }
        }
    }
}