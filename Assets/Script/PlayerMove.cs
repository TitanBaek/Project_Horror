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
            AudioSourceVolume(0);
        }
        else if (isWalking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
            AudioSourceVolume(1);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }
        anim.SetFloat("xSpeed", moveDir.x, 0.5f, Time.deltaTime); 
        anim.SetFloat("ySpeed", moveDir.z, 0.5f, Time.deltaTime);
        anim.SetFloat("MoveSpeed", moveSpeed, 0.5f, Time.deltaTime);
        //controller.Move(moveDir * moveSpeed * Time.deltaTime);
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

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
    private void PlayFootStep()
    {
        if (!audioSource.isPlaying)
        { 
        audioSource.clip = footStepSounds[footStepIndex++];
        audioSource.Play();
            if(footStepIndex >= 10)
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
