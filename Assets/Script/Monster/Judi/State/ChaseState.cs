using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : StateBase<Judi>
{
    public ChaseState(Judi owner) : base(owner)
    {
    }
    public override void Setup()
    {
    }

    public override void Enter()
    {
        Debug.Log("추적 시작");
        owner.Agent.speed = 0f;
        owner.Anim.SetBool("Chase", true);
    }
    public override void Update()
    {
        owner.Agent.destination = owner.PlayerPos.position;
        if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) > owner.ChaseRange)        // 플레이어가 ChaseRange에서 벗어나면 Return(Patrol로 해도 될듯..)으로 상태 변경
        {
            owner.ChangeState(State.Patrol);
        }
        else if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) < owner.AttackRange)  // 플레이어가 공격범위에 들어오면 Attack으로 상태 변경
        {
            owner.ChangeState(State.Attack);
        }
    }
    public override void Exit()
    {
        owner.Anim.SetBool("Chase", false);
    }

    public override void LateUpdate()
    {
    }
}