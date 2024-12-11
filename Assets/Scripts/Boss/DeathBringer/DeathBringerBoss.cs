using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DeathBringerBoss : Boss
{
    // Start is called before the first frame update
    #region State
    public DeathBringerIdleState IdleState { get; set; }
    public DeathBringerAttackState AttackState { get; internal set; }
    public DeathBringerBattleState BattleState { get; internal set; }
    public DeathBringerDeadState DeadState { get; internal set; }
    public DeathBringerSpellCastState SpellCastState { get; internal set; }
    public DeathBringerTeleportState TeleportState { get; internal set; }
    #endregion
    public bool bossFightBegun;
    [Header("CastSpellDetail")]
    [SerializeField] private GameObject SpellPrefar;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;
    [SerializeField] private Vector2 spellOffset;

    [Header("teleport detail")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport = 30;
    public float defaultChanceToTeleport;


    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private Vector2 damageAreaSize = new Vector2(2f, 2f);
    [SerializeField] private Vector2 damageAreaOffset = Vector2.zero;
    [SerializeField] private float damageInterval = 1f; // Time between damages
    private float lastDamageTime;

    protected override void Awake()
    {
        base.Awake();
        SetupDefailtFacingDir(-1);
        IdleState = new DeathBringerIdleState(stateMachine, this, "Idle", this);
        AttackState = new DeathBringerAttackState(stateMachine, this, "Attack", this);
        DeadState = new DeathBringerDeadState(stateMachine, this, "Idle", this);
        BattleState = new DeathBringerBattleState(stateMachine, this, "Move", this);
        SpellCastState = new DeathBringerSpellCastState(stateMachine, this, "CastSpell", this);
        TeleportState = new DeathBringerTeleportState(stateMachine, this, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        CheckForPlayerOverlap();
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(DeadState);
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            //Debug.Log("Looking for new position");
            FindPosition();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);

        Gizmos.color = Color.red;
        Vector2 position = (Vector2)transform.position + damageAreaOffset;
        Gizmos.DrawWireCube(position, damageAreaSize);
    }
    public bool CanTeleport()
    {
        if (Random.Range(1, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        return false;
    }
    public bool CanDoSpellCast()
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            lastTimeCast = Time.time;
            return true;
        }
        return false;
    }

    public void CastSpell()
    {
        PlayerMove player = PlayerManager.instance.player;

        float xOffset = 0;

        if (player.rb.velocity.x != 0)
            xOffset = player.dirX * spellOffset.x;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + spellOffset.y);

        GameObject newSpell = Instantiate(SpellPrefar, spellPosition, Quaternion.identity);
        newSpell.GetComponent<DeathBringerSpell_Controller>().SetupSpell(stats);
    }

    private void CheckForPlayerOverlap()
    {
        if (Time.time < lastDamageTime + damageInterval)
            return;

        Vector2 position = (Vector2)transform.position + damageAreaOffset;
        Collider2D hit = Physics2D.OverlapBox(position, damageAreaSize, 0f, whatIsPlayer);

        if (hit != null)
        {
            PlayerMove player = hit.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.OnDamaged(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }
}
