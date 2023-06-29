using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : PlayerAudio, IHitable
{
    [SerializeField] int maxHp;
    [SerializeField] private int curHp;
    [SerializeField] private GameObject[] hitPoints;
    [SerializeField] private AudioClip[] hitAudios;

    private Animator anim;
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;

    private Coroutine hit_Coroutine;

    [SerializeField] private UnityEvent onHit;

    private void Awake()
    {
        curHp = maxHp;
        playerAttack = GetComponent<PlayerAttack>();
        audioSource = GetComponents<AudioSource>();
        playerMove = GetComponent<PlayerMove>();
        anim = GetComponent<Animator>();
    }
     
    private void OnDisable()
    {
        StopCoroutine(hit_Coroutine);
    }
    public void Stun()
    {
        StopMoving();
        hit_Coroutine = StartCoroutine(Hit());
    }

    public void TakeHit(int dmg) 
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

    public void StopMoving() 
    {
        playerMove.CanMove = playerMove.MoveSpeed / 1.5f;
    }

    public void StartMoving()
    {
        playerMove.CanMove = 0;
    }

    public IEnumerator Hit()
    {
        int index = Random.Range(0, hitPoints.Length);
        PlayHitSound();
        yield return new WaitForSeconds(0.3f);
        SwitchParticle(index);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Hit", true);
        CameraShake.Instance.ShakeCamera(3f, .5f);
        yield return new WaitForSeconds(0.2f);
        SwitchParticle(index);
        anim.SetBool("Hit", false);
        StartMoving();
    }

    public void PlayHitSound()
    {
        audioSource[2].clip = hitAudios[Random.Range(0, hitAudios.Length)];
        if (!audioSource[2].isPlaying)
        {
            audioSource[2].Play();
        }
    }


    public void SwitchParticle(int index)
    {
        if (hitPoints[index].active)
        {
            hitPoints[index].SetActive(false);
        } else
        {
            hitPoints[index].SetActive(true);

        }
    }

    public void Dead()
    {
        playerMove.CanMove = playerMove.MoveSpeed;
        anim.SetBool("Dying", true);
    }

    public void GameOver()
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
