using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public enum State { Idle, Chase, Return, Attack, Patrol, Hit, Die, Size }
public class Judi : Monster
{
    // 상태머신 구현
    private State curState;
    public State CurState { get { return curState; } }
    private StateBase<Judi>[] states;
    [SerializeField] private AudioClip[] footStepSounds;
    [SerializeField] private AudioClip[] footStepRunSounds;
    private AudioSource[] audioSource;
    private AudioClip[] StepSounds;
    [SerializeField] private AudioClip[] ScreamSounds;
    private int footStepIndex = 0;

    private void Awake()
    {
        audioSource = GetComponents<AudioSource>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        states = new StateBase<Judi>[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Chase] = new ChaseState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Patrol] = new PatrolState(this);
        states[(int)State.Hit] = new HitState(this);
        states[(int)State.Die] = new DieState(this);
        curState = State.Idle;
        StepSounds = footStepSounds;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        ChangeState(State.Idle);
    }
    private void Update()
    {
        states[(int)curState].Update();         // 현재 상태에 대한 Update함수 호출
    }

    public void ChangeState(State state)
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


        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + AttackAngle * 0.5f); // 대상이 바라보고 있는 각도 + 앵글의 1/2
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - AttackAngle * 0.5f);  // 대상이 바라보고 있는 각도 - 앵글의 1/2
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
        if (!audioSource[0].isPlaying)                         // 현재 오디오가 재생중이 아니라면 재생
        {
            audioSource[0].clip = StepSounds[footStepIndex++]; // 오디오 재생과 인덱스를 증감 시켜줌
            audioSource[0].Play();                                 // 오디오 재생
            if (footStepIndex >= 3)                         // 인덱스 초기화 부분
                footStepIndex = 0;
        }
    }

    public void PlayerScreamSound(int mode)
    {
        // 1이 넘어오면 코루틴 시작해서 비명 지르게 그 외의 값이 들어오면 비명 스톱
        if (mode == 1)
        {
            StartCoroutine(PlayChaseScream());
        } else
        {           
            StopCoroutine(PlayChaseScream());
            audioSource[1].Stop();
        }
    }

    public IEnumerator PlayChaseScream()
    {
        audioSource[1].clip = ScreamSounds[0];
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
        // 넘어온 값이 1이면 달리기 사운드, 1이 아니면 걷는 사운드로 
        StepSounds = mode == 1 ? footStepRunSounds : footStepSounds; 
    }
}