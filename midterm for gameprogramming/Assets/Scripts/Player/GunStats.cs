using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun Stats")]

public class GunStats : ScriptableObject
{
   
    [Header("Stats")]
    public float fireRate = 0.2f;
    public int bulletsPerShot = 1;
    public float spreadAngle = 5f;
    public float bulletSpeed = 10f;
    public float bulletDamage = 1f;
    public float bulletLifeTime = 2f;

    public float KBForce = 0f;
    public Vector2 KBAngle = Vector2.zero;

    [Header("References")]
    public GameObject bulletPrefab;
    public LayerMask DamageableLayer;
}
