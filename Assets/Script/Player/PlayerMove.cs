using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : PlayerAudio
{
    [SerializeField] private AudioClip[] footStepSounds;
    [SerializeField] private AudioClip[] footStepRunSounds;
    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDir;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject FlashLight_Particle;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float ySpeed;

    private AudioClip[] StepSounds;
    private int footStepIndex = 0;

    private float canMove = 0;
    public float CanMove { get { return canMove; } set { canMove = value; }  }
    private bool isWalking = true;

    public float MoveSpeed { get { return moveSpeed; } }

    private void Awake()
    {
        StepSounds = footStepSounds;
        audioSource = GetComponents<AudioSource>();
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
            StepSounds = footStepSounds;
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
            AudioSourceVolume(1);                               // 움직임이 있다면 오디오 소스 볼륨을 키우는 함수 출력
        }
        else
        {
            StepSounds = footStepRunSounds;
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
            AudioSourceVolume(1);                               // 움직임이 있다면 오디오 소스 볼륨을 키우는 함수 출력
        };
        anim.SetFloat("xSpeed", moveDir.x * moveSpeed - canMove, 0.5f, Time.deltaTime); 
        anim.SetFloat("ySpeed", moveDir.z * moveSpeed - canMove, 0.5f, Time.deltaTime);
        anim.SetFloat("MoveSpeed", moveSpeed - canMove, 0.5f, Time.deltaTime);
        controller.Move(transform.forward * moveDir.z * (moveSpeed - canMove) * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * (moveSpeed - canMove) * Time.deltaTime);
    }

    /// <summary>
    /// 오디오 소스 볼륨 조절 함수(발걸음 함수에서 참조)
    /// </summary>
    /// <param name="mode"></param>
    private void AudioSourceVolume(int mode)
    {
        if(mode == 0)
        {
            audioSource[0].volume = Mathf.Lerp(audioSource[0].volume, 0, 0.05f); 
        } else if(mode == 1)
        {
            audioSource[0].volume = Mathf.Lerp(audioSource[0].volume, 0.10f, 0.1f);

        }
    }

    private void DoGravity()
    {
        // 내려오게..
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0)
            ySpeed = -1;

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private bool GroundCheck()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f); // 어디부터, 어느정도의 둘레짜리를, 어느방향으로, 얼마만큼의 길이로 쏠건지

    }
    private void OnRun(InputValue value)
    {
        if (value.isPressed)
        {
            isWalking = false;
            Debug.Log(isWalking);
        }
        else
        {
            isWalking = true;
            Debug.Log(isWalking);
        }
    }

    private void OnMove(InputValue value)
    {
        moveDir = new Vector3(value.Get<Vector2>().x,0,value.Get<Vector2>().y);
    }

    private void OnLight(InputValue value)
    {
        if (audioSource[1].isPlaying)
        {
            return;
        }
        audioSource[1].Play();
        if (flashLight.activeSelf)
        {
            FlashLight_Particle.SetActive(false);
            flashLight.SetActive(false);
        } else
        {
            FlashLight_Particle.SetActive(true);
            flashLight.SetActive(true);
        }
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
}
