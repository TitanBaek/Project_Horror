using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public enum State { Idle, Chase, Return, Attack, Patrol, Hit, Die, Size }
public class Judi : Monster
{
    // 상태머신 구현
    private State curState;
    public State CurState { get { return curState;  } }
    private StateBase<Judi>[] states;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        states = new StateBase<Judi>[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Chase] = new ChaseState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Patrol] = new PatrolState(this);
        states[(int)State.Hit] = new HitState(this);
        states[(int)State.Die] = new DieState(this);
        curState = State.Idle;
    }

    private void Start()
    {
        ChangeState(State.Idle);                
    }
    private void Update()
    {
       states[(int)curState].Update();         // 현재 상태에 대한 Update함수 호출
    }

    private void LateUpdate()
    {
        states[(int)curState].LateUpdate();
    }

    public void ChangeState(State state)
    {
        Debug.Log(state);
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Setup();
        states[(int)curState].Enter();
    }
    public void ScreamDone()
    {
        agent.speed = 3.5f;
    }
}