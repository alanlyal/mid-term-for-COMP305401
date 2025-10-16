using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeathState : EnemyBaseState
{
    public DeathState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enemy.Instantiate(enemy.stats.deathParticle);
        enemy.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
