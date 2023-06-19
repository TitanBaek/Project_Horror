using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDir;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private AudioClip[] footStepSounds;
    private int footStepIndex = 0;

    private bool isWalking = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        Move();
        DoGravity();
    }

    private void Move()
    {
        if(moveDir.magnitude == 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);
            AudioSourceVolume(0);                               // 움직임이 멈추면 오디오 소스 볼륨을 줄이는 함수 출력
        }
        else if (isWalking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
            AudioSourceVolume(1);                               // 움직임이 있다면 오디오 소스 볼륨을 키우는 함수 출력
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }
        anim.SetFloat("xSpeed", moveDir.x, 0.5f, Time.deltaTime); 
        anim.SetFloat("ySpeed", moveDir.z, 0.5f, Time.deltaTime);
        anim.SetFloat("MoveSpeed", moveSpeed, 0.5f, Time.deltaTime);
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 오디오 소스 볼륨 조절 함수(발걸음 함수에서 참조)
    /// </summary>
    /// <param name="mode"></param>
    private void AudioSourceVolume(int mode)
    {
        if(mode == 0)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, 0.05f);
        } else if(mode == 1)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0.25f, 0.1f);

        }
    }
    /// <summary>
    /// 발걸음 소리 출력 함수
    /// </summary>
    private void PlayFootStep() 
    {
        if (!audioSource.isPlaying)                         // 현재 오디오가 재생중이 아니라면 재생
        { 
        audioSource.clip = footStepSounds[footStepIndex++]; // 오디오 재생과 인덱스를 증감 시켜줌
        audioSource.Play();                                 // 오디오 재생
            if(footStepIndex >= 10)                         // 인덱스 초기화 부분
                footStepIndex = 0;
        } 
    }

    private void DoGravity()
    {
        // 내려오게..
    }
    private void OnMove(InputValue value)
    {
        moveDir = new Vector3(value.Get<Vector2>().x,0,value.Get<Vector2>().y);
    }

    private void OnLight(InputValue value)
    {
        if (flashLight.active)
        {
            flashLight.SetActive(false);
        } else
        {
            flashLight.SetActive(true);
        }
    }
}
