using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected Transform jumpCheck;

    [Header("Stunned info")]
    public float stunDuration = 1;
    public Vector2 stunDirection = new Vector2(10, 12);
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed = 2.5f;
    public float idleTime = 2;
    public float battleTime = 7;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float agroDistance = 2;
    public float attackDistance;
    public float attackCooldown;
    public float minAttackCooldown = 1;
    public float maxAttackCooldown = 2;
    [SerializeField] private float jumpCheckDistance;
    [HideInInspector] public float lastTimeAttacked;

    public BossStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }
    private PlayerMove player;
    public string lastAnimBoolName { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new BossStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();

        fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();


    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;


    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    public virtual RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsGround);

        if (wallDetected)
        {
            if (wallDetected.distance < playerDetected.distance)
                return default(RaycastHit2D);
        }

        return playerDetected;
    }
    public virtual bool CanJump()
    {
        Vector2 direction = Vector2.right * facingDir;
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, direction, 0.8f, whatIsGround);
        RaycastHit2D jumpDetected = Physics2D.Raycast(jumpCheck.position, direction, 0.8f, whatIsGround);

        // V? tia Raycast trong c?nh ?? debug
        Debug.DrawRay(wallCheck.position, direction * 0.8f, Color.yellow);
        Debug.DrawRay(jumpCheck.position, direction * 0.8f, Color.red);

        // Log th?ng tin raycast
        //Debug.Log($"Wall Detected: {wallDetected.collider != null}, Jump Detected: {jumpDetected.collider != null}");

        if (wallDetected && !jumpDetected)
        {
            //Debug.Log("Can Jump: True");
            return true;
        }
        //Debug.Log("Can Jump: False");
        return false;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(jumpCheck.position, new Vector3(jumpCheck.position.x + jumpCheckDistance * facingDir, jumpCheck.position.y));
    }
}
