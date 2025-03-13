using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GridManager gridManager;
    public MoveManager moveManager;
    private Tile lastFacingTile;
    private Tile lastSideTile1;
    private Tile lastSideTile2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
        moveManager = GameObject.FindGameObjectWithTag("Move Manager").GetComponent<MoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectTilePlayerIsFacing();
        
        Attack();
    }

    void DetectTilePlayerIsFacing()
    {
        Vector2Int playerGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        Vector2Int tileFacingPos = playerGridPos + moveManager.facingDirection;

        // Determine perpendicular directions correctly
        Vector2Int perpDir1, perpDir2;

        if (moveManager.facingDirection.x == 0) // Facing Up or Down
        {
            perpDir1 = new Vector2Int(1, 0);  // Right
            perpDir2 = new Vector2Int(-1, 0); // Left
        }
        else // Facing Left or Right
        {
            perpDir1 = new Vector2Int(0, 1);  // Up
            perpDir2 = new Vector2Int(0, -1); // Down
        }

        // Get side tiles
        Vector2Int sideTilePos1 = tileFacingPos + perpDir1;
        Vector2Int sideTilePos2 = tileFacingPos + perpDir2;

        // Get tile references
        Tile facingTile = gridManager.GetTileAtPosition(tileFacingPos);
        Tile sideTile1 = gridManager.GetTileAtPosition(sideTilePos1);
        Tile sideTile2 = gridManager.GetTileAtPosition(sideTilePos2);

        // Reset previously highlighted tiles
        ResetTileHighlight(lastFacingTile);
        ResetTileHighlight(lastSideTile1);
        ResetTileHighlight(lastSideTile2);

        // Set attack highlight for the new tiles
        HighlightTile(facingTile, true);
        HighlightTile(sideTile1, true);
        HighlightTile(sideTile2, true);

        // Store current tiles as last highlighted tiles
        lastFacingTile = facingTile;
        lastSideTile1 = sideTile1;
        lastSideTile2 = sideTile2;

        // Log detected tiles
        if (facingTile != null)
            Debug.Log("Player is facing tile: " + facingTile.name);
        else
            Debug.Log("Player is facing an empty space.");

        if (sideTile1 != null)
            Debug.Log("Side tile 1: " + sideTile1.name);

        if (sideTile2 != null)
            Debug.Log("Side tile 2: " + sideTile2.name);
    }
    void HighlightTile(Tile tile, bool shouldHighlight)
    {
        if (tile != null)
        {
            // Use the HoverAttack method to highlight or deselect the attack highlight
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
            // Use the DeselectAttack method to remove the highlight
            tile.DeselectAttack();
        }
    }

    void Attack()
    {
        if (GameManager.Instance.State != GameManager.GameState.Playerturn)
        return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Damage();

            GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
        }
    }

    void Damage()
    {
        DealDamageToTile(lastFacingTile);
        DealDamageToTile(lastSideTile1);
        DealDamageToTile(lastSideTile2);
    }

    void DealDamageToTile(Tile tile)
    {
        if (tile != null)
        {
            GameObject enemy = tile.GetComponent<Tile>().GetOccupant(); // Assuming the enemy is stored in the tile or has a reference
            if (enemy != null)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(2); // Apply damage to the enemy
                Debug.Log("Enemy on " + tile.name + " took damage!");
            }
        }
    }
}
