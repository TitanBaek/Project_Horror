using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AttackState : StateBase<Judi>
{
    private bool isAttack;
    private float cosResult;
    public AttackState(Judi owner) : base(owner)
    {
    }

    public override void Setup()
    {
    }

    public override void Enter()
    {
        isAttack = false;
        owner.Anim.SetBool("Attack", true);
    }
    public override void Update()
    {

        if(!isAttack)
        {
            // 공격 실행
            Attack();
        }
    }

    public override void LateUpdate()
    {
        //owner.transform.LookAt(owner.PlayerPos);
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
            //if (Vector3.Dot(transform.forward, dirTarget) < 0)// 내적 구해주는 함수
            {
                // 공격 범위 밖에 있다.
                owner.ChangeState(State.Chase);
            }
            else
            {
                // 앞에 있다.                
                IHitable hitable = collider.GetComponent<IHitable>();
                hitable?.TakeHit(owner.Dmg);
                Debug.Log("때림");
            }

        }
    }
    public override void Exit()
    {
        owner.Anim.SetBool("Attack", false);
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
}
