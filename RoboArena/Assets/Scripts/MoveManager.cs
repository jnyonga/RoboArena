using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayerMovement playerMovement;

    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public void InitializePlayer()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public void PerformAttack(Vector2Int targetPosition)
    {
        // Get the selected tile and its adjacent tiles
        List<Vector2Int> attackTiles = GetAttackTiles(targetPosition);

        HighlightAttackArea(attackTiles);

        // For each tile in the attack area, check if it contains an enemy and apply damage
        foreach (Vector2Int tilePos in attackTiles)
        {
            Tile targetTile = gridManager.GetTileAtPosition(tilePos);
            if (targetTile != null)
            {
                GameObject occupant = targetTile.GetOccupant();
                if (occupant != null && occupant.CompareTag("Enemy"))
                {
                    // Here you can apply damage to the enemy
                    Debug.Log($"Enemy hit at {tilePos}");
                    // Apply damage logic, e.g., reduce health
                    occupant.GetComponent<EnemyHealth>().TakeDamage(2);
                }
            }
        }

        
    }

    List<Vector2Int> GetAttackTiles(Vector2Int targetPosition)
    {
        List<Vector2Int> attackTiles = new List<Vector2Int>();

        // Add the target tile
        attackTiles.Add(targetPosition);

        // Get the adjacent tiles (8 directions)
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0), new Vector2Int(-1, 0), // Right, Left
            new Vector2Int(0, 1), new Vector2Int(0, -1), // Up, Down
            new Vector2Int(1, 1), new Vector2Int(-1, 1), // Top-right, Top-left
            new Vector2Int(1, -1), new Vector2Int(-1, -1) // Bottom-right, Bottom-left
        };

        // Add adjacent tiles to the attack area
        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentTile = targetPosition + dir;
            if (IsWithinGrid(adjacentTile))
            {
                attackTiles.Add(adjacentTile);
            }
        }

        return attackTiles;
    }

    bool IsWithinGrid(Vector2Int position)
    {
        // Check if the position is within the grid bounds
        return position.x >= 0 && position.x < gridManager.width &&
               position.y >= 0 && position.y < gridManager.height;
    }

    void HighlightAttackArea(List<Vector2Int> attackTiles)
    {
        // Highlight the attack tiles
        foreach (Vector2Int tilePos in attackTiles)
        {
            Tile targetTile = gridManager.GetTileAtPosition(tilePos);
            if (targetTile != null)
            {
                targetTile.HoverAttack();
            }
        }
    }

    void ResetHighlightArea(List<Vector2Int> attackTiles)
    {
        foreach (Vector2Int tilePos in attackTiles)
        {
            Tile targetTile = gridManager.GetTileAtPosition(tilePos);
            if (targetTile != null)
            {
                targetTile.DeselectAttack();
            }
        }
    }
}
