using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterState
{
    public class AttackState : StateBase<Judi>
    {
        private float coolTime;
        private bool isAttack = false;
        private float cosResult;
        private bool attackFinish;
        public AttackState(Judi owner) : base(owner)
        {
            cosResult = Mathf.Cos(owner.AttackAngle * 0.5f * Mathf.Deg2Rad);
        }

        public override void Setup()
        {
        }

        public override void Enter()
        {
            owner.Agent.speed = 1;
            attackFinish = false;
            Attack();
        }

        public override void Update()
        {
        }

        public override void LateUpdate()
        {
        }
        public void Attack()
        {
            // 1. ������ ����
            Collider[] colliders = Physics.OverlapSphere(owner.transform.position, owner.AttackRange);
            foreach (Collider collider in colliders)
            {
                // 2. �տ� �ִ��� Ȯ��
                Vector3 dirTarget = (collider.transform.position - owner.transform.position).normalized; // collider �� ��ġ(Vector3) ... 
                if (Vector3.Dot(owner.transform.forward, dirTarget) < cosResult)
                {
                    // ���� ���� �ۿ� �ִ�.
                    continue;
                }
                else
                {
                    IHitable hitable = collider.GetComponent<IHitable>();
                    if (Vector3.Dot(owner.transform.forward, dirTarget) > cosResult)
                        owner.Attack_coroutine = owner.StartCoroutine(DoAttackAnimation(hitable));
                }
            }
            owner.DoChase_Coroutine = owner.StartCoroutine(DoChase());
        }

        public IEnumerator DoAttackAnimation(IHitable hitable)
        {
            hitable?.Stun();                            // ĳ���� ������ ����
            owner.Anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1f);
            owner.Anim.SetTrigger("Attack");
        }

        public IEnumerator DoChase()
        {
            yield return new WaitForSeconds(1f);
            attackFinish = true;
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.DoChase_Coroutine);
            owner.StopCoroutine(owner.Attack_coroutine);
        }

        public override void Transition()
        {
            if (attackFinish)
            {
                owner.ChangeState(M_State.Chase);
            }
        }
    }

}
