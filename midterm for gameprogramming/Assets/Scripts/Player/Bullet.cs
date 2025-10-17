using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private float speed;
    private float lifetime;
    private float spawnTime;
    private bool facingRight;
    private float KBForce;
    private Vector2 KBAngle;

    private LayerMask damageableLayer;

    private Rigidbody2D rb;

    public void Init(float damage, float speed, float lifetime, float KBForce, Vector2 KBAngle, LayerMask dameableLayer, bool facingRight)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.KBForce = KBForce;
        this.KBAngle = KBAngle;
        this.damageableLayer = dameableLayer;
        this.facingRight = facingRight;
        spawnTime = Time.time;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate((facingRight ? Vector2.right : Vector2.left) * ((facingRight ? 1 : -1) * speed) * Time.deltaTime);
        if (Time.time >= spawnTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((damageableLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable != null)
            {
                Destroy(gameObject);
                damageable.Damage(damage, KBForce, new Vector2(KBAngle.x * (facingRight ? 1 : -1), KBAngle.y));
            }
        }
    }
}
