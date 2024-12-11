using UnityEngine;

public class DeathBringerBattleState : BossState
{
    private Transform player;
    private PlayerMove playerState;
    private DeathBringerBoss enemy;
    private int moveDir;

    private bool flippedOnce;

    public DeathBringerBattleState(BossStateMachine stateMachine, Boss bossBase, string animBoolName, DeathBringerBoss _enemy) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = _enemy;
    }

    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
        playerState = PlayerManager.instance.player;

        //if (player.GetComponent<Cat>().isDead)
        //stateMachine.ChangeState(enemy.moveState);

        stateTimer = enemy.battleTime;
        flippedOnce = false;
    }

    public override void Update()
    {
        base.Update();
        if (playerState.isDead)
        {
            enemy.bossFightBegun = false;
            enemy.stateMachine.ChangeState(enemy.IdleState);
        }
        if (enemy.CanJump())
        {
            enemy.SetVelocity(enemy.rb.velocity.x, 7);
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.TeleportState);
        }

        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.AttackState);
            }
        }

        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);

        if (distanceToPlayerX < .8f)
            return;

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;


        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
