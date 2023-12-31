using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class HitState : StateBase<Player>
    {
        private Coroutine hit_Coroutine;
        private bool hit_Finished;

        public HitState(Player owner) : base(owner) { }

        public override void Enter()
        {
            hit_Finished = false;

            hit_Coroutine = owner.StartCoroutine(Hit());
        }

        public override void Exit()
        {
            owner.StopCoroutine(hit_Coroutine);
        }

        public override void LateUpdate()
        {
        }

        public override void Setup()
        {
        }

        public override void Transition()
        {
            if(hit_Finished)
            {
                owner.ChangeState(P_State.Chase);
            }
        }

        public override void Update()
        {
        }

        public IEnumerator Hit() // 히트 상태에서 실행되게
        {
            int index = Random.Range(0, owner.HitPoints.Length);
            owner.TakeHit(15);
            PlayHitSound();
            yield return new WaitForSeconds(0.3f);
            SwitchParticle(index);
            yield return new WaitForSeconds(0.3f);
            owner.Anim.SetTrigger("Hit");
            CameraShake.Instance.ShakeCamera(3f, 0.5f);
            yield return new WaitForSeconds(0.15f);
            SwitchParticle(index);
            owner.StartMoving();
            yield return new WaitForSeconds(0.3f);
            hit_Finished = true;
        }

        public void PlayHitSound() // 히트 상태에서 실행되게
        {
            owner.AudioSources[2].clip = owner.HitAudios[Random.Range(0, owner.HitAudios.Length)];
            if (!owner.AudioSources[2].isPlaying)
            {
                owner.AudioSources[2].Play();
            }
        }

        public void SwitchParticle(int index) // 히트 상태에서 실행되게
        {
            // 파티클이 활성화 되어있는지 확인하여 on off 하는 조건문
            if (owner.HitPoints[index].activeSelf)
            {
                owner.HitPoints[index].SetActive(false);
            }
            else
            {
                owner.HitPoints[index].SetActive(true);

            }
        }
    }
}