using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawn
    {
        public GameObject enemyPrefab;
        public string spawnTileName;
    }

    [SerializeField] private List<EnemySpawn> enemiesToSpawn; // List of enemies and their spawn locations

    private List<GameObject> enemyInstances = new List<GameObject>();

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        foreach (EnemySpawn enemy in enemiesToSpawn)
        {
            Tile targetTile = FindTileByName(enemy.spawnTileName);
            if (targetTile != null)
            {
                GameObject enemyInstance = Instantiate(enemy.enemyPrefab, targetTile.transform.position, Quaternion.identity);
                targetTile.SetOccupant(enemyInstance);
                enemyInstances.Add(enemyInstance);
            }
            else
            {
                Debug.LogError($"Tile with name {enemy.spawnTileName} not found for enemy!");
            }
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
