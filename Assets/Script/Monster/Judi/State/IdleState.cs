using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterState
{
    public class IdleState : StateBase<Judi>
    {
        float idleTime;

        public IdleState(Judi owner) : base(owner)
        {
        }
        public override void Setup()
        {
        }
        public override void Enter()
        {
            idleTime = 0;
            owner.Anim.SetBool("Idle", true);
        }
        public override void Update()
        {
            idleTime += Time.deltaTime;
            if (idleTime > 10)
            {
                idleTime = 0;
                owner.ChangeState(State.Patrol);
            }
        }

        public override void Exit()
        {
            owner.Anim.SetBool("Idle", false);
        }

        public override void LateUpdate()
        {
        }

        public override void Transition()
        {
        }
    }
}