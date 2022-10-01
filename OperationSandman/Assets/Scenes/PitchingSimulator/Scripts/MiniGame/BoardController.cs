// Copyright (c) 2022

namespace Assets.Scenes.PitchingSimualtor.Scripts.MiniGame
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class BoardController : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        protected List<Sprite> Characters = new List<Sprite>();

        [SerializeField]
        protected GameObject Tile;

        [SerializeField]
        protected Vector2 BoardSize = new Vector2(4, 4);

        [SerializeField]
        protected Vector2 Padding = new Vector2();

        [SerializeField]
        protected Transform BoardParentObject;

        [NonSerialized]
        protected GameObject[,] Tiles;
        #endregion Fields

        #region Events
        #endregion Events

        #region Properties
        /// <summary>
        /// Singleton Instance of this class.
        /// </summary>
        public static BoardController Instance { get; private set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Called of Unity Awake.
        /// </summary>
        protected void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Called of Unity Start.
        /// </summary>
        protected void Start()
        {
            var tile = Tile.GetComponent<Tile>();
            if(tile != null)
            {
                var offset = tile.TileSpriteRenderer.bounds.size;
                GenerateBoard(offset.x + Padding.x, offset.y + Padding.y);
            }
            else
            {
                Debug.LogErrorFormat("Unable to generate game board since no " +
                    "Tile component was found on '{0}'", tile.name);
            }

        }

        /// <summary>
        /// Generates the game board.
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        protected void GenerateBoard(float xOffset, float yOffset)
        {
            Tiles = new GameObject[(int)BoardSize.x, (int)BoardSize.y];

            var startX = transform.position.x;
            var startY = transform.position.y;

            for(int x = 0; x < (int)BoardSize.x; x++)
            {
                for(int y = 0; y < (int)BoardSize.y; y++)
                {
                    var newTile = Instantiate(Tile,
                        new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0.0f),
                        Tile.transform.rotation, BoardParentObject);
                    newTile.name = $"{x}x{y}";
                    Tiles[x, y] = newTile;
                }
            }
        }

        /// <summary>
        /// Generates the game board.
        /// </summary>
        protected void ResetBoard()
        {
            foreach(var tile in Tiles)
            {
                Destroy(tile);
            }
            Array.Clear(Tiles, 0, Tiles.Length);
        }
        #endregion Methods
    }
}