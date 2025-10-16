using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerGun : MonoBehaviour
{
    /// <shot gun power up>
    public GunStats newGunStats;
    /// </summary>
    public GunStats stats;
    private float nextFireTime;
    private PlayerMovement playerMove;
    public Transform firePoint;
    private void Awake()
    {
        playerMove = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (InputManager.AttackWasPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + stats.fireRate;
        }
    }

    void Shoot()
    {
        for (int i = 0; i < stats.bulletsPerShot; i++)
        {
            float angle = Random.Range(-stats.spreadAngle / 2, stats.spreadAngle / 2);
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            GameObject bulletObj = Instantiate(stats.bulletPrefab, firePoint.position, rotation);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.Init(stats.bulletDamage, stats.bulletSpeed, stats.bulletLifeTime, stats.KBForce, stats.KBAngle, stats.DamageableLayer, playerMove.isFacingRight);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerGun playerGun = collision.GetComponent<PlayerGun>(); ;
            if (playerGun != null)
            {
                Debug.Log("new gun has been picked up");
                playerGun.stats = newGunStats;
            }
            Destroy(gameObject);
        }
    }
}
