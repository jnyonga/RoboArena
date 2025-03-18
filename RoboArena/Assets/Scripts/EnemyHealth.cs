using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int maxHealth;

    public GameObject[] healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        switch(health)
        {
            case(2):
                healthBar[2].SetActive(false);
                break;
            case(1):
                healthBar[2].SetActive(false);
                healthBar[1].SetActive(false);
                break;
            case(0):
                healthBar[2].SetActive(false);
                healthBar[1].SetActive(false);
                healthBar[0].SetActive(false);
                break;
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
}
