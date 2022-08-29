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
        /// Gameobject to follow the cursor.
        /// </summary>
        [SerializeField]
        protected GameObject Cursor;

        /// <summary>
        /// Distance the point is at in the scene.
        /// </summary>
        [SerializeField]
        protected float Distance = 10.0f;

        /// <summary>
        /// Called on Unity Fixed Updated.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            var mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            Cursor.transform.position = mousePosition.GetPoint(Distance);
        }
    }
}