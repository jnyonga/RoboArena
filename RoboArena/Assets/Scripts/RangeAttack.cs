using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RangeAttack : MonoBehaviour, IPlayerMove
{
    public GridManager gridManager;
    public MoveManager moveManager;
    public bool isReady = false;
    private bool justAttacked = true;
    public int cooldownTurns = 5;
    
    private Tile lastFacingTile; // To store the last tile the player faced
    private Tile closestOccupantTile;

    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
        moveManager = GameObject.FindGameObjectWithTag("Move Manager").GetComponent<MoveManager>();
        GameManager.OnGameStateChanged += HandleTurnChange;
        cooldownTurns = 5;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleTurnChange; // Unsubscribe to avoid memory leaks
    }

    void Update()
    {
        if (isReady && cooldownTurns == 5)
        {
            DetectTilePlayerIsFacing();
        }
    }

    void HandleTurnChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playerturn)
        {
            if(!justAttacked && cooldownTurns < 5)
            {
                cooldownTurns++; // Reduce cooldown when a new player turn starts
            }
            
            justAttacked = false;
        }
    }

    void DetectTilePlayerIsFacing()
    {
        Vector2Int playerGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        // Get the direction the player is facing
        Vector2Int direction = moveManager.facingDirection;

        // Initialize list to store detected tiles
        List<Tile> detectedTiles = new List<Tile>();

        // Initialize the closest occupant tile to null
        closestOccupantTile = null;

        // Start detecting in the facing direction
        Vector2Int currentTilePos = playerGridPos + direction;

        // Detect tiles in a straight line in the facing direction
        while (true)
        {
            Tile currentTile = gridManager.GetTileAtPosition(currentTilePos);
            
            if (currentTile != null)
            {
                // Add the tile to the detected list
                detectedTiles.Add(currentTile);

                // Check if the tile has an occupant
                GameObject occupant = currentTile.GetOccupant();
                if (occupant != null)
                {
                    // If an occupant is found, set this tile as the closest occupant tile
                    closestOccupantTile = currentTile;
                    break; // Stop detecting further since we found the closest enemy
                }
            }
            else
            {
                // Stop if there's no valid tile (out of bounds or no tile)
                break;
            }

            // Move one tile further in the same direction
            currentTilePos += direction;
        }

        // Reset previous highlighted tiles (if any)
        ResetTileHighlight(lastFacingTile);
        lastFacingTile = null;

        // Highlight the detected tiles
        foreach (var tile in detectedTiles)
        {
            HighlightTile(tile, true);
        }
    }

    void HighlightTile(Tile tile, bool shouldHighlight)
    {
        if (tile != null)
        {
            if (shouldHighlight)
                tile.HoverAttack();
            else
                tile.DeselectAttack();
        }
    }

    void ResetTileHighlight(Tile tile)
    {
        if (tile != null)
        {
            tile.DeselectAttack();
        }
    }
    public void PerformAttack()
    {
        if (GameManager.Instance.State != GameManager.GameState.Playerturn || cooldownTurns < 1)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.GetComponent<GameManager>().attackTime = 1;
            Damage();
            isReady = false;
            cooldownTurns = 0;
            justAttacked = true;
            //GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
        }
    }

    void Damage()
    {
        if (closestOccupantTile != null)
        {
            DealDamageToTile(closestOccupantTile);
        }
    }

    void DealDamageToTile(Tile tile)
    {
        if (tile != null)
        {
            GameObject enemy = tile.GetOccupant(); // Assuming the enemy is stored in the tile or has a reference
            if (enemy != null)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(2); // Apply damage to the enemy
                Debug.Log("Enemy on " + tile.name + " took damage!");
            }
        }
    }
}
