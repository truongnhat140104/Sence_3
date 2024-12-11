using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    public BossState currentState;
    // Start is called before the first frame update
    public void Initialize(BossState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(BossState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
