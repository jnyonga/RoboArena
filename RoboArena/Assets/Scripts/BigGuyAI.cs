using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigGuyAI : MonoBehaviour
{
    public int damage = 2;
    public float moveSpeed = 5f; // Movement speed for smooth transitions
    public bool isActing = false;

    private GridManager gridManager;
    private GameManager gameManager;
    private Transform player;
    private Vector2Int enemyGridPos;
    private Vector2Int playerGridPos;
    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
        gameManager = GameManager.Instance;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameManager.OnGameStateChanged += HandleTurnChange;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleTurnChange;
    }

    void HandleTurnChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Enemyturn && !isActing)
        {
            StartCoroutine(TakeTurn());
        }
    }

    IEnumerator TakeTurn()
    {
        isActing = true;

        UpdatePositions();

        if (IsPlayerAdjacent())
        {
            AttackPlayer();
        }
        else
        {
            MoveTowardPlayer();
        }

        yield return new WaitForSeconds(0.5f); // Delay to show movement/attack animation
        isActing = false;
        gameManager.UpdateGameState(GameManager.GameState.Playerturn);
    }
    
    void UpdatePositions()
    {
        enemyGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        playerGridPos = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));
    }

    bool IsPlayerAdjacent()
    {
        return Mathf.Abs(enemyGridPos.x - playerGridPos.x) + Mathf.Abs(enemyGridPos.y - playerGridPos.y) == 1;
    }

    void AttackPlayer()
    {
        Debug.Log("Enemy attacks player!");
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    void MoveTowardPlayer()
    {
        Vector2Int direction = GetMoveDirection();
        Vector2Int targetPos = enemyGridPos + direction;

        if (gridManager.IsTileWalkable(targetPos))
        {
            StartCoroutine(MoveToPosition(targetPos));
        }
    }

    Vector2Int GetMoveDirection()
    {
        int xDiff = playerGridPos.x - enemyGridPos.x;
        int yDiff = playerGridPos.y - enemyGridPos.y;

        if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
        {
            return new Vector2Int(Mathf.Clamp(xDiff, -1, 1), 0);
        }
        else
        {
            return new Vector2Int(0, Mathf.Clamp(yDiff, -1, 1));
        }
    }

    IEnumerator MoveToPosition(Vector2Int targetPos)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }
}
