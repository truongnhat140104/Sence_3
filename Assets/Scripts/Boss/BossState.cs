using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState
{
    protected BossStateMachine stateMachine;
    protected Boss BossBase;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public BossState(BossStateMachine stateMachine, Boss bossBase, string animBoolName)
    {
        this.stateMachine = stateMachine;
        BossBase = bossBase;
        this.animBoolName = animBoolName;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }
    public virtual void Enter()
    {
        triggerCalled = false;
        rb = BossBase.rb;
        BossBase.anim.SetBool(animBoolName, true);
    }
    public virtual void Exit()
    {
        BossBase.anim.SetBool(animBoolName, false);
        BossBase.AssignLastAnimName(animBoolName);
    }
    // Update is called once per frame
    public virtual void Update()
    {
       stateTimer -= Time.deltaTime;
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
