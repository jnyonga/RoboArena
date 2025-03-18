using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10;
    private int maxHealth = 10;
    private int healthRegen = 1;

    public int power = 100;
    private int powerLoss = 1;
    private int i = 0;
    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
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
        health += healthRegen;
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

}
