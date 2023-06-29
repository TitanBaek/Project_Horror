using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;
using MonsterState;

public enum M_State { Idle, Chase, Return, Attack, Patrol, Hit, Die, Size }
public class Judi : Monster
{
    // ���¸ӽ� ����
    private M_State curState;
    public M_State CurState { get { return curState; } }
    private StateBase<Judi>[] states;
    [SerializeField] private AudioClip[] footStepSounds;
    [SerializeField] private AudioClip[] footStepRunSounds;
    private AudioSource[] audioSource;
    private AudioClip[] StepSounds;
    [SerializeField] private AudioClip[] ScreamSounds;
    private int footStepIndex = 0;
    Coroutine judi_Coroutine;

    private void Awake()
    {
        audioSource = GetComponents<AudioSource>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        states = new StateBase<Judi>[(int)M_State.Size];
        states[(int)M_State.Idle] = new IdleState(this);
        states[(int)M_State.Chase] = new ChaseState(this);
        states[(int)M_State.Return] = new ReturnState(this);
        states[(int)M_State.Attack] = new AttackState(this);
        states[(int)M_State.Patrol] = new PatrolState(this);
        states[(int)M_State.Hit] = new HitState(this);
        states[(int)M_State.Die] = new DieState(this);
        curState = M_State.Idle;
        StepSounds = footStepSounds;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        ChangeState(M_State.Idle);
    }
    private void Update()
    {
        states[(int)curState].Update();         // ���� ���¿� ���� Update�Լ� ȣ��
    }

    public void ChangeState(M_State state)
    {
        Debug.Log(state);
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Setup();
        states[(int)curState].Enter();
    }

    public void ScreamDone()
    {
        agent.speed = 3.5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + AttackAngle * 0.5f); // ����� �ٶ󺸰� �ִ� ���� + �ޱ��� 1/2
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - AttackAngle * 0.5f);  // ����� �ٶ󺸰� �ִ� ���� - �ޱ��� 1/2
        Debug.DrawRay(transform.position, rightDir * AttackRange, Color.white);
        Debug.DrawRay(transform.position, leftDir * AttackRange, Color.white);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    public void PlayFootStep()
    {
        if (!audioSource[0].isPlaying)                         // ���� ������� ������� �ƴ϶�� ���
        {
            audioSource[0].clip = StepSounds[footStepIndex++]; // ����� ����� �ε����� ���� ������
            audioSource[0].Play();                                 // ����� ���
            if (footStepIndex >= 3)                         // �ε��� �ʱ�ȭ �κ�
                footStepIndex = 0;
        }
    }

    public void PlayerScreamSound(int mode)
    {
        // 1�� �Ѿ���� �ڷ�ƾ �����ؼ� ��� ������ �� ���� ���� ������ ��� ����
        if (mode == 1)
        {
            judi_Coroutine = StartCoroutine(PlayChaseScream());
        } else
        {           
            StopCoroutine(judi_Coroutine);
            audioSource[1].Stop();
            Debug.Log("�ڷ�ƾ ����");
        }
    }

    public IEnumerator PlayChaseScream()
    {
        audioSource[1].clip = ScreamSounds[0];
        yield return new WaitForSeconds(0.5f);
        audioSource[1].Play();
        yield return new WaitForSeconds(audioSource[1].clip.length);
        while (true)
        {
            audioSource[1].clip = ScreamSounds[Random.Range(1,ScreamSounds.Length)];

            if (!audioSource[1].isPlaying)
            {
                audioSource[1].Play();
                yield return new WaitForSeconds(audioSource[1].clip.length);
            }
        }
    }

    public void SwitchStepSounds(int mode)
    {
        // �Ѿ�� ���� 1�̸� �޸��� ����, 1�� �ƴϸ� �ȴ� ����� 
        StepSounds = mode == 1 ? footStepRunSounds : footStepSounds; 
    }
}