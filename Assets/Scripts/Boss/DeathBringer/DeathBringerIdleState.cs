using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : BossState
{
    private DeathBringerBoss enemy;
    private Transform player;
    public DeathBringerIdleState(BossStateMachine stateMachine, Boss bossBase, string animBoolName, DeathBringerBoss DeathBringerBoss) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = DeathBringerBoss;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 20)
            enemy.bossFightBegun = true;

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.TeleportState);
        }

        if (stateTimer < 0 && enemy.bossFightBegun)
            stateMachine.ChangeState(enemy.BattleState);
    }
}
