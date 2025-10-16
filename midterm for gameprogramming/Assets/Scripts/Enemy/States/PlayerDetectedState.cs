using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : EnemyBaseState
{
    public PlayerDetectedState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enemy.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!enemy.CheckForPlayer())
        {
            enemy.SwitchState(enemy.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
