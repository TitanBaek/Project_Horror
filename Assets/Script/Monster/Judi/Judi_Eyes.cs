using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judi_Eyes : MonoBehaviour
{
    private Judi judi;
    // 몬스터 시야구현
    [SerializeField] float range;
    [SerializeField, Range(0f, 360f)] float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    private float cosResult;

    private void Awake()
    {        
        judi = GetComponentInParent<Judi>();
        cosResult = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
    }

    private void Update()
    {
        if (judi.CurState != M_State.Chase && judi.CurState != M_State.Attack)
        {
            FindTarget();
        }
    }

    public void FindTarget()
    {
        // 1. 범위에 들어옴
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetMask);
        foreach (Collider collider in colliders)
        {
            // 2. 앞에 있는지 확인
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized; // collider 의 위치(Vector3) ... 
            if (Vector3.Dot(transform.forward, dirTarget) < cosResult)
            {
                continue;
            }
            // 3. 중간에 장애물이 없는지
            float distToTarget = Vector3.Distance(transform.position, collider.transform.position);
            if (Physics.Raycast(transform.position, dirTarget, out var hit, distToTarget, obstacleMask))
            {
                Debug.DrawRay(transform.position, dirTarget * hit.distance, Color.yellow);
                Debug.Log(hit.collider.gameObject.name);
                continue;
            }

            Debug.Log("찾았다");
            Debug.DrawRay(transform.position, dirTarget * distToTarget, Color.green);
            judi.ChangeState(M_State.Chase);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f); // 대상이 바라보고 있는 각도 + 앵글의 1/2
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);  // 대상이 바라보고 있는 각도 - 앵글의 1/2
        Debug.DrawRay(transform.position, rightDir * range, Color.yellow);
        Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
