using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class ChaseState : StateBase<Player>
    {

        public ChaseState(Player owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("플레이어는 지금 추격 받는 중!");
        }

        public override void Exit()
        {
        }

        public override void LateUpdate()
        {
        }

        public override void Setup()
        {
        }

        public override void Transition()
        {
        }

        public override void Update()
        {
        }
    }
}
