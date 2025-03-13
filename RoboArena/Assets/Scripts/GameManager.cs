using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string playerSpawnTileName;
    [SerializeField] private MoveManager moveManager;
    private GameObject playerInstance;

    public enum GameState
    {
        Playerturn,
        Enemyturn,
        Victory,
        Lose
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch(newState) {
            case GameState.Playerturn:
            StartCoroutine(PlayerTurn());
                break;
            case GameState.Enemyturn:
            StartCoroutine(EnemyTurn());
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }
    public void Awake()
    {
        Instance = this;
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
        moveManager = GameObject.FindGameObjectWithTag("Move Manager").GetComponent<MoveManager>();
    }

    void Start()
    {
        SpawnPlayerAtTileName(playerSpawnTileName);
        UpdateGameState(GameState.Playerturn);

        moveManager.InitializePlayer();
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

    IEnumerator PlayerTurn()
    {
        Debug.Log("Player turn started.");
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy turn started.");
        yield return new WaitForSeconds(1f); // Simulate enemy actions
        Debug.Log("Enemy turn ended.");
        UpdateGameState(GameState.Playerturn);
    }
}
