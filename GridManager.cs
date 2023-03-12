using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public struct Tile
    {

    }
    public class GridManager : MonoBehaviour
    {
        Tile[,] grid;
        public int gridWidth, gridHeight;
        private void Awake()
        {
            GenerateGrid();
        }
        public void GenerateGrid()
        {
            Initialize2DArray();
            PlaceGridRows();
            PlaceGridColumns();
        }

        GameObject gridMain;
        void Initialize2DArray()
        {
            grid = new Tile[gridWidth, gridHeight];
            gridMain = new GameObject("Grid");
            gridMain.transform.SetParent(transform.parent);
            gridMain.transform.localScale = Vector2.one;
            gridMain.transform.localPosition = new();
        }
        
        void PlaceGridRows()
        {
            for (int i = 0; i <= gridWidth; i++)
            {
                PlaceGridRow(i);
            }
        }
        void PlaceGridRow(int r)
        {
            var sprite = GetSprite().transform;
            sprite.SetParent(gridMain.transform);
            sprite.localPosition = new(0.5f, (float)r / gridWidth);
            sprite.localScale = new(1f, gridBroadness);
        }
        void PlaceGridColumns()
        {
            for (int j = 0; j <= gridHeight; j++)
            {
                PlaceGridColumn(j);
            }
        }
        public float gridBroadness = 0.01f;
        void PlaceGridColumn(int c)
        {
            var sprite = GetSprite().transform;
            sprite.SetParent(gridMain.transform);
            sprite.localPosition = new((float)c / gridHeight, 0.5f);
            sprite.localScale = new(gridBroadness, 1f);
        }
        GameObject GetSprite()
        {
            var sprite = new GameObject("Sprite");
            var spriteRenderer = sprite.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 2;
            spriteRenderer.sprite = Resources.Load<Sprite>("Square");
            return sprite;
        }
    }
}