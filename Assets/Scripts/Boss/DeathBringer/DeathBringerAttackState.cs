using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : BossState
{
    private DeathBringerBoss enemy;
    public DeathBringerAttackState(BossStateMachine stateMachine, Boss bossBase, string animBoolName, DeathBringerBoss enemy) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.chanceToTeleport += 5;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        if (triggerCalled)
        {
            if (enemy.CanTeleport())
            {
                stateMachine.ChangeState(enemy.TeleportState);
            }
            else
            {
                stateMachine.ChangeState(enemy.BattleState);
            }
        }
    }
}
