using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string playerSpawnTileName;
    [SerializeField] private MoveManager moveManager;
    private EnemyManager enemyManager;
    public GameObject playerInstance;
    public GameObject player;
    public int attackTime;
    public int turnNumber = 0;

    public TextMeshProUGUI turnText;

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
            turnNumber++;
            StartCoroutine(PlayerTurn());
            player.GetComponent<PlayerHealth>().PowerPerTurn(); //lose power per turn
            enemyManager.CheckForWaveCompletion();
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
        enemyManager = GameObject.FindGameObjectWithTag("Enemy Manager").GetComponent<EnemyManager>();
    }

    void Start()
    {
        SpawnPlayerAtTileName(playerSpawnTileName);
        UpdateGameState(GameState.Playerturn);

        moveManager.InitializePlayer();
    }

    void Update()
    {
        turnText.text = turnNumber.ToString("F0");

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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

            player = GameObject.FindGameObjectWithTag("Player");
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
        yield return new WaitForSeconds(attackTime);

        Debug.Log("Enemy turn started.");

        attackTime = 0;

        ClearTileSelect();

        yield return new WaitForSeconds(0.5f); // Simulate enemy actions
        //Debug.Log("Enemy turn ended.");
        UpdateGameState(GameState.Playerturn);
    }

    public void ClearTileSelect()
    {
        List<Tile> allTiles = gridManager.GetAllTiles(); // Get all tiles

        foreach (Tile tile in allTiles)
        {
            tile.DeselectAttack();
        }
    }
}
