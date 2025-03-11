using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GridManager gridManager;
    public MoveManager moveManager;
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
    }

    void DetectTilePlayerIsFacing()
    {
        Vector2Int playerGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        Vector2Int tileFacingPos = playerGridPos + moveManager.facingDirection;

        Tile facingTile = gridManager.GetTileAtPosition(tileFacingPos);

        if (facingTile != null)
        {
            Debug.Log("Player is facing tile: " + facingTile.name);
        }
        else
        {
            Debug.Log("Player is facing an empty space.");
        }
    }
}
