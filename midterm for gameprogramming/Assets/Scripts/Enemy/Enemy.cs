using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    #region Variables

    public EnemyBaseState currentState;

    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public ChargeState chargeState;
    public MeleeAttackState meleeAttackState;
    public DamagedState damagedState;
    public DeathState deathState;

    public EnemyStats stats;
    public Transform ledgeDetector;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public float stateTime; //keeps track of when we enter a state

    public int facingDirection = 1;
    public float currentHealth;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        patrolState = new PatrolState(this, "patrol");
        playerDetectedState = new PlayerDetectedState(this, "playerDetected");
        chargeState = new ChargeState(this, "chargeState");
        meleeAttackState = new MeleeAttackState(this, "meleeAttack");
        damagedState = new DamagedState(this, "damaged");
        deathState = new DeathState(this, "death");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        currentHealth = stats.maxHealth;

        currentState = patrolState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    #endregion

    #region Checks

    public bool CheckForObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, stats.edgeCheckDistance, stats.GroundLayer);
        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.obstacleDistance, stats.ObstacleLayer);

        if (hit.collider == null || hitObstacle == true) { return true; }
        else { return false; }
    }

    public bool CheckForPlayer()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.playerDetectDistance, stats.PlayerLayer);

        if (hitPlayer.collider == true) { return true; }
        else { return false; }
    }

    public bool CheckForMeleeTarget()
    {
        RaycastHit2D hitMeleeTarget = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.meleeDetectDistance, stats.DamageableLayer);

        if (hitMeleeTarget.collider == true) { return true; }
        else { return false; }
    }

    #endregion

    #region Other Functions

    public void SwitchState(EnemyBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();

        stateTime = Time.time;
    }

    public void Rotate()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDirection = -facingDirection;
    }

    public void Instantiate(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    public void Damage(float damageAmount)
    {
    }

    public void Damage(float damageAmount, float KBForce, Vector2 KBAngle)
    {
        damagedState.KBForce = KBForce;
        damagedState.KBAngle = KBAngle;
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            SwitchState(deathState);
        }
        else
        {
            SwitchState(damagedState);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ledgeDetector.position, (facingDirection == 1 ? Vector2.right : Vector2.left) * stats.playerDetectDistance);
    }

    #endregion
}
