using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public bool canFly = false;

    [Header("Jump")]
    public bool canJump = true;
    public float jumpForce = 5f;

    [Header("Detection")]
    public float playerDetectDistance = 5f;
    public float edgeCheckDistance = 2f;
    public float obstacleDistance = 5f;
    public LayerMask GroundLayer;
    public LayerMask ObstacleLayer;
    public LayerMask PlayerLayer;

    [Header("Attack")]
    public float damage = 1f;

    [Header("Health")]
    public float health = 3f;
}
