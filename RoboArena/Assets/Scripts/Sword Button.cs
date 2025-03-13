using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class SwordButton : MonoBehaviour
{
    private GameObject player;

    private bool isSelected = false;
    private Image buttonImage;
    private Color defaultColor;
    private Color selectedColor = Color.green;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonImage = GetComponent<Image>();
        defaultColor = buttonImage.color;
        GameManager.Instance.GetComponent<GameManager>().playerInstance = player;
    }

    void Update()
    {
        if (player.GetComponent<SwordAttack>().isReady == false)
        {
            buttonImage.color = defaultColor;
        }
    }
    public void ToggleSelection()
    {
        isSelected = !isSelected;
        buttonImage.color = isSelected ? selectedColor : defaultColor;

        player.GetComponent<SwordAttack>().isReady = isSelected;

        GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
    }

}
