using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float health = 10;
    private float maxHealth = 10;
    private int healthRegen = 1;

    public float power = 100;
    private float maxPower = 100;
    private int powerLoss = 1;
    private int i = 0;

    [Header("Object References")]
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject powerBar;
    private TextMeshProUGUI healthTXT;
    private TextMeshProUGUI powerTXT;
    
    void Start()
    {
        health = maxHealth;
        power = maxPower;

        healthBar = GameObject.FindGameObjectWithTag("Health");
        healthTXT = GameObject.FindGameObjectWithTag("TXThealth").GetComponent<TextMeshProUGUI>();
        powerBar = GameObject.FindGameObjectWithTag("Power");
        powerTXT = GameObject.FindGameObjectWithTag("TXTpower").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        PowerBarFiller();
        HealthBarFiller();

        if(Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(1);
        }

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void TakeDamage(int damage)
    {
        if(health != 0)
        {
            health -= damage;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void HealthPerTurn()
    {
        if (health < maxHealth)
        {
            health += healthRegen;
        }
    }

    public void PowerPerTurn()
    {
        while (i < 10)
        {
            if(i == 9)
            {
                power -= powerLoss;
                i = 0;
                return;
            }

            i++;
        }
        
    }

    void HealthBarFiller()
    {
        healthBar.GetComponent<Image>().fillAmount = health / maxHealth;
        healthTXT.text = health.ToString("F0");
    }

    void PowerBarFiller()
    {
        powerBar.GetComponent<Image>().fillAmount = power / maxPower;
        powerTXT.text = power.ToString("F0");
    }

}
