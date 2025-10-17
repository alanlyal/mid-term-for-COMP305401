using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : EnemyBaseState
{
    public PatrolState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.CheckForPlayer() && enemy.stats.doChargeState)
        {
            enemy.SwitchState(enemy.playerDetectedState);
        }
        else
        {
            if (enemy.CheckForMeleeTarget())
            {
                enemy.SwitchState(enemy.meleeAttackState);
            }
        }

        if (enemy.CheckForObstacles())
        {
            enemy.Rotate();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        enemy.rb.velocity = new Vector2(enemy.facingDirection * enemy.stats.moveSpeed, enemy.rb.velocity.y);
    }
}
