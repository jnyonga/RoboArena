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
    [SerializeField] private List<EnemySpawn> wave2Enemies; // List of enemies and their spawn locations
    [SerializeField] private List<EnemySpawn> wave3Enemies; // List of enemies and their spawn locations

    private int currentWave = 0;

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

    public void SpawnWave2()
    {
        foreach (EnemySpawn enemy in wave2Enemies)
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

    public void SpawnWave3()
    {
        foreach (EnemySpawn enemy in wave3Enemies)
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

    // Check if all enemies in the current wave are defeated
    public void CheckForWaveCompletion()
    {
        if (currentWave == 0 && AreEnemiesDefeated())
        {
            // Spawn wave 1
            currentWave = 1;
        }
        else if (currentWave == 1 && AreEnemiesDefeated())
        {
            Debug.Log("Wave 1 defeated! Spawning Wave 2.");
            currentWave = 2;
            SpawnWave2(); // Spawn wave 2
        }
        else if (currentWave == 2 && AreEnemiesDefeated())
        {
            Debug.Log("Wave 2 defeated! Spawning Wave 3.");
            currentWave = 3;
            SpawnWave3(); // Spawn wave 3
        }
    }

    // Check if all enemies are defeated by checking the count of active enemies
    private bool AreEnemiesDefeated()
    {
        // Check if there are no active enemies left in the scene
        foreach (GameObject enemy in enemyInstances)
        {
            if (enemy != null) // If the enemy is still in the scene (alive)
            {
                return false; // Not all enemies are defeated
            }
        }
        return true; // All enemies are defeated
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
