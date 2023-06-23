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
        Debug.Log("���� ����");
        owner.Agent.speed = 0f;
        owner.Anim.SetBool("Chase", true);
    }
    public override void Update()
    {
        owner.Agent.destination = owner.PlayerPos.position;
        if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) > owner.ChaseRange)        // �÷��̾ ChaseRange���� ����� Return(Patrol�� �ص� �ɵ�..)���� ���� ����
        {
            owner.ChangeState(State.Patrol);
        }
        else if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) < owner.AttackRange)  // �÷��̾ ���ݹ����� ������ Attack���� ���� ����
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