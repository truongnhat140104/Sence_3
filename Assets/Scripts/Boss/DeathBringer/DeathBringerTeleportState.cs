using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : BossState
{
    private DeathBringerBoss enemy;
    public DeathBringerTeleportState(BossStateMachine stateMachine, Boss bossBase, string animBoolName, DeathBringerBoss _enemy) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            if (enemy.CanDoSpellCast())
            {
                stateMachine.ChangeState(enemy.SpellCastState);
            }
            else { stateMachine.ChangeState(enemy.BattleState); }
        }
    }
    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvincible(false);
    }
}
