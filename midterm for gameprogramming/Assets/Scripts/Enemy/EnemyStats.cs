using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Patrol State")]
    public float moveSpeed = 5f;
    public float edgeCheckDistance = 1f;
    public float obstacleDistance = 0.5f;
    public LayerMask GroundLayer;
    public LayerMask ObstacleLayer;

    [Header("Player Detection")]
    public float playerDetectDistance = 5f;
    public float playerDetectedWaitTime = 1f;
    public LayerMask PlayerLayer;

    [Header("Charge State")]
    public float chargeTime = 2f;
    public float chargeSpeed = 5f;
    public bool doChargeState;

    [Header("Melee Attack State")]
    public float damage = 1f;
    public float meleeDetectDistance = 0.5f;
    public float attackCooldown = 1f;
    public Vector2 knockbackAngle;
    public float knockbackForce;
    public LayerMask DamageableLayer;

    [Header("Health")]
    public float maxHealth = 3f;
    public float stunTime = 0.5f;

    [Header("Prefabs")]
    public GameObject deathParticle;
}
