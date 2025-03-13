using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
   [SerializeField] public int width, height;

   [SerializeField] private Tile tilePrefab;

   [SerializeField] private Transform cam;

   private Tile[,] tiles;

   private List<Tile> allTiles = new List<Tile>();

    void Awake()
    {
        GenerateGrid();
    }
    void GenerateGrid()
   {
        tiles = new Tile[width,height];
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x,y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                tiles[x,y] = spawnedTile;
                allTiles.Add(spawnedTile);
            }
        }

        cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
   }

   public Tile GetTileAtPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < width && position.y >= 0 && position.y < height)
        {
            return tiles[position.x, position.y]; // Return the tile at the specified position
        }

        return null; // If the position is out of bounds, return null
    }

    public List<Tile> GetAllTiles()
   {
        return allTiles; // Return the list of all tiles
   }
}
