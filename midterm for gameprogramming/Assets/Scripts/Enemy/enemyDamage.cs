using System;
using UnityEngine;

public class enemyDamage : MonoBehaviour
{
    public int damage = 1;
    public PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            playerHealth.takeDamage(damage);
        }
    }
}
