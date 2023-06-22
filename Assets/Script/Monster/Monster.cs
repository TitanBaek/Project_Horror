using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField] private GameObject MonsterHead;
    protected int maxHp;
    protected int curHp;

    protected int dmg;

    private void LateUpdate()
    {
        MonsterHead.transform.localRotation = Quaternion.Euler(Random.Range(-40, 40), Random.Range(-40, 40), Random.Range(-40, 40));
    }

    protected abstract void Attack();

}
