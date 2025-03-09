using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject playerPrefab;
    private GameObject playerInstance;

    public void Awake()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
    }

    public void SpawnPlayerAtTileName(string tileName)
    {
        Tile targetTile = FindTileByName(tileName);
        
        if (targetTile != null)
        {
            if (playerInstance != null)
            {
                Destroy(playerInstance);
            }
            
            playerInstance = Instantiate(playerPrefab, targetTile.transform.position, Quaternion.identity);
            targetTile.SetOccupant(playerInstance);
        }
        else
        {
            Debug.LogError($"Tile with name {tileName} not found!");
        }
    }

    private Tile FindTileByName(string tileName)
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in allTiles)
        {
            if (tile.name == tileName)
            {
                return tile;
            }
        }
        return null;
    }
}
