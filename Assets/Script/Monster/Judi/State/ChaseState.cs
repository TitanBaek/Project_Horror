using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterState
{
    public class ChaseState : StateBase<Judi>
    {
        Coroutine setSpeedCoroutine;

        public ChaseState(Judi owner) : base(owner)
        {
        }

        public override void Setup()
        {
        }

        public override void Enter()
        {
            Debug.Log("추적 시작");
            //owner.Player_State.SpottedByEnemy(); // 플레이어 추격중으로 상태 변경
            owner.PlayerScreamSound(1);
            owner.SwitchStepSounds(1);  // 1을 보내서 뛰는 소리가 재생되게
            owner.Agent.speed = 0f;
            owner.Anim.SetBool("Chase", true);
            setSpeedCoroutine = owner.StartCoroutine(DoSpeedUp());
        }

        public override void Update()
        {

            owner.Agent.destination = owner.PlayerPos.position;
            if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) > owner.ChaseRange)        // 플레이어가 ChaseRange에서 벗어나면 Return(Patrol로 해도 될듯..)으로 상태 변경
            {
                owner.ChangeState(M_State.Patrol);
            }
            else if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) < owner.AttackRange)  // 플레이어가 공격범위에 들어오면 Attack으로 상태 변경
            {
                owner.ChangeState(M_State.Attack);
            }
        }

        public override void Exit()
        {
            owner.Anim.SetBool("Chase", false);
            owner.PlayerScreamSound(0);
            owner.StopCoroutine(DoSpeedUp());
        }

        public override void LateUpdate()
        {
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(owner.transform.position, owner.AttackRange);

            Vector3 rightDir = AngleToDir(owner.transform.eulerAngles.y + owner.AttackAngle * 0.5f); // 대상이 바라보고 있는 각도 + 앵글의 1/2
            Vector3 leftDir = AngleToDir(owner.transform.eulerAngles.y - owner.AttackAngle * 0.5f);  // 대상이 바라보고 있는 각도 - 앵글의 1/2
            Debug.DrawRay(owner.transform.position, rightDir * owner.AttackRange, Color.red);
            Debug.DrawRay(owner.transform.position, leftDir * owner.AttackRange, Color.red);
        }

        private Vector3 AngleToDir(float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
        }

        public override void Transition()
        {
        }

        public IEnumerator DoSpeedUp()
        {
            yield return new WaitForSeconds(1.5f);
            owner.Agent.speed = 3f;
        }
    }
}