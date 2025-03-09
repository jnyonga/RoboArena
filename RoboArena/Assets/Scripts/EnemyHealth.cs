using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
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
}
