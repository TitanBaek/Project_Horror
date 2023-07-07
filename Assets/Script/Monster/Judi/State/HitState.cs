using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 상태머신 Hit,Idle 따로 구분해서 빼버리고 
// Hit 애니메이션은 상체만 적용되게 바꾸자 
// 즉 Chase 도중 서브 상태 머신이 Hit로 바뀌게 해서 상체만 대미지를 받고 주춤하게 만들어주는 방식으로 ..
// Player 도 그런식으로 수정해줘야할까..?
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
            Debug.Log("스피드 0로");
            prevSpeed = owner.Agent.speed;
            owner.Agent.speed = 0;
            owner.Anim.SetTrigger("Hit");
            CameraShake.Instance.ShakeCamera(0.5f, .1f);
            hitCoroutine = owner.StartCoroutine(HitFinish());
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            hitFinished = false;
            owner.StopCoroutine(hitCoroutine);
        }

        public override void LateUpdate()
        {
        }

        public override void Transition()
        {
            if (hitFinished)
            {
                owner.Agent.speed = prevSpeed == 0 ? 3.5f : prevSpeed;
                Debug.Log($"스피드 {owner.Agent.speed}로");
                owner.ChangeState(M_SubState.Idle);
                if(owner.CurState != M_State.Chase)
                {
                    // 현재 몬스터 상태가 추적중이 아니라면 추적중으로 바꿔주자 
                    owner.ChangeState(M_State.Chase);
                }
            }
        }
          
        public IEnumerator HitFinish()
        {
            yield return new WaitForSeconds(1.5f);
            hitFinished = true;
        }
    }
}