using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterState
{
    public class HitState : StateBase<Judi>
    {
        Coroutine hitCoroutine;
        bool hitFinished;

        public HitState(Judi owner) : base(owner)
        {
        }
        public override void Setup()
        {
        }

        public override void Enter()
        {
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
                owner.ChangeState(M_State.Chase);
            }
        }

        public IEnumerator HitFinish()
        {
            yield return new WaitForSeconds(0.5f);
            hitFinished = true;
        }
    }
}