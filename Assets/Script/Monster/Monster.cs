using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour
{

    // ��Ʈ�� ���� ����
    [SerializeField] private Transform[] patrolPoints;

    [SerializeField] protected GameObject MonsterHead;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int curHp;
    [SerializeField] protected int dmg;

    protected Animator anim;
    protected NavMeshAgent agent;

    // �÷��̾� ü�̽� ���� ����
    protected Transform playerPos;
    [SerializeField] protected float chaseRange;
    
    // ���� ���¿� ���Ǵ� ����
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCoolTime;
    [SerializeField] protected float attackAngle;

    public virtual void Awake()
    {
        curHp = maxHp;
        Debug.Log($"�� ������ ü���� {curHp}/{maxHp} �Դϴ�.");
    }
    public Transform[] PatrolPoints { get { return patrolPoints; } }
    public int MaxHp { get { return maxHp; } }
    public int CurHp { get { return curHp; } }
    public int Dmg { get { return dmg; } }
    public Animator Anim { get { return anim; } set { anim = value; } }
    public NavMeshAgent Agent { get { return agent; } set { agent = value; } }
    public Transform PlayerPos { get { return playerPos; } }
    public float ChaseRange { get { return chaseRange;  } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } } 
    public float AttackCoolTime { get {  return attackCoolTime; } set {  attackCoolTime = value; } }
    public float AttackAngle { get { return attackAngle; } }

}
