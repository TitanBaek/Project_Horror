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
            Debug.Log("���� ����");
            //owner.Player_State.SpottedByEnemy(); // �÷��̾� �߰������� ���� ����
            owner.PlayerScreamSound(1);
            owner.SwitchStepSounds(1);  // 1�� ������ �ٴ� �Ҹ��� ����ǰ�
            owner.Agent.speed = 0f;
            owner.Anim.SetBool("Chase", true);
            setSpeedCoroutine = owner.StartCoroutine(DoSpeedUp());
        }

        public override void Update()
        {

            owner.Agent.destination = owner.PlayerPos.position;
            if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) > owner.ChaseRange)        // �÷��̾ ChaseRange���� ����� Return(Patrol�� �ص� �ɵ�..)���� ���� ����
            {
                owner.ChangeState(M_State.Patrol);
            }
            else if (Vector3.Distance(owner.PlayerPos.position, owner.transform.position) < owner.AttackRange)  // �÷��̾ ���ݹ����� ������ Attack���� ���� ����
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

            Vector3 rightDir = AngleToDir(owner.transform.eulerAngles.y + owner.AttackAngle * 0.5f); // ����� �ٶ󺸰� �ִ� ���� + �ޱ��� 1/2
            Vector3 leftDir = AngleToDir(owner.transform.eulerAngles.y - owner.AttackAngle * 0.5f);  // ����� �ٶ󺸰� �ִ� ���� - �ޱ��� 1/2
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