using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase<Judi>
{
    private float coolTime;
    private bool isAttack = false;
    private float cosResult;
    private Coroutine attack_coroutine;
    private Coroutine doChase_coroutine;
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
                Debug.Log("����");                
                IHitable hitable = collider.GetComponent<IHitable>();
                if(Vector3.Dot(owner.transform.forward, dirTarget) > cosResult)
                    attack_coroutine = owner.StartCoroutine(DoAttackAnimation(hitable));

            }
        }
        doChase_coroutine = owner.StartCoroutine(DoChase());
    }

    public IEnumerator DoAttackAnimation(IHitable hitable)
    {
        hitable?.Stun();                            // ĳ���� ������ ����
        owner.Anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        owner.Anim.SetBool("Attack", false);
        hitable?.TakeHit(15);                       // ĳ���� ����� �ֱ�� ������ �����ϰ�
    }

    public IEnumerator DoChase()
    {
        yield return new WaitForSeconds(1f);
        owner.ChangeState(State.Chase);
    }

    public override void Exit()
    {
        owner.StopCoroutine(doChase_coroutine);
        owner.StopCoroutine(attack_coroutine);
        owner.Anim.SetBool("Attack", false);
    }

    public override void Transition()
    {
    }
}
