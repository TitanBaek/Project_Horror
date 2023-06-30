using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;
using MonsterState;

public enum M_State { Chase, Return, Attack, Patrol, Hit, Die, Size }
public enum M_SubState { Idle ,Hit, Size }
public class Judi : Monster,IHitable
{
    private Judi_Eyes eyes;
    public Judi_Eyes Eyes { get { return eyes; } set { eyes = value; } }

    private GameObject player;
    public GameObject Player { get { return player; } }
    private Player player_State;
    public Player Player_State { get { return player_State; } }



    // ���¸ӽ� ����
    private M_State curState;
    public M_State CurState { get { return curState; } }
    // ���¸ӽ� ����
    private M_SubState curSubState;
    public M_SubState CurSubState { get { return curSubState; } }

    private StateBase<Judi>[] states;
    private StateBase<Judi>[] sub_states;

    // ���� ����� Ŭ�� �� �ҽ�
    [SerializeField] private AudioClip[] footStepSounds;
    [SerializeField] private AudioClip[] footStepRunSounds;
    private AudioSource[] audioSource;
    public AudioSource[] AudioSource { get { return audioSource; } set { audioSource = value; } }
    private AudioClip[] StepSounds;
    [SerializeField] private AudioClip[] ScreamSounds;
    private int footStepIndex = 0;

    // ���� �ڷ�ƾ 
    Coroutine judi_Coroutine;
    private Coroutine attack_coroutine;
    public Coroutine Attack_coroutine { get { return attack_coroutine; } set { attack_coroutine = value; } }
    private Coroutine doChase_coroutine;
    public Coroutine DoChase_Coroutine { get { return doChase_coroutine; } set { doChase_coroutine = value; } }



    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponents<AudioSource>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        eyes = GetComponentInChildren<Judi_Eyes>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;
        player_State = player.GetComponent<Player>();

        states = new StateBase<Judi>[(int)M_State.Size];
        states[(int)M_State.Chase] = new ChaseState(this);
        states[(int)M_State.Return] = new ReturnState(this);
        states[(int)M_State.Attack] = new AttackState(this);
        states[(int)M_State.Patrol] = new PatrolState(this);
        states[(int)M_State.Hit] = new HitState(this);
        states[(int)M_State.Die] = new DieState(this);
        curState = M_State.Patrol;

        
        sub_states = new StateBase<Judi>[(int)M_SubState.Size];
        sub_states[(int)M_SubState.Idle] = new IdleState(this);
        sub_states[(int)M_SubState.Hit] = new HitState(this);
        curSubState = M_SubState.Idle;

        StepSounds = footStepSounds;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        ChangeState(M_State.Patrol);
    }
    private void Update()
    {
        states[(int)curState].Update();         // ���� ���¿� ���� Update�Լ� ȣ��
        states[(int)curState].Transition();

        sub_states[(int)curSubState].Update();         // ���� ���¿� ���� Update�Լ� ȣ��
        sub_states[(int)curSubState].Transition();
    }
    private void LateUpdate()
    {
        if (curState == M_State.Die)
            return;                     // ������ �Ӹ� ��������
        HeadBanging();
    }

    private void HeadBanging()
    {
        MonsterHead.transform.localRotation = Quaternion.Euler(Random.Range(-40, 40), Random.Range(-40, 40), Random.Range(-40, 40));
    }

    public void ChangeState(M_State state)
    {
        Debug.Log(state);
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Setup();
        states[(int)curState].Enter();
    }
    public void ChangeState(M_SubState state)
    {
        Debug.Log(state);
        sub_states[(int)curSubState].Exit();
        curSubState = state;
        sub_states[(int)curSubState].Setup();
        sub_states[(int)curSubState].Enter();
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

    public void Stun()
    {
        Debug.Log("���Ͱ� �¾Ҵ�.");
        ChangeState(M_SubState.Hit);
    }

    public void TakeHit(RaycastHit hit, int dmg)
    {
        curHp -= dmg;
        if(curHp <= 0)
        {
            curHp = 0;
            ChangeState(M_State.Die);
        }
        Debug.Log($"���� ���ϰ� �� ���� ������ ü�� {curHp}/{maxHp}");
        ParticleSystem effect = GameManager.Resource.Instantiate<ParticleSystem>("Effect/MonsterHit", hit.point, Quaternion.LookRotation(hit.normal), true);
        effect.transform.parent = hit.transform.transform;
        GameManager.Resource.Destroy(effect.gameObject, 0.3f);
    }

    public void TakeHit(int dmg)
    {
        curHp -= dmg;
        if (curHp <= 0)
        {
            curHp = 0;
            ChangeState(M_State.Die);
        }
        Debug.Log($"���� ���ϰ� �� ���� ������ ü�� {curHp}/{maxHp}");
    }
}