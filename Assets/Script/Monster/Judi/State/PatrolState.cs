using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterState
{
    public class PatrolState : StateBase<Judi>
    {
        private Transform curPatrolPoint;
        private int patrol_Index;
        public PatrolState(Judi owner) : base(owner)
        {
        }
        public override void Setup()
        {
        }

        public override void Enter()
        {
            patrol_Index = 0;
            owner.SwitchStepSounds(0);  // 0�� ������ �ȴ� �Ҹ��� ����ǰ�
            owner.Anim.SetBool("Patrol", true);
            curPatrolPoint = owner.PatrolPoints[patrol_Index];
            SetDestination();
        }
        public override void Update()
        {
            if (Vector3.Distance(new Vector3(owner.transform.position.x, 0, owner.transform.position.z), new Vector3(curPatrolPoint.position.x, 0, curPatrolPoint.position.z)) < 0.1f)
            {
                Debug.Log($"{patrol_Index}/{owner.PatrolPoints.Length} ����");
                patrol_Index++;
                if (patrol_Index == owner.PatrolPoints.Length)
                    patrol_Index = 0;
                curPatrolPoint = owner.PatrolPoints[patrol_Index];
                SetDestination();
            }
            // �÷��̾� ������ ���ݱ���
        }

        public void SetDestination()
        {
            owner.Agent.destination = curPatrolPoint.position;
        }

        public override void Exit()
        {
            owner.Anim.SetBool("Patrol", false);
        }

        public override void LateUpdate()
        {
        }

        public override void Transition()
        {
            // ��Ʈ�� ����� ���� ����
        }
    }
}