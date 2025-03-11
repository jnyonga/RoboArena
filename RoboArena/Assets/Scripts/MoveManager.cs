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
        if(Input.GetKeyDown(KeyCode.R))
        {
            RotatePlayer();
        }
    }

    void RotatePlayer()
    {
        facingDirection = RotateVector(facingDirection);
        player.transform.Rotate(0, 0, -90); // Visually rotate the player 90 degrees clockwise
    }

    Vector2Int RotateVector(Vector2Int dir)
    {
        return new Vector2Int(dir.y, -dir.x); // Always rotates 90 degrees clockwise
    }
}
