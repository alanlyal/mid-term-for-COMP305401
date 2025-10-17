using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyBaseState
{
    public IdleState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!enemy.CheckForGround())
        {
            enemy.SwitchState(enemy.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
