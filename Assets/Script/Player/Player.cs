using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerAudio, IHitable
{
    private Animator anim;
    [SerializeField] int maxHp;
    [SerializeField] private int curHp; 
    [SerializeField] private GameObject[] hitPoints;
    [SerializeField] private AudioClip[] hitAudios;
    private PlayerMove playerMove;

    private void Awake()
    {
        curHp = maxHp;
        audioSource = GetComponents<AudioSource>();
        playerMove = GetComponent<PlayerMove>();
        anim = GetComponent<Animator>();
    }
     
    private void OnDisable()
    {
        StopCoroutine(Hit());
    }
    public void Stun()
    {
        StopMoving();
        StartCoroutine(Hit());
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
        yield return new WaitForSeconds(0.5f);
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
        anim.SetBool("Dying", true);
    }

    public void GameOver()
    {
        GameManager.Scene.LoadScene("GameOverScene");
    }


}
