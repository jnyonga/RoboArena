using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveTime = 0.2f;
    private Vector2Int gridPosition;
    private bool isMoving = false;

   [SerializeField] private GridManager gridManager;
    void Awake()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid Manager").GetComponent<GridManager>();
    }
    void Start()
    {
        gridPosition = Vector2Int.RoundToInt(transform.position);
        transform.position = new Vector2(gridPosition.x, gridPosition.y);
    }

   
    void Update()
    {
        if(!isMoving)
        {
            Vector2Int moveDirection = Vector2Int.zero;

            if (Input.GetKey(KeyCode.W)) moveDirection.y += 1;
            if (Input.GetKey(KeyCode.S)) moveDirection.y -= 1;
            if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
            if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;

            if (moveDirection != Vector2Int.zero)
            {
                Vector2Int targetPosition = gridPosition + moveDirection;

                if (IsValidPosition(targetPosition))
                {
                    StartCoroutine(MoveToPosition(targetPosition));
                }
            }
        }
    }

    bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridManager.width &&
               position.y >= 0 && position.y < gridManager.height;
    }

    IEnumerator MoveToPosition(Vector2Int targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetPosition.x, targetPosition.y, 0);
        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        gridPosition = targetPosition;
        isMoving = false;
    }
}
