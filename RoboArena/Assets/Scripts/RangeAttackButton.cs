using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RangeAttackButton : MonoBehaviour
{
    private GameObject player;
    public List<Image> cooldownBlockList;
    private MoveManager moveManager;
    private bool isSelected = false;
    private Image buttonImage;
    private Button button;
    private Color defaultColor;
    private Color selectedColor = Color.green;
    
    private RangeAttack rangeAttack;

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
            rangeAttack = player.GetComponent<RangeAttack>();

            switch(player.GetComponent<RangeAttack>().cooldownTurns)
            {
                case(0):
                    cooldownBlockList[0].color = Color.grey;
                    cooldownBlockList[1].color = Color.grey;
                    cooldownBlockList[2].color = Color.grey;
                    cooldownBlockList[3].color = Color.grey;
                    cooldownBlockList[4].color = Color.grey;
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
                case(4):
                    cooldownBlockList[3].color = Color.black;
                    break;
                case(5):
                    cooldownBlockList[4].color = Color.black;
                    break;
            }

            if (player.GetComponent<RangeAttack>().cooldownTurns < 5)
            {
                button.interactable = false;  // Make button non-interactable
                buttonImage.color = new Color(190,190,190);
            }
            else
            {
                button.interactable = true;  // Enable button when cooldown is at 5

                if (player.GetComponent<RangeAttack>().isReady == false)
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

        if (isSelected)
        {
            moveManager.GetComponent<MoveManager>().AddMoveToQueue(rangeAttack);
        }

        Debug.Log("Range Attack Selected: " + isSelected);

        GameManager.Instance.UpdateGameState(GameManager.GameState.Enemyturn);
    }
}
