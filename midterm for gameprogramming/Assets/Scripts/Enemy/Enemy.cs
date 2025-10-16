using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    public EnemyBaseState currentState;

    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;

    public EnemyStats stats;
    public Rigidbody2D rb;
    public Transform ledgeDetector;

    public bool facingRight = true;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        patrolState = new PatrolState(this, "patrol");
        playerDetectedState = new PlayerDetectedState(this, "playerDetected");
        rb = GetComponent<Rigidbody2D>();

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
        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, facingRight ? Vector2.right : Vector2.left, stats.obstacleDistance, stats.ObstacleLayer);

        if (hit.collider == null || hitObstacle == true) { return true; }
        else { return false; }
    }

    public bool CheckForPlayer()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingRight ? Vector2.right : Vector2.left, stats.playerDetectDistance, stats.PlayerLayer);

        if (hitPlayer.collider == true) { return true; }
        else { return false; }
    }

    #endregion

    #region Other Functions

    public void SwitchState(EnemyBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ledgeDetector.position, (facingRight ? Vector2.right : Vector2.left) * stats.playerDetectDistance);
    }

    #endregion
}
