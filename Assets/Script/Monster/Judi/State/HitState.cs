using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���¸ӽ� Hit,Idle ���� �����ؼ� �������� 
// Hit �ִϸ��̼��� ��ü�� ����ǰ� �ٲ��� 
// �� Chase ���� ���� ���� �ӽ��� Hit�� �ٲ�� �ؼ� ��ü�� ������� �ް� �����ϰ� ������ִ� ������� ..
// Player �� �׷������� ����������ұ�..?
namespace MonsterState
{
    public class HitState : StateBase<Judi>
    {
        Coroutine hitCoroutine;
        bool hitFinished;
        float prevSpeed;

        public HitState(Judi owner) : base(owner)
        {
        }
        public override void Setup()
        {
        }

        public override void Enter()
        {
            prevSpeed = owner.Agent.speed;
            owner.Agent.speed = 0;
            owner.Anim.SetTrigger("Hit");            
        }
        public override void Update()
        {
        }
        public override void Exit()
        {
        }

        public override void LateUpdate()
        {
        }

        public override void Transition()
        {
            if (hitFinished)
            {
                owner.Agent.speed = prevSpeed;
                owner.ChangeState(M_SubState.Idle);
                if(owner.CurState != M_State.Chase)
                {
                    // ���� ���� ���°� �������� �ƴ϶�� ���������� �ٲ����� 
                    owner.ChangeState(M_State.Chase);
                }
            }
        }

        public IEnumerator HitFinish()
        {
            yield return new WaitForSeconds(0.5f);
            hitFinished = true;
        }
    }
}