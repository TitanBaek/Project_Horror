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
        // 1. 범위에 들어옴
        Collider[] colliders = Physics.OverlapSphere(owner.transform.position, owner.AttackRange);
        foreach (Collider collider in colliders)
        {
            // 2. 앞에 있는지 확인
            Vector3 dirTarget = (collider.transform.position - owner.transform.position).normalized; // collider 의 위치(Vector3) ... 
            if (Vector3.Dot(owner.transform.forward, dirTarget) < cosResult)
            {
                // 공격 범위 밖에 있다.
                continue;
            }
            else
            {
                Debug.Log("때림");                
                IHitable hitable = collider.GetComponent<IHitable>();
                if(Vector3.Dot(owner.transform.forward, dirTarget) > cosResult)
                    attack_coroutine = owner.StartCoroutine(DoAttackAnimation(hitable));

            }
        }
        doChase_coroutine = owner.StartCoroutine(DoChase());
    }

    public IEnumerator DoAttackAnimation(IHitable hitable)
    {
        hitable?.Stun();                            // 캐릭터 움직임 멈춤
        owner.Anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        owner.Anim.SetBool("Attack", false);
        hitable?.TakeHit(15);                       // 캐릭터 대미지 주기와 움직임 가능하게
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
