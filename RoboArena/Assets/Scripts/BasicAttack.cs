using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    public GridManager gridManager;
    public MoveManager moveManager;
    private Tile lastFacingTile;
    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
        moveManager = GameObject.FindGameObjectWithTag("Move Manager").GetComponent<MoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<SwordAttack>().isReady == false && gameObject.GetComponent<RangeAttack>().isReady == false)
        {
            DetectTilePlayerIsFacing();
            Attack();
        }
    }

    void DetectTilePlayerIsFacing()
    {
        Vector2Int playerGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        Vector2Int tileFacingPos = playerGridPos + moveManager.facingDirection;

        // Get only the tile the player is facing
        Tile facingTile = gridManager.GetTileAtPosition(tileFacingPos);

        // Reset previously highlighted tile
        ResetTileHighlight(lastFacingTile);

        // Set attack highlight for the new tile
        HighlightTile(facingTile, true);

        // Store current tile as the last facing tile
        lastFacingTile = facingTile;
    }
    void Attack()
    {
        if (GameManager.Instance.State != GameManager.GameState.Playerturn)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.GetComponent<GameManager>().attackTime = 1;
            Damage();
            GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
        }
    }

    void Damage()
    {
        DealDamageToTile(lastFacingTile); // Only damage the tile the player is facing
    }

    void DealDamageToTile(Tile tile)
    {
        if (tile != null)
        {
            GameObject enemy = tile.GetComponent<Tile>().GetOccupant(); // Assuming the enemy is stored in the tile or has a reference
            if (enemy != null)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(1); // Apply damage to the enemy
                Debug.Log("Enemy on " + tile.name + " took damage!");
            }
        }
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
}

