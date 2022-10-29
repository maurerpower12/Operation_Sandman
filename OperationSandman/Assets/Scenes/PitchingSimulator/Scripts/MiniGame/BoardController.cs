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
        protected GameObject Tile;

        [SerializeField]
        protected Vector2 BoardSize = new Vector2(4, 4);

        [SerializeField]
        protected Vector2 Padding = new Vector2();

        [SerializeField]
        protected Transform BoardParentObject;

        [NonSerialized]
        protected Tile[,] Tiles;
        #endregion Fields

        #region Events
        #endregion Events

        #region Properties
        /// <summary>
        /// Singleton Instance of this class.
        /// </summary>
        public static BoardController Instance { get; private set; }

        /// <summary>
        /// The types of pitch colors possible.
        /// </summary>
        public List<Color> PitchColors { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Called of Unity Awake.
        /// </summary>
        protected void Awake()
        {
            Instance = this;
            PitchColors = new List<Color>();
        }

        /// <summary>
        /// Called of Unity Start.
        /// </summary>
        protected void Start()
        {
            CreateBoard();
        }

        public void CreateBoard()
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
        /// Called when the collision occurs on this object.
        /// </summary>
        /// <param name="collision">Object involved in this collision.</param>
        public void OnPitchFinished(Vector3 contactPoint)
        {
            foreach(var tile in Tiles)
            {
                var touching = tile.IsTouchingTile(contactPoint);
                if(touching)
                {
                    Debug.Log("Hit tile: " + tile.name);
                    var matching = FindAllMatches(tile);

                    foreach(var match in matching)
                    {
                        match.gameObject.SetActive(false);
                    }
                }
            }
        }

        protected List<Tile> FindAllMatches(Tile hitTile)
        {
            var matchingTiles = new List<Tile>() { hitTile };
            var adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            foreach(var direction in adjacentDirections)
            {
                var adjacentTile = GetAdjacent(hitTile.BoardPosition, direction);
                if(adjacentTile != null)
                {
                    Debug.Log("Adjacent tile: " + adjacentTile.name);
                    if(adjacentTile.TileColor == hitTile.TileColor)
                    {
                        Debug.Log("Match Found tile: " + adjacentTile.name);
                        matchingTiles.Add(adjacentTile);
                    }
                }
            }

            return matchingTiles;
        }

        private Tile GetAdjacent(Vector2 pos, Vector2 castDir)
        {
            var newLocation = pos + castDir;
            if(newLocation.x >= 0 && newLocation.x < BoardSize.x
                && newLocation.y >= 0 && newLocation.y < BoardSize.y)
            {
                return Tiles[(int)newLocation.x, (int)newLocation.y];
            }
            return null;
        }

        /// <summary>
        /// Generates the game board.
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        protected void GenerateBoard(float xOffset, float yOffset)
        {
            // there should only ever be one board so reset if needed.
            ResetBoard();

            Tiles = new Tile[(int)BoardSize.x, (int)BoardSize.y];

            var startX = BoardParentObject.position.x;
            var startY = BoardParentObject.position.y;
            Color previousColor = PitchColors[0];

            for(var x = 0; x < (int)BoardSize.x; x++)
            {
                for(var y = 0; y < (int)BoardSize.y; y++)
                {
                    var newTile = Instantiate(Tile,
                        new Vector3(startX + (xOffset * x), startY + (yOffset * y),
                        BoardParentObject.position.z),
                        Tile.transform.rotation, BoardParentObject);
                    var newTiltComponent = newTile.GetComponent<Tile>();
                    if(newTiltComponent != null)
                    {
                        newTile.name = $"{x}x{y}";
                        newTiltComponent.BoardPosition = new Vector2(x,y);
                        var randColor = GetRandomColor(new List<Color>() { previousColor });
                        newTiltComponent.SetColor(randColor);
                        Tiles[x, y] = newTiltComponent;
                        previousColor = randColor;
                    }
                }
            }
        }

        /// <summary>
        /// Generates the game board.
        /// </summary>
        protected void ResetBoard()
        {
            if(Tiles != null)
            {
                foreach(var tile in Tiles)
                {
                    Destroy(tile.gameObject);
                }
                Array.Clear(Tiles, 0, Tiles.Length);
            }
        }

        /// <summary>
        /// Gets a random color from <see cref="PitchColors"/> given an exclude list.
        /// </summary>
        /// <param name="excludeColors">Colors to exclude.</param>
        /// <returns>Random color.</returns>
        protected Color GetRandomColor(List<Color> excludeColors)
        {
            var possibleCharacters = new List<Color>();
            possibleCharacters.AddRange(PitchColors);

            foreach(var color in excludeColors)
            {
                possibleCharacters.Remove(color);
            }

            return possibleCharacters[UnityEngine.Random.Range(0, possibleCharacters.Count)];
        }
        #endregion Methods
    }
}