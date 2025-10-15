using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    public HealthUI healthUI;

    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private float numOfFlashes;
    private SpriteRenderer spriteRend;

    private void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);

        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemyController enemy = collision.gameObject.GetComponent<enemyController>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    private void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthUI.UpdateHearts(currentHealth);
            StartCoroutine(Invunerability());
        }


        if (currentHealth <= 0)
        {
            //player is dead
        }
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < numOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0.5f, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numOfFlashes));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numOfFlashes));
        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }
}
