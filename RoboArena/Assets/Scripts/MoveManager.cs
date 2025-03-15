using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class MoveManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayerMovement playerMovement;
    private GameObject player;
    public Vector2Int facingDirection = Vector2Int.up;

    private Queue<IPlayerMove> moveQueue = new Queue<IPlayerMove>();

    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void InitializePlayer()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Rotate clockwise
        {
            RotatePlayer(-90);
        }
        else if (Input.GetKeyDown(KeyCode.E)) // Rotate counterclockwise
        {
            RotatePlayer(90);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && moveQueue.Count > 0)
        {
            StartCoroutine(PerformMoves());  // Start performing moves in the queue
        }
    }

    void RotatePlayer(float angle)
    {
        if (GameManager.Instance.State != GameManager.GameState.Playerturn)
        return;

        facingDirection = RotateVector(facingDirection, angle);
        player.transform.Rotate(0, 0, angle);

        GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
    }

    Vector2Int RotateVector(Vector2Int dir, float angle)
    {
        if (angle == -90) // Clockwise rotation
            return new Vector2Int(dir.y, -dir.x);
        else if (angle == 90) // Counterclockwise rotation
            return new Vector2Int(-dir.y, dir.x);
        
        return dir;
    }

    // Add a move to the queue, now referencing the player's attack components
    public void AddMoveToQueue(IPlayerMove move)
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject not found!");
            return;
        }

        moveQueue.Enqueue(move);  // Add the selected move to the queue
        Debug.Log("Move added to queue: " + move.GetType().Name);
    }

    // Perform all moves in the queue one by one
    private IEnumerator<WaitForSeconds> PerformMoves()
    {
        while (moveQueue.Count > 0)
        {
            IPlayerMove currentMove = moveQueue.Dequeue();  // Get the next move from the queue
            
            // Perform the move
            if (currentMove is SwordAttack)
            {
                SwordAttack swordAttack = (SwordAttack)currentMove;
                swordAttack.PerformAttack();  // Implement this in your SwordAttack class
            }
            else if (currentMove is RangeAttack)
            {
                RangeAttack rangeAttack = (RangeAttack)currentMove;
                rangeAttack.PerformAttack();  // Implement this in your RangeAttack class
            }

            yield return new WaitForSeconds(1f);  // Wait for 1 second (or however long the attack animation takes)
        }

        // After performing all moves, reset or update game state
        Debug.Log("All moves completed.");
    
        GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
    }
}
