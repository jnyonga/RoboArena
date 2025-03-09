using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;

    [SerializeField] private GameObject occupant;

     public void Init(bool isOffset)
     {
        renderer.color = isOffset ? offsetColor : baseColor;
     }

    void OnMouseEnter()
    {
        highlight.SetActive(true);
    }
    void OnMouseExit()
    {
        highlight.SetActive(false);
    }
    public void SetOccupant(GameObject newOccupant)
    {
        occupant = newOccupant;
    }

    public GameObject GetOccupant()
    {
        return occupant;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            SetOccupant(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == occupant)
        {
            SetOccupant(null);
        }
    }
}
