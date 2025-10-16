using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    //movement
    public float speed;
    private bool movingRight = true;
    public Transform groundDetection;
    ///////////
    //health
    public int health = 3;
    private int currentHealth;
    ///////////
    //damage
    public int damage = 1;
    private SpriteRenderer spriteRenderer;
    public Color damageColor = Color.red;
    private Color originalColor;
    public float damageflash = 0.1f;
    private float flashTimer = 0f;
    private bool flash = false;
    private void Start()
    {
        currentHealth = health;
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)// this part is temp remove later
        {
            originalColor = spriteRenderer.color;
        }
    }
    public void Update()
    {
        rb.velocity = new Vector2((movingRight ? 1 : -1) * speed, rb.velocity.y);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        if (groundInfo.collider == false)
        {
            if (movingRight)
            {
                transform.Rotate(0f, 180f, 0f);
                movingRight = false;
            }
            else
            {
                transform.Rotate(0f, 180f, 0f);
                movingRight = true;
            }
        }

        if (flash && spriteRenderer != null)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer < 0f)
            {
                spriteRenderer.color = originalColor;
                flash = false;
            }
        }
    }
    public void Damage(int damage)
    {
        currentHealth -= damage;
        FlashRed();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            flashTimer = damageflash;
            flash = true;
        }
    }


}
