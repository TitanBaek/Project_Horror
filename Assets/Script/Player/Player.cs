using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayerState;

public enum P_State { Idle , Chase , Hit , Die , Size };
public enum P_Health_State { Normal, Hurts, Deadly, Size};

public class Player : PlayerAudio, IHitable
{

    private P_State curState;
    private P_Health_State cur_HealthState;

    private StateBase<Player>[] states;
    private StateBase<Player>[] h_states;

    [SerializeField] int maxHp;
    [SerializeField] private int curHp;
    [SerializeField] private GameObject[] hitPoints;
    [SerializeField] private AudioClip[] hitAudios;

    private Animator anim;
    private PlayerMove playerMove;
    private PlayerAttacker playerAttacker;

    private Coroutine hit_Coroutine;

    [SerializeField] private UnityEvent onHit;
    public P_State CurState { get { return curState; } }
    public P_Health_State Cur_HealthState { get { return cur_HealthState; } }
    public GameObject[] HitPoints { get { return hitPoints; } }
    public AudioClip[] HitAudios { get { return hitAudios; } }
    public Animator Anim { get { return anim; } set { anim = value; } }
    public Coroutine Hit_Coroutine { get { return hit_Coroutine; } set { hit_Coroutine = value; } }
    public AudioSource[] AudioSources { get { return audioSource; } }

    private void Awake()
    {
        curHp = maxHp;
        playerAttacker = GetComponent<PlayerAttacker>();
        audioSource = GetComponents<AudioSource>();
        playerMove = GetComponent<PlayerMove>();
        anim = GetComponent<Animator>();

        // 상태머신 생성
        states = new StateBase<Player>[(int)P_State.Size];
        states[(int)P_State.Idle] = new IdleState(this);
        states[(int)P_State.Chase] = new ChaseState(this);
        states[(int)P_State.Hit] = new HitState(this);
        states[(int)P_State.Die] = new DieState(this);
        curState = P_State.Idle;
        ChangeState(curState);

        // HP상태머신 생성
        h_states = new StateBase<Player>[(int)P_Health_State.Size];
        h_states[(int)P_Health_State.Normal] = new NormalState(this);
        h_states[(int)P_Health_State.Hurts] = new HurtState(this);
        h_states[(int)P_Health_State.Deadly] = new DeadlyState(this);
        cur_HealthState = P_Health_State.Normal;
        ChangeState(cur_HealthState);
    }

    public void Update()
    {
        states[(int)curState].Update();
        h_states[(int)cur_HealthState].Update();
        states[(int)curState].Transition();
        h_states[(int)cur_HealthState].Transition();
    }

    public void ChangeState(P_State state)
    {
        Debug.Log(state);
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Setup();
        states[(int)curState].Enter();
    }    
    
    public void ChangeState(P_Health_State state)
    {
        Debug.Log(state);
        h_states[(int)cur_HealthState].Exit();
        cur_HealthState = state;
        h_states[(int)cur_HealthState].Setup();
        h_states[(int)cur_HealthState].Enter();
    }

    public void SpottedByEnemy()
    {
        ChangeState(P_State.Chase);
    }

    public void RunAwaySuccesse()
    {
        ChangeState(P_State.Idle);

    }

    public void Stun() // 이대로 두고 상태 전환( Hit 로 )
    {
        SlowMoving();
        ChangeState(P_State.Hit);// Hit 상태로 변경
    }

    public void TakeHit(int dmg)
    {
        curHp -= dmg;
        if (curHp <= 0)
        {
            curHp = 0;
            Dead();
        }
        else
        {
        }
        Debug.Log($"Player HP: {curHp}/{maxHp}");
        CurrentHp();
    }
    public void TakeHit(RaycastHit hit, int dmg) // 히트 상태에서 실행되게
    {
        curHp -= dmg;
        if(curHp <= 0)
        {
            curHp = 0;
            Dead();
        }else
        {
        }
        Debug.Log($"Player HP: {curHp}/{maxHp}");
        CurrentHp();
    }

    public void SlowMoving() // 이건 그냥 내버려두기 
    {
        playerMove.CanMove = playerMove.MoveSpeed / 1.5f;
    }

    public void StopMoving() // 이건 그냥 내버려두기 
    {
        playerMove.CanMove = playerMove.MoveSpeed;
    }

    public void StartMoving() // 이건 그냥 내버려두기 
    {
        playerMove.CanMove = 0;
    }

    public void Dead() // 다이 상태에서 실행되게
    {
        playerMove.CanMove = playerMove.MoveSpeed;
        anim.SetBool("Dying", true);
    }

    public void GameOver() // 다이 상태에서 실행되게
    {
        GameManager.Scene.LoadScene("GameOverScene");
    }

    // 현재 남은체력에 대한 수치값을 HurtScreenUI의 SetAlpha에 매개변수로 넘겨서 화면에 피칠갑되게 
    public void CurrentHp()
    {        
        float hpPercent = 100 - ((curHp / maxHp) * 100);
        Debug.Log($"100 - ({curHp}/{maxHp} * 100) = {hpPercent}");
        GameManager.UI.hurtScreenUI.SetAlpha(hpPercent / 100);
    }


}
