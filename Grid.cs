using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Game
{
    public class Tile
    {
        public bool empty;
        protected static Grid CurrentGrid => Grid.currentGrid;
        protected Vector2Int position;

        public GameObject GameObject => CurrentGrid.GetGameObject(position.x, position.y);
        public Tile()
        {
            empty = true;
        }
        public void Swap(Vector2Int direction)
        {

        }
        public virtual void Init(Vector2Int position)
        {
            this.position = position;
        }
        public virtual void Update()
        {

        }
    }
    public enum ObjectTileType
    {
        Type1,
        Type2,
        Type3
    }
    public class ObjectTile : Tile
    {
        public ObjectTileType type;
        public ObjectTile(ObjectTileType type)
        {
            this.type = type;
            empty = false;
        }
        public override void Init(Vector2Int position)
        {
            GameObject.GetComponentInChildren<SpriteRenderer>().sprite = CurrentGrid.objectTileSprites[(int)type];
        }
        public override void Update()
        {

        }
    }
    public class Grid : MonoBehaviour
    {
        public Sprite[] objectTileSprites;
        public bool IsTile(Vector2Int pos)
        {
            try
            {
                return this[pos] != null;
            }
            catch (System.IndexOutOfRangeException)
            {
                return false;
            }
        }

        public void Move(Vector2Int old, Vector2Int target)
        {
            StartCoroutine(_Move(old, target));
        }

        private const float MoveSpeed = 0.1f;
        private IEnumerator _Move(Vector2Int old, Vector2Int target)
        {
            var tile = this[old];
            var otherTile = this[target];
            for (float i = 0; i < 1; i += MoveSpeed)
            {
                yield return null;
            }
        }

        public GameObject GetGameObject(int x, int y)
        {
            return _gameObjects[x, y];
        }
        public Tile this[int x, int y]
        {
            get => _grid[x, y];
            set
            {
                _grid[x, y] = value;
                
            }
        }
        public Tile this[Vector2Int vec2int]
        {
            get => _grid[vec2int.x, vec2int.y];
            set => _grid[vec2int.x, vec2int.y] = value;
        }
        public static Grid currentGrid;

        private GameObject[,] _gameObjects;
        private Tile[,] _grid;
        public GameObject[] elements;
        
        public Vector2 tileSize;
        public int width, height;

        private void Awake()
        {
            currentGrid = this;
            BuildGrid();
            _grid[0, 3] = new ObjectTile(ObjectTileType.Type1);
        }
        private void BuildGrid()
        {
            ConstructGrid();
            InitializeGrid();
        }
        void ConstructGrid()
        {
            Setup2DArray();
        }
        void Setup2DArray()
        {
            _grid = new Tile[width, height];
        }
        void InitializeGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var element = PlaceObject(y + x);
                    ArrangeObject(element, x, y);
                }
            }
        }
        GameObject PlaceObject(int index)
        {
            return InstantiateObject(index);
        }
        GameObject InstantiateObject(int index)
        {
            return Instantiate(elements[index % elements.Length]);
        }
        void ArrangeObject(GameObject gameObject, int r, int c)
        {
            Vector2Int position = GetAppropriatePosition(r, c);
            MapToGrid(gameObject, position);
            MapToWorld(gameObject, position);
        }
        Vector2Int GetAppropriatePosition(int r, int c)
        {
            return new(r, c);
        }
        void MapToGrid(GameObject gameObject, Vector2Int vector2Int)
        {
            _gameObjects[vector2Int.x, vector2Int.y] = gameObject;
            _grid[vector2Int.x, vector2Int.y] = new();
        }

        public Vector2 CoordinateToWorld(Vector2Int coordinate)
        {
            return tileSize * coordinate - (tileSize / 2f * new Vector2Int(width - 1, height - 1));
        }
        void MapToWorld(GameObject gameObject, Vector2Int vector2Int)
        {
            gameObject.transform.position = CoordinateToWorld(vector2Int);
        }
    }
}