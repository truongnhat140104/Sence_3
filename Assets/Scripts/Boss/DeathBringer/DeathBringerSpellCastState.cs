using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : BossState
{
    private DeathBringerBoss enemy;

    private int amountOfSpells;
    private float spellTimer;
    public DeathBringerSpellCastState(BossStateMachine stateMachine, Boss bossBase, string animBoolName, DeathBringerBoss enemy) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.stats.currentHealth <= enemy.stats.maxHealth / 2)
        {
            amountOfSpells = 7;
        }
        else
        {
            amountOfSpells = enemy.amountOfSpells;
        }
        spellTimer = 0.5f;
    }

    public override void Update()
    {
        base.Update();
        spellTimer -= Time.deltaTime;
        if (canCast())
        {
            enemy.CastSpell();
        }
        if (amountOfSpells <= 0)
        {
            stateMachine.ChangeState(enemy.TeleportState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }
    private bool canCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }
}
