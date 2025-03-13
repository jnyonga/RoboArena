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
    
}
