using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterState
{
    public class DieState : StateBase<Judi>
    {
        public DieState(Judi owner) : base(owner)
        {
        }
        public override void Setup()
        {
        }

        public override void Enter()
        {
            owner.Anim.SetBool("Die", true);
            owner.Agent.speed = 0;
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
        }
    }
}