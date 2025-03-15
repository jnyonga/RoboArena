using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class SwordButton : MonoBehaviour
{
    private GameObject player;
    public List<Image> cooldownBlockList;
    private MoveManager moveManager;
    private bool isSelected = false;
    private Image buttonImage;
    private Button button;
    private Color defaultColor;
    private Color selectedColor = Color.green;
    private SwordAttack swordAttack;
    
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        defaultColor = buttonImage.color;
        GameManager.Instance.GetComponent<GameManager>().playerInstance = player;
        moveManager = MoveManager.FindAnyObjectByType<MoveManager>();
    }

    void Update()
    {
        if(player != null)
        {
            swordAttack = player.GetComponent<SwordAttack>();

            switch(player.GetComponent<SwordAttack>().cooldownTurns)
            {
                case(0):
                    cooldownBlockList[0].color = Color.grey;
                    cooldownBlockList[1].color = Color.grey;
                    cooldownBlockList[2].color = Color.grey;
                    break;
                case(1):
                    cooldownBlockList[0].color = Color.black;
                    break;
                case(2):
                    cooldownBlockList[1].color = Color.black;
                    break;
                case(3):
                    cooldownBlockList[2].color = Color.black;
                    break;
            }

            if (player.GetComponent<SwordAttack>().cooldownTurns < 3)
            {
                button.interactable = false;  // Make button non-interactable
                buttonImage.color = new Color(190,190,190);
            }
            else
            {
                button.interactable = true;  // Enable button when cooldown is at 3

                if (player.GetComponent<SwordAttack>().isReady == false)
                {
                    buttonImage.color = defaultColor;
                }
            }
        }
        else if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
    }
    public void ToggleSelection()
    {
        isSelected = !isSelected;
        buttonImage.color = isSelected ? selectedColor : defaultColor;

        if (isSelected && swordAttack != null)
        {
            // Add the existing SwordAttack component to the move queue
            moveManager.GetComponent<MoveManager>().AddMoveToQueue(swordAttack);
        }

        Debug.Log("Sword Attack Selected: " + isSelected);

        GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
    }

}
